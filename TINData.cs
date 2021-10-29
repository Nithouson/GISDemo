using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISDemo
{
    public class TINtriangle
    {
        public int pa, pb, pc;
        public int ntbc, ntca,ntab;
        public double segbc, segca, segab; //内插等值点在三边的位置；
        public bool valid;
        public TINtriangle(int pa_,int pb_,int pc_,int ntbc_=-1,int ntca_=-1,int ntab_=-1)
        {
            pa = pa_;
            pb = pb_;
            pc = pc_;
            ntab = ntab_;
            ntbc = ntbc_;
            ntca = ntca_; 
            valid = true;
            segbc = segca = segab = -1;
        }

        public void Rotate()
        {
            // (a,b,c)  -> (b,c,a);
            int tp = pa, tn = ntbc;
            double ts=segbc;
            pa = pb;
            pb = pc;
            pc = tp;
            ntbc = ntca;
            ntca = ntab;
            ntab = tn;
            segbc = segca;
            segca = segab;
            segab = ts;
           
        }

        public void Swap()
        {
            // (a,b,c)->(a,c,b)
            int tp = pb,tn = ntca;
            double ts = segca;
            pb = pc;
            pc = tp;
            ntca = ntab;
            ntab = tn;
            segca = 1 - segab;
            segab = 1 - segca;
            segbc = 1 - segbc;

        }

        public void UpdateNeighbor(int t_old,int t_new)
        {
            if (ntbc == t_old) ntbc = t_new;
            else if (ntca == t_old) ntca = t_new;
            else if (ntab == t_old) ntab = t_new;
        }

    }
    

    public class TINData
    {
        public List<PointZ> points;
        public List<TINtriangle> triangles;
        private double M;
        public double BiasX,BiasY;

        public TINData(List<PointRecord> data,double XBias,double YBias)
        {
            points = new List<PointZ>();
            M = 0;
            for(int i=0;i<data.Count;i++)
            {
                points.Add(new PointZ(data[i].X - XBias, data[i].Y - YBias, data[i].Z));
                if (Math.Abs(data[i].X - XBias) > M) M = Math.Abs(data[i].X - XBias);
                if (Math.Abs(data[i].Y - YBias) > M) M = Math.Abs(data[i].Y - YBias);
            }
            triangles = new List<TINtriangle>();
            BiasX = XBias;
            BiasY = YBias;
        }

        public void Delaunay()
        {
            // 建立一个货真价实、如假包换、满足空圆性质的Delaunay三角网

            List<TINtriangle> temptri = new List<TINtriangle>();

            // 包围三角形 编号：N，N+1，N+2
            PointZ pr = new PointZ(3 * M, 0), pu = new PointZ(0, 3 * M), pl = new PointZ(-3 * M, -3 * M);
            int N = points.Count,tc;
            TINtriangle T;
            
            points.Add(pr);
            points.Add(pu);
            points.Add(pl);
            temptri.Add(new TINtriangle(N, N+1, N+2));

            
            int inside;
            PointZ P;
            for(int i=0;i<N;i++)
            {
                P = points[i];
                //找到点所在的三角形
                inside = -1;
                for(int t=0;t< temptri.Count;t++)
                {
                    if (!temptri[t].valid) continue;
                    PointZ  p1, p2,p3;
                    p1 = points[temptri[t].pa];
                    p2 = points[temptri[t].pb];
                    p3 = points[temptri[t].pc];
                    if (GeomTools.Point_in_Triangle(P,p1,p2,p3))
                    {
                        inside = t;
                        break;
                    }
                }

                T = temptri[inside];
                tc = temptri.Count;
                //新加入三角形：id为tc,tc+1,tc+2
                TINtriangle tab, tbc, tca;
                tab = new TINtriangle(i,T.pa, T.pb, T.ntab,tc + 1,tc+2);
                tbc = new TINtriangle(i,T.pb, T.pc, T.ntbc,tc + 2,tc);
                tca = new TINtriangle(i,T.pc, T.pa, T.ntca,tc, tc+1);
                if (T.ntab>0) temptri[T.ntab].UpdateNeighbor(inside, tc);
                if (T.ntbc > 0) temptri[T.ntbc].UpdateNeighbor(inside, tc + 1);
                if (T.ntca > 0) temptri[T.ntca].UpdateNeighbor(inside, tc + 2);
                temptri.Add(tab);
                temptri.Add(tbc);
                temptri.Add(tca);
                T.valid = false;
                LegalizeEdge(points, temptri, tc, 0);
                LegalizeEdge(points, temptri, tc+1, 0);
                LegalizeEdge(points, temptri, tc+2, 0);
                
            }

            //后处理
            for(int t=0;t<temptri.Count;t++)
            {
                if (!temptri[t].valid) continue;
                if (temptri[t].pa >= N || temptri[t].pb >= N || temptri[t].pc >= N) continue;
                T = temptri[t];
                triangles.Add(new TINtriangle(T.pa, T.pb, T.pc));
            }

            points.RemoveRange(N, 3);
        }

        public void NeighborLinks()
        {
            //对于固定的三角网，建立三角形的邻接关系
            int N = triangles.Count;
            HashSet<int> tp;
            TINtriangle T, NT;
            for(int t=0;t<N;t++)
            {
                T = triangles[t];
                for (int u = 0; u < N; u++)
                {
                    if (u == t) continue;
                    NT = triangles[u];
                    tp = new HashSet<int>();
                    tp.Add(NT.pa);
                    tp.Add(NT.pb);
                    tp.Add(NT.pc);
                    if (tp.Contains(T.pa) && tp.Contains(T.pb)) T.ntab = u;
                    else if (tp.Contains(T.pa) && tp.Contains(T.pc)) T.ntca = u;
                    else if (tp.Contains(T.pc) && tp.Contains(T.pb)) T.ntbc = u;
                }
            }
            return;
        }

        private void LegalizeEdge(List<PointZ> plist,List<TINtriangle> tlist,int tri,int side)
        {
            //边反转，检查tri号三角形的边（0-bc,1-ca,2-ab）
            TINtriangle T1 = tlist[tri],T2;
            int ntri;
            //对齐两三角形位置，使得T1.pb==T2.pb;T1.pc==T2.pc;
            switch (side)
            {
                case 0:
                    ntri = T1.ntbc;
                    break;
                case 1:
                    ntri = T1.ntca;
                    T1.Rotate();
                    break;
                case 2:
                    ntri = T1.ntab;
                    T1.Rotate();
                    T1.Rotate();
                    break;
                default:
                    ntri = -1;
                    break;
            }
            if (ntri == -1) return;
            T2 = tlist[ntri];
            HashSet<int> com = new HashSet<int>();
            com.Add(T1.pb);
            com.Add(T1.pc);
            while(com.Contains(T2.pa)) T2.Rotate();
            if (T2.pb != T1.pb) T2.Swap();

            if (GeomTools.Concave(plist[T1.pb], plist[T1.pc], plist[T1.pa], plist[T2.pa])) return;

            int M = plist.Count - 3;
            bool diaglegal = true;
            if(T1.pb>=M || T1.pc>=M)
            {
                if (T1.pa >= M || T2.pa >= M)
                {
                    diaglegal = Math.Max(T1.pb, T1.pc) < Math.Max(T1.pa, T2.pa);
                }
                else
                {
                    diaglegal = false;
                }
            }
            else if(T1.pa>=M || T2.pa>=M)
            {
                diaglegal = true;
            }
            else if (Illegal(plist[T1.pb], plist[T1.pc], plist[T1.pa], plist[T2.pa]))
            {
                 diaglegal = false;
            }
            if (!diaglegal)
            {
                TINtriangle tab, tac; // 编号tc,tc+1
                int tc = tlist.Count;
                tab = new TINtriangle(T1.pa, T1.pb, T2.pa, T2.ntab, tc+1, T1.ntab);
                tac = new TINtriangle(T1.pa, T1.pc, T2.pa, T2.ntca, tc, T1.ntca);
                if (T1.ntab > 0) tlist[T1.ntab].UpdateNeighbor(tri, tc);
                if (T1.ntca > 0) tlist[T1.ntca].UpdateNeighbor(tri, tc+1);
                if (T2.ntab > 0) tlist[T2.ntab].UpdateNeighbor(ntri, tc);
                if (T2.ntca > 0) tlist[T2.ntca].UpdateNeighbor(ntri, tc + 1);
                T1.valid = false;
                T2.valid = false;
                tlist.Add(tab);
                tlist.Add(tac);

                LegalizeEdge(plist, tlist, tc, 0);
                LegalizeEdge(plist, tlist, tc+1, 0);
            }
        }

        private bool Illegal(PointZ a,PointZ b,PointZ c,PointZ d)
        {
            //对角线ab 是否在四边形acbd中合法
            double angsum = GeomTools.Angle(a, c, b) + GeomTools.Angle(a, d, b);
            return angsum > Math.PI;
        }
    }
}
