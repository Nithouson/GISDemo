using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISDemo
{
    public class Arc
    {
        public List<int> pid;
        public int lpoly=-1, rpoly=-1;
        public Arc()
        {
            pid = new List<int>();
        }
        public Arc(List<int> nodes)
        {
            pid = new List<int>();
            pid.AddRange(nodes);
        }

        public int start
        {
            get { return pid[0]; }
        }
        
        public int end
        {
            get { return pid[pid.Count - 1]; }
        }
    }
    
    public class Polygon
    {
        public List<int> arcs;
        public HashSet<int> npoly;
        public Polygon()
        {
            arcs = new List<int>();
            npoly = new HashSet<int>();
        }
        public string Arcs_String()
        {
            string s = "";
            for(int i=0;i<arcs.Count;i++)
            {
                if (i > 0) s += ",";
                s += arcs[i].ToString();
            }
            return s;
        }

        public string Npolys_String()
        {
            string s = "";
            List<int> nplist = new List<int>(npoly);
            for (int i = 0; i < nplist.Count; i++)
            {
                if (i > 0) s += ",";
                s += nplist[i].ToString();
            }
            return s;
        }
    }

    public class PolygonData
    {
        public List<PointD> points;
        public List<Arc> arcs; //下标从1开始
        public List<Polygon> polys; //正下标：s到e;负下标：e到s.
        public int selp = -1;
        public PolygonData()
        {
            points = new List<PointD>();
            arcs = new List<Arc>();
            arcs.Add(new Arc());
            polys = new List<Polygon>();
        }

        public List<PointD> ArcPoints(int id)
        {
            List<PointD> res = new List<PointD>();
            Arc a = arcs[id];
            for(int p=0;p<a.pid.Count;p++)
            {
                res.Add(points[a.pid[p]]);
            }
            return res;
        }

        public int ArcPointID(int id,int no)
        {
            int uid = id;
            if (uid < 0) uid = -id;
            Arc a = arcs[uid];
            if(id>0)
            {
                return a.pid[no];
            }
            else
            {
                return a.pid[a.pid.Count - 1 - no];
            }
        }

        public List<PointD> PolygonPoints(int id)
        {
            List<PointD> res = new List<PointD>();
            Polygon p = polys[id];
            Arc a;
            for(int i=0;i<p.arcs.Count;i++)
            {
                if (p.arcs[i] > 0)
                {
                    a = arcs[p.arcs[i]];
                    for(int s=0;s<a.pid.Count-1;s++)
                    {
                        res.Add(points[a.pid[s]]);
                    }
                }
                else
                {
                    a = arcs[-p.arcs[i]];
                    for (int s = a.pid.Count-1; s >=1; s--)
                    {
                        res.Add(points[a.pid[s]]);
                    }
                }
            }
            return res;
        }

        public List<int> PolygonPointIDs(int id)
        {
            //多边形顶点序列；最后一个点闭合到第一个点
            List<int> res = new List<int>();
            Polygon p = polys[id];
            Arc a;
            for (int i = 0; i < p.arcs.Count; i++)
            {
                if (p.arcs[i] > 0)
                {
                    a = arcs[p.arcs[i]];
                    for (int s = 0; s < a.pid.Count - 1; s++)
                    {
                        res.Add(a.pid[s]);
                    }
                }
                else
                {
                    a = arcs[-p.arcs[i]];
                    for (int s = a.pid.Count - 1; s >= 1; s--)
                    {
                        res.Add(a.pid[s]);
                    }
                }
            }
            return res;
        }

        public List<int> LinkedArcs(int pid)
        {
            List<int> aid = new List<int>();
            for(int a=1;a<arcs.Count;a++)
            {
                if (arcs[a].start == pid) aid.Add(a);
                else if (arcs[a].end == pid) aid.Add(-a);
            }
            return aid;
        }

        public int Arc_Start(int aid)
        {
            if (aid > 0) return arcs[aid].start;
            else return arcs[-aid].end;
        }

        public int Arc_End(int aid)
        {
            if (aid > 0) return arcs[aid].end;
            else return arcs[-aid].start;
        }

        public double Polygon_Perimeter(int pid)
        {
            List<PointD> polypts = PolygonPoints(pid);
            int pnum = polypts.Count;
            double peri = 0;
            for(int i=0;i<pnum-1;i++)
            {
                peri += GeomTools.Dist2D(polypts[i].X, polypts[i].Y, polypts[i + 1].X, polypts[i + 1].Y);
            }
            peri += GeomTools.Dist2D(polypts[pnum-1].X, polypts[pnum - 1].Y, polypts[0].X, polypts[0].Y);
            return peri;
        }

        public double Polygon_Area(int pid,double XBias = 38428000, double YBias = 3737000)
        {
            List<PointD> polypts = PolygonPoints(pid);
            for(int p=0;p<polypts.Count;p++)
            {
                polypts[p] = new PointD(polypts[p].X - XBias, polypts[p].Y - YBias);
            }
            return GeomTools.PolygonArea(polypts);
        }

        public void FromGridContour(List<List<PointD>> contour, double Xmin, double Xmax, double Ymin, double Ymax)
        {
            //预存附加结点
            double Xmid = (Xmin + Xmax) / 2, Ymid = (Ymin + Ymax) / 2;
            points.Add(new PointD(Xmin, Ymax));//0
            points.Add(new PointD(Xmid, Ymax));//1
            points.Add(new PointD(Xmax, Ymax));//2
            points.Add(new PointD(Xmin, Ymid));//3
            points.Add(new PointD(Xmid, Ymid));//4
            points.Add(new PointD(Xmax, Ymid));//5
            points.Add(new PointD(Xmin, Ymin));//6
            points.Add(new PointD(Xmid, Ymin));//7
            points.Add(new PointD(Xmax, Ymin));//8


            //存储已知结点
            List<Arc> told = new List<Arc>(), tnew;
            int s, pts;
            for (int c = 0; c < contour.Count; c++)
            {
                pts = contour[c].Count;
                Arc a = new Arc();
                for (int p = 0; p < pts; p++)
                {
                    if (RepeatCheck(contour[c][p]) > 0)
                    {
                        a.pid.Add(RepeatCheck(contour[c][p]));
                    }
                    else
                    {
                        a.pid.Add(points.Count);
                        points.Add(contour[c][p]);
                    }
                }
                told.Add(a);
            }

            //打断生成弧段：从外边框到内分割线
            HashSet<int> cutpoint; //分割线的断点
            List<int> cutplist;
            List<int> curnode; //等值线的断点
            PointD ps, pt, pc;
            Arc temp;
            double eps = 1e-7;
            int nodes;

            int[] sid = { 0, 1, 6, 7, 0, 2, 3, 5, 3, 4, 1, 4, 0, 2, 6, 8 };
            int[] tid = { 1, 2, 7, 8, 3, 5, 6, 8, 4, 5, 4, 7, 4, 4, 4, 4 };

            //横向外边框
            for (int b = 0; b < 4; b++)
            {
                cutpoint = new HashSet<int>();
                ps = points[sid[b]];
                pt = points[tid[b]];
                cutpoint.Add(sid[b]);
                cutpoint.Add(tid[b]);
                for (int a = 0; a < told.Count; a++)
                {
                    nodes = told[a].pid.Count;
                    pc = points[told[a].start];
                    if (Math.Abs(pc.Y - ps.Y) < eps && ps.X < pc.X && pt.X > pc.X)
                    {
                        cutpoint.Add(told[a].start);
                    }
                    pc = points[told[a].end];
                    if (Math.Abs(pc.Y - ps.Y) < eps && ps.X < pc.X && pt.X > pc.X)
                    {
                        cutpoint.Add(told[a].end);
                    }
                }
                cutplist = new List<int>(cutpoint);
                cutplist.Sort((x, y) => { return (points[x].X.CompareTo(points[y].X)); });
                for (int q = 0; q < cutpoint.Count - 1; q++)
                {
                    temp = new Arc();
                    temp.pid.Add(cutplist[q]);
                    temp.pid.Add(cutplist[q + 1]);
                    arcs.Add(temp);
                }
            }

            //纵向外边框
            for (int b = 4; b < 8; b++)
            {
                cutpoint = new HashSet<int>();
                ps = points[sid[b]];
                pt = points[tid[b]];
                cutpoint.Add(sid[b]);
                cutpoint.Add(tid[b]);
                for (int a = 0; a < told.Count; a++)
                {
                    nodes = told[a].pid.Count;
                    pc = points[told[a].start];
                    if (Math.Abs(pc.X - ps.X) < eps && ps.Y > pc.Y && pt.Y < pc.Y)
                    {
                        cutpoint.Add(told[a].start);
                    }
                    pc = points[told[a].end];
                    if (Math.Abs(pc.X - ps.X) < eps && ps.Y > pc.Y && pt.Y < pc.Y)
                    {
                        cutpoint.Add(told[a].end);
                    }
                }
                cutplist = new List<int>(cutpoint);
                cutplist.Sort((x, y) => { return (points[x].Y.CompareTo(points[y].Y)); });
                for (int q = 0; q < cutpoint.Count - 1; q++)
                {
                    temp = new Arc();
                    temp.pid.Add(cutplist[q]);
                    temp.pid.Add(cutplist[q + 1]);
                    arcs.Add(temp);
                }
            }

            //横向内边框
            for (int b = 8; b < 10; b++)
            {
                cutpoint = new HashSet<int>();
                ps = points[sid[b]];
                pt = points[tid[b]];
                cutpoint.Add(sid[b]);
                cutpoint.Add(tid[b]);
                tnew = new List<Arc>();
                for (int a = 0; a < told.Count; a++)
                {
                    nodes = told[a].pid.Count;
                    curnode = new List<int>();
                    for (int p = 0; p < nodes - 1; p++)
                    {
                        curnode.Add(told[a].pid[p]);
                        PointD pint = GeomTools.Intersect(ps, pt, points[told[a].pid[p]], points[told[a].pid[p + 1]]);
                        if (pint.X > 0 && pint.Y > 0)
                        {
                            if(RepeatCheck(pint)>=0)
                            {
                                s = RepeatCheck(pint);
                            }
                            else
                            {
                                s = points.Count;
                                points.Add(pint);
                            }
                            if(s!=curnode[curnode.Count-1]) curnode.Add(s);
                            tnew.Add(new Arc(curnode));
                            curnode.Clear();
                            curnode.Add(s);
                            cutpoint.Add(s);

                        }
                    }
                    if (!GeomTools.Identical(points[told[a].pid[nodes - 1]], points[curnode[0]]) || curnode.Count>1)
                    {
                        curnode.Add(told[a].pid[nodes - 1]);
                        tnew.Add(new Arc(curnode));
                    }
                    
                }
                told.Clear();
                told.AddRange(tnew);
                cutplist = new List<int>(cutpoint);
                cutplist.Sort((x, y) => { return (points[x].X.CompareTo(points[y].X)); });
                for (int q = 0; q < cutpoint.Count - 1; q++)
                {
                    temp = new Arc();
                    temp.pid.Add(cutplist[q]);
                    temp.pid.Add(cutplist[q + 1]);
                    arcs.Add(temp);
                }
            }

            //纵向内边框、对角线
            for (int b = 10; b < 16; b++)
            {
                cutpoint = new HashSet<int>();
                ps = points[sid[b]];
                pt = points[tid[b]];
                cutpoint.Add(sid[b]);
                cutpoint.Add(tid[b]);
                tnew = new List<Arc>();
                for (int a = 0; a < told.Count; a++)
                {
                    nodes = told[a].pid.Count;
                    curnode = new List<int>();
                    for (int p = 0; p < nodes - 1; p++)
                    {
                        curnode.Add(told[a].pid[p]);
                        PointD pint = GeomTools.Intersect(ps, pt, points[told[a].pid[p]], points[told[a].pid[p + 1]]);
                        if (pint.X > 0 && pint.Y > 0)
                        {
                            if (RepeatCheck(pint) >= 0)
                            {
                                s = RepeatCheck(pint);

                            }
                            else
                            {
                                s = points.Count;
                                points.Add(pint);
                            }
                            if (s != curnode[curnode.Count - 1]) curnode.Add(s);
                            tnew.Add(new Arc(curnode));
                            curnode.Clear();
                            curnode.Add(s);
                            cutpoint.Add(s);
                        }
                    }
                    if (!GeomTools.Identical(points[told[a].pid[nodes - 1]], points[curnode[0]]) || curnode.Count > 1)
                    {
                        curnode.Add(told[a].pid[nodes - 1]);
                        tnew.Add(new Arc(curnode));
                    }
                }
                told.Clear();
                told.AddRange(tnew);
                cutplist = new List<int>(cutpoint);
                cutplist.Sort((x, y) => { return (points[x].Y.CompareTo(points[y].Y)); });
                for (int q = 0; q < cutpoint.Count - 1; q++)
                {
                    temp = new Arc();
                    temp.pid.Add(cutplist[q]);
                    temp.pid.Add(cutplist[q + 1]);
                    arcs.Add(temp);
                }
            }

            arcs.AddRange(told);

            //搜索多边形 
            int pnum = points.Count;
            for(int p=0;p<pnum;p++)
            {
                List<int> arc_p = LinkedArcs(p);
                for(int q=0;q<arc_p.Count;q++)
                {
                    Polygon newpoly = new Polygon();
                    newpoly.arcs.Add(arc_p[q]);
                    ClosePolygon(newpoly,  arc_p[q], p);
                    if(!RepeatCheck(newpoly))
                    {
                        polys.Add(newpoly);
                    }
                }
            }

            //去除外包多边形
            for(int p=0;p<polys.Count;p++)
            {
                HashSet<int> pidset = new HashSet<int>(PolygonPointIDs(p));
                if(pidset.Contains(0) && pidset.Contains(2) && pidset.Contains(6) && pidset.Contains(8))
                {
                    polys.RemoveAt(p);
                    break;
                }
            }
 

            //生成弧段的左右多边形、多边形的相邻多边形
            for (int pid=0;pid<polys.Count;pid++)
            {
                Polygon p = polys[pid];
                int aid;
                for(int u=0;u<p.arcs.Count;u++)
                {
                    aid = p.arcs[u];
                    if(aid>0)
                    {
                        arcs[aid].lpoly = pid;
                    }
                    else
                    {
                        arcs[-aid].rpoly = pid;
                    }
                }
            }
            for (int pid = 0; pid < polys.Count; pid++)
            {
                Polygon p = polys[pid];
                int aid;
                for (int u = 0; u < p.arcs.Count; u++)
                {
                    aid = p.arcs[u];
                    if (aid > 0)
                    {
                        if(arcs[aid].rpoly>=0) polys[pid].npoly.Add(arcs[aid].rpoly);
                    }
                    else
                    {
                        if (arcs[-aid].lpoly >= 0) polys[pid].npoly.Add(arcs[-aid].lpoly);
                    }
                }
            }
        }


        private void ClosePolygon(Polygon pc, int from_e,int end_p)
        {
            int cur_p = Arc_End(from_e);
            List<int> arc_p = LinkedArcs(cur_p);
            List<int> dirs = new List<int>();
            arc_p.Remove(-from_e);
            for(int br=0;br<arc_p.Count;br++)
            {
                dirs.Add(ArcPointID(arc_p[br], 1));
            }
            int out_e = arc_p[ChooseEdge(cur_p, ArcPointID(-from_e, 1), dirs)];
            pc.arcs.Add(out_e);
            if(Arc_End(out_e)!=end_p)
            {
                ClosePolygon(pc, out_e, end_p);
            }
        }

        private int ChooseEdge(int cur,int from,List<int> to)
        {
            double afrom = GeomTools.Direction_Angle(points[cur], points[from]),ato;
            List<Pair> atos = new List<Pair>();
            for(int d=0;d<to.Count;d++)
            {
                ato = GeomTools.Direction_Angle(points[cur], points[to[d]]);
                if (ato < afrom) ato += 2 * Math.PI;
                atos.Add(new Pair(d, ato));
            }
            atos.Sort((x, y) => { return x.val.CompareTo(y.val); });
            int out_id = atos[0].id;
            return out_id;
        }

        private int RepeatCheck(PointD pt)
        {
            for(int p=0;p<points.Count;p++)
            {
                if(GeomTools.Identical(points[p],pt))
                {
                    return p;
                }
            }
            return -1;
        }

        private bool RepeatCheck(Polygon pc)
        {
            bool repeat = false;
            HashSet<int> pca = new HashSet<int>();
            for(int i=0;i<pc.arcs.Count;i++)
            {
                if (pc.arcs[i] < 0) pca.Add(-pc.arcs[i]);
                else pca.Add(pc.arcs[i]);
            }
            for(int p=0;p<polys.Count;p++)
            {
                if (polys[p].arcs.Count != pc.arcs.Count) continue;
                int j,parc;
                for(j=0;j< polys[p].arcs.Count;j++)
                {
                    parc = polys[p].arcs[j];
                    if (parc < 0) parc = -parc;
                    if (!pca.Contains(parc)) break;
                }
                if (j == polys[p].arcs.Count)
                {
                    repeat = true;
                    break;
                }
            }
            return repeat;
        }
        
    }
    
}
