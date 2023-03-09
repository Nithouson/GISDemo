using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISDemo
{
    public struct PointD
    {
        public double X, Y;
        public PointD(double X_, double Y_)
        {
            X = X_;
            Y = Y_;
        }
    }

    public struct PointZ
    {
        public double X, Y, Z;

        public PointZ(double X_,double Y_,double Z_= -9999)
        {
            X = X_;
            Y = Y_;
            Z = Z_;
        }
    }

    public struct PointRecord
    {
        public int id;
        public string name;
        public double X,Y,Z;
    }
    

    public partial class frmMain : Form
    {
        List<PointRecord> data;
        GridData grid;
        TINData tin;
        ContourData cont;
        PolygonData poly;
        
        // Xmin: 38429562.9 Xmax: 38433413.45 Xdiff:3850.55
        // Ymin: 3738049.289 Ymax: 3741317.94 Ydiff:3268.65

        double Xwmin = 38428000, Ywmax = 3742000;
        int Zcmin = -500, Zcmax = 0;
        int scale =7;

        //double Xwmin = 0, Ywmax = 700;
        //int scale = 1;

        double Xmin = 40000000, Xmax=0, Ymin = 4000000, Ymax=0;

        bool gridGenerated = false, tinGenerated = false, contourGenerated = false, topoGenerated = false;
        bool displayGrid = false, displayTIN = false, displayContour = false, displaySmoothContour = false,
            displayPolygons = false,displayFilledPolygons=false;
        bool grid_contour_compliance = false;


        int dotsize = 2;

        public Point MapToScreen(double X, double Y)
        {
            double xs = (X - Xwmin) / scale;
            double ys = (Ywmax - Y) / scale;
            return new Point((int)xs, (int)ys);
        }

        public PointD ScreenToMap(Point pscr)
        {
            double xm = Xwmin + pscr.X * scale;
            double ym = Ywmax - pscr.Y * scale;
            return new PointD(xm, ym);
        }


        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            data = new List<PointRecord>();
            StreamReader sr = new StreamReader("data.txt");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] val = line.Split(' ');
                PointRecord p = new PointRecord();
                p.id = int.Parse(val[0]);
                p.name = val[1];
                p.X = double.Parse(val[2]);
                p.Y = double.Parse(val[3]);
                p.Z = double.Parse(val[4]);

                if (p.X > Xmax) Xmax = p.X;
                if (p.X < Xmin) Xmin = p.X;
                if (p.Y > Ymax) Ymax = p.Y;
                if (p.Y < Ymin) Ymin = p.Y;

                data.Add(p);
            }

            pbxMain.Refresh();
        }

        private void pbxMain_Paint(object sender, PaintEventArgs e)
        {
            int npoints = data.Count;
            //tSStatus.Text = npoints.ToString();
            Pen blackpen = new Pen(Color.Black), brownpen = new Pen(Color.Brown), graypen = new Pen(Color.Gray);
            SolidBrush redbrush = new SolidBrush(Color.Red),yellowbrush= new SolidBrush(Color.Yellow),
                blackbrush = new SolidBrush(Color.Black);
            Font drawFont = new Font("Arial", 10);

            Point pscr;
            Rectangle rect;

            if(displayPolygons)
            {
                if (displayFilledPolygons)
                {
                    int npoly = poly.polys.Count;
                    
                    for (int p = 0; p < npoly; p++)
                    {
                        List<PointD> poly_points = poly.PolygonPoints(p);
                        List<Point> closeline = new List<Point>();
                        for (int m = 0; m < poly_points.Count; m++)
                        {
                            closeline.Add(MapToScreen(poly_points[m].X, poly_points[m].Y));
                        }
                        if (p != poly.selp)
                        {
                            Random ra = new Random(p);
                            int r = ra.Next(0, 256), g = ra.Next(0, 256), b = ra.Next(0, 256);
                            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(120, r, g, b)), closeline.ToArray());
                        }
                        else
                        {
                            e.Graphics.FillPolygon(yellowbrush, closeline.ToArray());
                        }
                    }
                }

                int narcs = poly.arcs.Count;
                List<PointD> arc;
                for (int l = 1; l < narcs; l++)
                {
                    arc = poly.ArcPoints(l);
                    List<Point> line = new List<Point>();
                    for (int m = 0; m < arc.Count; m++)
                    {
                        line.Add(MapToScreen(arc[m].X,arc[m].Y));
                    }
                    e.Graphics.DrawLines(blackpen, line.ToArray());

                    pscr = MapToScreen(arc[0].X, arc[0].Y);
                    rect = new Rectangle(pscr.X - dotsize, pscr.Y - dotsize, 2 * dotsize, 2 * dotsize);
                    e.Graphics.FillRectangle(redbrush, rect);

                    pscr = MapToScreen(arc[arc.Count-1].X, arc[arc.Count - 1].Y);
                    rect = new Rectangle(pscr.X - dotsize, pscr.Y - dotsize, 2 * dotsize, 2 * dotsize);
                    e.Graphics.FillRectangle(redbrush, rect);
                }
            }

            if (displayGrid)
            {
                for (int r = 0; r < grid.rows; r++)
                {
                    e.Graphics.DrawLine(graypen, MapToScreen(grid.Xmin, grid.GetY(r)),
                        MapToScreen(grid.GetX(grid.cols - 1), grid.GetY(r)));
                }
                for (int c = 0; c < grid.cols; c++)
                {
                    e.Graphics.DrawLine(graypen, MapToScreen(grid.GetX(c), grid.Ymax),
                        MapToScreen(grid.GetX(c), grid.GetY(grid.rows - 1)));
                }
                if (grid.selr != -1 && grid.selc != -1)
                {
                    pscr = MapToScreen(grid.GetX(grid.selc), grid.GetY(grid.selr));
                    rect = new Rectangle(pscr.X - dotsize, pscr.Y - dotsize, 2 * dotsize, 2 * dotsize);
                    e.Graphics.FillRectangle(yellowbrush, rect);
                }
            }

            if (displayTIN)
            {
                int ntri = tin.triangles.Count, a, b, c;
                for (int t = 0; t < ntri; t++)
                {
                    a = tin.triangles[t].pa;
                    b = tin.triangles[t].pb;
                    c = tin.triangles[t].pc;
                    e.Graphics.DrawLine(graypen, MapToScreen(data[a].X, data[a].Y),
                        MapToScreen(data[b].X, data[b].Y));
                    e.Graphics.DrawLine(graypen, MapToScreen(data[a].X, data[a].Y),
                        MapToScreen(data[c].X, data[c].Y));
                    e.Graphics.DrawLine(graypen, MapToScreen(data[c].X, data[c].Y),
                        MapToScreen(data[b].X, data[b].Y));
                }
            }

            if (displayContour)
            {
                if (displaySmoothContour)
                {
                    int nlines = cont.contour.Count, pnum, labelX, labelY;
                    for (int l = 0; l < nlines; l++)
                    {
                        pnum = cont.contour[l].Count;
                        List<Point> line = new List<Point>();
                        for (int m = 0; m < pnum; m++)
                        {
                            line.Add(MapToScreen(cont.contour[l][m].X, cont.contour[l][m].Y));
                        }
                        e.Graphics.DrawCurve(brownpen, line.ToArray(), 0.5F);

                        labelX = MapToScreen(cont.contour[l][0].X, cont.contour[l][0].Y).X;
                        labelY = MapToScreen(cont.contour[l][0].X, cont.contour[l][0].Y).Y;
                        e.Graphics.DrawString(cont.hlist[l].ToString(), drawFont, redbrush,
                            labelX, labelY);
                    }
                }
                else
                {
                    int nlines = cont.contour.Count, pnum, labelX, labelY;
                    for (int l = 0; l < nlines; l++)
                    {
                        pnum = cont.contour[l].Count;
                        List<Point> line = new List<Point>();
                        for (int m = 0; m < pnum; m++)
                        {
                            line.Add(MapToScreen(cont.contour[l][m].X, cont.contour[l][m].Y));
                        }
                        e.Graphics.DrawLines(brownpen, line.ToArray());

                        labelX = MapToScreen(cont.contour[l][0].X, cont.contour[l][0].Y).X;
                        labelY = MapToScreen(cont.contour[l][0].X, cont.contour[l][0].Y).Y;
                        e.Graphics.DrawString(cont.hlist[l].ToString(), drawFont, redbrush,
                            labelX, labelY);
                    }

                }
            }

            for (int i = 0; i < npoints; i++)
            {
                pscr = MapToScreen(data[i].X, data[i].Y);
                rect = new Rectangle(pscr.X - dotsize, pscr.Y - dotsize, 2 * dotsize, 2 * dotsize);
                e.Graphics.FillRectangle(blackbrush, rect);
            }


        }


        private void pbxMain_MouseClick(object sender, MouseEventArgs e)
        {
            if(displayGrid)
            {
                PointD pmap = ScreenToMap(e.Location);
                double gridr = (Ymax - pmap.Y) / grid.res;
                double gridc = (pmap.X- Xmin) / grid.res;
                int igridr = (int)Math.Round(gridr);
                int igridc = (int)Math.Round(gridc);
                if(Math.Abs(gridr-igridr)<0.2 && Math.Abs(gridc - igridc) < 0.2 &&
                    igridr>=0 && igridr<grid.rows && igridc >= 0 && igridc < grid.cols)
                {
                    grid.selr = igridr;
                    grid.selc = igridc;
                    pbxMain.Refresh();
                    MessageBox.Show(" X:" + (grid.Xmin + grid.res * igridc).ToString() + "\n " +
                        "Y:" + (grid.Ymax - grid.res * igridc).ToString() + "\n " +
                        "Z:" + (grid.val[igridr][igridc]).ToString() + "\n ", "格网点查询");
                }
                else
                {
                    grid.selr = -1;
                    grid.selc = -1;
                }
            }
            if(displayPolygons)
            {
                poly.selp = -1;
                PointD pmap = ScreenToMap(e.Location);
                for(int i=0;i<poly.polys.Count;i++)
                {
                    if(GeomTools.Point_in_Polygon(pmap,poly.PolygonPoints(i).ToArray()))
                    {
                        poly.selp = i;
                        pbxMain.Refresh();
                        MessageBox.Show(" 多边形编号:" + i.ToString() + "\n " +
                        "周长:" + poly.Polygon_Perimeter(i).ToString() + "m\n " +
                        "面积:" + poly.Polygon_Area(i).ToString() + "m²\n ", "多边形查询");
                    }
                } 
            }
            
        }


        private void pbxMain_MouseMove(object sender, MouseEventArgs e)
        {
            tSPos.Text = ScreenToMap(e.Location).X.ToString() + ',' + ScreenToMap(e.Location).Y.ToString();
        }


        private void mSIntGen_Click(object sender, EventArgs e)
        {
            frmInter sfrmInter = new frmInter();
            if (sfrmInter.ShowDialog(this) != DialogResult.OK) return;

            double resol = sfrmInter.res;
            int nx = (int)Math.Ceiling((Xmax - Xmin) / resol)+1;
            int ny = (int)Math.Ceiling((Ymax - Ymin) / resol)+1;

            grid = new GridData(ny, nx, Xmin, Ymax, resol);
            if (sfrmInter.UseIDW) 
                grid.Interpolate_IDW(data);
            else grid.Interpolate_Direc(data);

            gridGenerated = true;
            displayGrid = true;
            mSIntShow.Checked = true;
            pbxMain.Refresh();
            grid_contour_compliance = false;
        }

        

        private void mSIntDens_Click(object sender, EventArgs e)
        {
            if (!gridGenerated) return;
            frmGridDense sfrmGridDense = new frmGridDense();
            if (sfrmGridDense.ShowDialog(this) != DialogResult.OK) return;

            grid = grid.Dense(sfrmGridDense.cut);

            displayGrid = true;
            mSIntShow.Checked = true;
            pbxMain.Refresh();

        }


        private void mSIntShow_Click(object sender, EventArgs e)
        {
            if (gridGenerated && displayGrid == false)
            {
                displayGrid = true;
                mSIntShow.Checked = true;
            }
            else
            {
                displayGrid = false; 
                mSIntShow.Checked = false;
            }
            pbxMain.Refresh();
        }

        private void mSIntClear_Click(object sender, EventArgs e)
        {
            gridGenerated = false;
            displayGrid = false;
            mSIntShow.Checked = false;
            pbxMain.Refresh();
        }

        private void mSTINGen_Click(object sender, EventArgs e)
        {
            tin = new TINData(data,(Xmin+Xmax)/2,(Ymin+Ymax)/2);
            tin.Delaunay();
            
            tinGenerated = true;
            displayTIN = true; 
            mSTINShow.Checked = true;
            pbxMain.Refresh();
        }

        private void mSTINShow_Click(object sender, EventArgs e)
        {
            if (tinGenerated && displayTIN == false)
            {
                displayTIN = true;
                mSTINShow.Checked = true;
            }
            else
            {
                displayTIN = false;
                mSTINShow.Checked = false;
            }
            pbxMain.Refresh();
        }

        private void mSConGen_Click(object sender, EventArgs e)
        {
            frmCont sfrmCont = new frmCont();
            if (sfrmCont.ShowDialog(this) != DialogResult.OK) return;
            int itv = sfrmCont.interval;

            if (sfrmCont.UseGrid)
            {
                if (!gridGenerated)
                {
                    MessageBox.Show("请先生成格网！", "错误");
                    return;
                }
                cont = new ContourData();
                cont.FromGridData(grid, Zcmin, itv, Zcmax);
                grid_contour_compliance = true;
            }
            else
            {
                if (!tinGenerated)
                {
                    MessageBox.Show("请先生成TIN！", "错误");
                    return;
                }
                cont = new ContourData();
                cont.FromTINData(tin, Zcmin, itv, Zcmax);
                grid_contour_compliance = false;
            }

                contourGenerated = true;
            displayContour = true;
            mSConShow.Checked = true;
            pbxMain.Refresh();

        }

        private void mSConShow_Click(object sender, EventArgs e)
        {
            if (contourGenerated && displayContour == false)
            {
                displayContour = true;
                mSConShow.Checked = true;
            }
            else
            {
                displayContour = false;
                mSConShow.Checked = false;
            }
            pbxMain.Refresh();
        }

        private void mSConClear_Click(object sender, EventArgs e)
        {
            contourGenerated = false;
            displayContour = false;
            mSConShow.Checked = false;
            displaySmoothContour = false;
            mSConSmooth.Checked = false;
            pbxMain.Refresh();
        }

        private void mSConSmooth_Click(object sender, EventArgs e)
        {
            if (contourGenerated && displaySmoothContour == false)
            {
                displaySmoothContour = true;
                mSConSmooth.Checked = true;
            }
            else
            {
                displaySmoothContour = false;
                mSConSmooth.Checked = false;
            }
            pbxMain.Refresh();
        }

        private void mSTopoGen_Click(object sender, EventArgs e)
        {
            if ((!gridGenerated) || (!contourGenerated) )
            {
                MessageBox.Show("请先生成格网及等值线！", "错误");
                return;
            }
            if (!grid_contour_compliance)
            {
                MessageBox.Show("格网与等值线范围不一致！", "错误");
                return;
            }
            poly = new PolygonData();
            poly.FromGridContour(cont.contour, grid.Xmin, grid.Xmax, grid.Ymin, grid.Ymax);

            topoGenerated = true;
            displayPolygons = true;
            mSTopoShow.Checked = true;
            pbxMain.Refresh();

        }

        private void mSTopoShow_Click(object sender, EventArgs e)
        {
            if (topoGenerated && displayPolygons == false)
            {
                displayPolygons = true;
                mSTopoShow.Checked = true;
            }
            else
            {
                displayPolygons = false;
                mSTopoShow.Checked = false;
            }
            pbxMain.Refresh();
        }

        private void mSTopoClear_Click(object sender, EventArgs e)
        {
            topoGenerated = false;
            displayPolygons = false;
            mSTopoShow.Checked = false;
            displayFilledPolygons = false;
            mSFillPoly.Checked = false;
            pbxMain.Refresh();
        }

        private void mSTopoTab_Click(object sender, EventArgs e)
        {
            if (!topoGenerated) return;
            sfdTopo.Filter = "Text File(*.txt)|*.txt|All files(*.*)|*.*";
            if (sfdTopo.ShowDialog() == DialogResult.OK)
            {
                string filePath = sfdTopo.FileName;

                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                TextWriter tw = new StreamWriter(fs, Encoding.Default);
                tw.Write("弧段|起始结点|终止结点|左多边形|右多边形\n");
                for (int aid=1;aid<poly.arcs.Count;aid++)
                {
                     tw.Write(aid.ToString()+" "+poly.Arc_Start(aid).ToString()+" "+
                         poly.Arc_End(aid).ToString() + " "+poly.arcs[aid].lpoly.ToString()+" "+
                         poly.arcs[aid].rpoly.ToString()+"\n");
                }
                tw.Flush();
                tw.Write("多边形|弧段表|相邻多边形\n");
                for (int pid = 0; pid < poly.polys.Count; pid++)
                {
                    tw.Write(pid.ToString() + " " + poly.polys[pid].Arcs_String() + " " 
                        + poly.polys[pid].Npolys_String() +"\n");
                }
                tw.Flush();
                tw.Close();
                MessageBox.Show("拓扑关系表导出完成！", "提示");
            }
        }

        private void mSFillPoly_Click(object sender, EventArgs e)
        {
            if (topoGenerated && displayFilledPolygons == false)
            {
                displayFilledPolygons = true;
                mSFillPoly.Checked = true;
            }
            else
            {
                displayFilledPolygons = false;
                mSFillPoly.Checked = false;
            }
            pbxMain.Refresh();
        }

        private void mSAuthor_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" 作者：郭浩\n 单位：北京大学遥感与地理信息系统研究所\n " +
                "联系方式：sinesloop@pku.edu.cn\n","作者信息", MessageBoxButtons.OK) ;
        }
    }
}
