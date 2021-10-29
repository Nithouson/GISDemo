using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISDemo
{
    public class ContourData
    {
        public List<int> hlist;
        public List<List<PointD>> contour;

        public ContourData()
        {
            hlist = new List<int>();
            contour = new List<List<PointD>>();
        }

        public void FromGridData(GridData grid,int hmin,int hitv,int hmax)
        {
            int M = grid.rows, N = grid.cols;
            double vl, vr, vu, vd;
            for(int h=hmin;h<hmax;h+=hitv)
            {
                //内插等值点
                double[,] hseg = new double[M, N - 1];
                double[,] vseg = new double[M - 1, N];
                for(int r = 0; r < M; r++)
                {
                    for(int ci = 0;ci<N-1;ci++)
                    {
                        vl = grid.val[r][ci];
                        vr = grid.val[r][ci + 1];
                        if ((h - vl) * (h - vr) < 0)
                        {
                            hseg[r, ci] = (h - vl) / (vr - vl);
                        }
                        else hseg[r,ci] = -1;
                    }
                }
                for (int ri = 0; ri < M-1; ri++)
                {
                    for (int c = 0; c < N ; c++)
                    {
                        vu = grid.val[ri][c];
                        vd = grid.val[ri+1][c];
                        if ((h - vu) * (h - vd) < 0)
                        {
                            vseg[ri, c] = (h - vu) / (vd - vu);
                        }
                        else vseg[ri, c] = -1;
                    }
                }

                //追踪开线头 顺序：下、左、上、右
                for(int ci=0;ci<N-1;ci++)
                {
                    if(hseg[M-1,ci]>=0)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(grid.GetX(ci + hseg[M-1, ci]), grid.GetY(M - 1)));
                        hseg[M-1, ci] = -1;
                        TraceGrid(grid, cline, hseg, vseg,M-2,ci,0);
                        contour.Add(cline);
                        hlist.Add(h);
                     }
                }

                for (int ri = 0; ri < M - 1; ri++)
                {
                    if (vseg[ri, 0] >= 0)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(grid.GetX(0), grid.GetY(ri+vseg[ri,0])));
                        vseg[ri, 0] = -1;
                        TraceGrid(grid, cline, hseg, vseg, ri, 0, 1);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                }

                for (int ci = 0; ci < N - 1; ci++)
                {
                    if (hseg[0, ci] >= 0)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(grid.GetX(ci + hseg[0, ci]), grid.GetY(0)));
                        hseg[0, ci] = -1;
                        TraceGrid(grid, cline, hseg, vseg, 0, ci, 2);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                }

                for (int ri = 0; ri < M - 1; ri++)
                {
                    if (vseg[ri, N-1] >= 0)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(grid.GetX(N-1), grid.GetY(ri + vseg[ri, N-1])));
                        vseg[ri, N - 1] = -1;
                        TraceGrid(grid, cline, hseg, vseg, ri, N-2, 3);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                }

                //追踪闭线头
                for (int r = 1; r < M - 1; r++)
                {
                    for (int ci = 0; ci < N - 1; ci++)
                    {
                        if(hseg[r, ci]>0)
                        {
                            List<PointD> cline = new List<PointD>();
                            cline.Add(new PointD(grid.GetX(ci + hseg[r, ci]), grid.GetY(r)));
                            TraceGrid(grid, cline, hseg, vseg, r, ci, 2);
                            contour.Add(cline);
                            hlist.Add(h);
                        }
                    }
                }
                for (int ri = 0; ri < M - 1; ri++)
                {
                    for (int c = 1; c < N-1; c++)
                    {
                        if (vseg[ri, c] > 0)
                        {
                            List<PointD> cline = new List<PointD>();
                            cline.Add(new PointD(grid.GetX(c), grid.GetY(ri+vseg[ri,c])));
                            TraceGrid(grid, cline, hseg, vseg, ri,c, 1);
                            contour.Add(cline); 
                            hlist.Add(h);
                        }
                    }
                }
            }
        }

        public void FromTINData(TINData tin, int hmin,int hitv, int hmax)
        {
            int N= tin.triangles.Count;
            tin.NeighborLinks();
            TINtriangle T;
            for (int h = hmin; h < hmax; h += hitv)
            {
                //内插等值点
                for(int t=0;t<N;t++)
                {
                    T = tin.triangles[t];
                    PointZ pta = tin.points[T.pa], ptb = tin.points[T.pb], ptc = tin.points[T.pc];
                    if ((pta.Z-h)*(ptb.Z-h)<0)
                    {
                        T.segab = (h - pta.Z) / (ptb.Z - pta.Z);
                    }
                    if ((ptb.Z - h) * (ptc.Z - h) < 0)
                    {
                        T.segbc = (h - ptb.Z) / (ptc.Z - ptb.Z);
                    }
                    if ((ptc.Z - h) * (pta.Z - h) < 0)
                    {
                        T.segca = (h - ptc.Z) / (pta.Z - ptc.Z);
                    }
                }

                //追踪开线头 
                for (int t = 0; t < N; t++)
                {
                    T = tin.triangles[t];
                    PointZ pta = tin.points[T.pa], ptb = tin.points[T.pb], ptc = tin.points[T.pc];
                    if(T.segbc>=0 && T.ntbc==-1)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(tin.BiasX+Linear_Interpolate(ptb.X,ptc.X,T.segbc), 
                            tin.BiasY+Linear_Interpolate(ptb.Y, ptc.Y, T.segbc)));
                        T.segbc = -1;
                        TraceTIN(tin, cline, t, 0);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                    if (T.segca >= 0 && T.ntca == -1)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(tin.BiasX+Linear_Interpolate(ptc.X, pta.X, T.segca),
                            tin.BiasY + Linear_Interpolate(ptc.Y, pta.Y, T.segca)));
                        T.segca = -1;
                        TraceTIN(tin, cline, t, 1);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                    if (T.segab >= 0 && T.ntab == -1)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(tin.BiasX + Linear_Interpolate(pta.X, ptb.X, T.segab),
                            tin.BiasY + Linear_Interpolate(pta.Y, ptb.Y, T.segab)));
                        T.segab = -1;
                        TraceTIN(tin, cline, t, 2);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                }

                //追踪闭线头
                for (int t = 0; t < N; t++)
                {
                    T = tin.triangles[t];
                    PointZ pta = tin.points[T.pa], ptb = tin.points[T.pb], ptc = tin.points[T.pc];
                    if (T.segbc >= 0)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(tin.BiasX + Linear_Interpolate(ptb.X, ptc.X, T.segbc),
                            tin.BiasY + Linear_Interpolate(ptb.Y, ptc.Y, T.segbc)));
                        TraceTIN(tin, cline, t, 0);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                    if (T.segca >= 0)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(tin.BiasX + Linear_Interpolate(ptc.X, pta.X, T.segca),
                            tin.BiasY + Linear_Interpolate(ptc.Y, pta.Y, T.segca)));
                        TraceTIN(tin, cline, t, 1);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                    if (T.segab >= 0)
                    {
                        List<PointD> cline = new List<PointD>();
                        cline.Add(new PointD(tin.BiasX + Linear_Interpolate(pta.X, ptb.X, T.segab),
                            tin.BiasY + Linear_Interpolate(pta.Y, ptb.Y, T.segab)));
                        TraceTIN(tin, cline, t, 2);
                        contour.Add(cline);
                        hlist.Add(h);
                    }
                }
            }
        }

        private void TraceGrid(GridData grid,List<PointD> cline,double[,] hseg,double[,] vseg,
            int boxU,int boxL,int direc)
        {
            //direc: 0-Up;1-Right;2-Down;3-Left
            int M = grid.rows, N = grid.cols;
            int Useg=0, Dseg=0, Lseg=0, Rseg=0;
            int ndirec = -1 ;
            if (hseg[boxU, boxL] >= 0) Useg = 1;
            if (hseg[boxU+1, boxL] >= 0) Dseg = 1;
            if (vseg[boxU, boxL] >= 0) Lseg = 1;
            if (vseg[boxU, boxL+1] >= 0) Rseg = 1;
            switch (direc)
            {
                case 0:
                    if (Useg+Lseg+Rseg == 1)
                    {
                        if (Useg == 1) ndirec = 0;
                        else if (Lseg == 1) ndirec = 3;
                        else if (Rseg == 1) ndirec = 1;
                    }
                    else if(Useg + Lseg + Rseg == 3)
                    {
                        if (vseg[boxU, boxL] > vseg[boxU, boxL + 1]) ndirec = 3;
                        else ndirec = 1;
                    }
                    break;
                case 1:
                    if (Useg + Dseg + Rseg == 1)
                    {
                        if (Useg == 1) ndirec = 0;
                        else if (Dseg == 1) ndirec = 2;
                        else if (Rseg == 1) ndirec = 1;
                    }
                    else if (Useg + Dseg + Rseg == 3)
                    {
                        if (hseg[boxU, boxL] < hseg[boxU+1, boxL]) ndirec = 0;
                        else ndirec = 2;
                    }
                    break;
                case 2:
                    if (Dseg + Lseg + Rseg == 1)
                    {
                        if (Dseg == 1) ndirec = 2;
                        else if (Lseg == 1) ndirec = 3;
                        else if (Rseg == 1) ndirec = 1;
                    }
                    else if (Dseg + Lseg + Rseg == 3)
                    {
                        if (vseg[boxU, boxL] < vseg[boxU, boxL + 1]) ndirec = 3;
                        else ndirec = 1;
                    }
                    break;
                case 3:
                    if (Useg + Dseg + Lseg == 1)
                    {
                        if (Useg == 1) ndirec = 0;
                        else if (Dseg == 1) ndirec = 2;
                        else if (Lseg == 1) ndirec = 3;
                    }
                    else if (Useg + Dseg + Lseg == 3)
                    {
                        if (hseg[boxU, boxL] > hseg[boxU + 1, boxL]) ndirec = 0;
                        else ndirec = 2;
                    }
                    break;
                default:
                    
                    break;
            }
            switch(ndirec)
            {
                case 0:
                    cline.Add(new PointD(grid.GetX(boxL + hseg[boxU, boxL]), grid.GetY(boxU)));
                    hseg[boxU, boxL] = -1;
                    if (boxU > 0)
                    {
                        TraceGrid(grid, cline, hseg, vseg, boxU - 1, boxL, 0);
                    }
                    break;
                case 1:
                    cline.Add(new PointD(grid.GetX(boxL + 1), grid.GetY(boxU + vseg[boxU, boxL + 1])));
                    vseg[boxU, boxL + 1] = -1;
                    if (boxL < N - 2)
                    {
                        TraceGrid(grid, cline, hseg, vseg, boxU, boxL + 1, 1);
                    }
                    break;
                case 2:
                    cline.Add(new PointD(grid.GetX(boxL + hseg[boxU + 1, boxL]), grid.GetY(boxU + 1)));
                    hseg[boxU + 1, boxL] = -1;
                    if (boxU < M - 2)
                    {
                        TraceGrid(grid, cline, hseg, vseg, boxU + 1, boxL, 2);
                    }
                    break;
                case 3:
                    cline.Add(new PointD(grid.GetX(boxL), grid.GetY(boxU + vseg[boxU, boxL])));
                    vseg[boxU, boxL] = -1;
                    if (boxL > 0)
                    {
                        TraceGrid(grid, cline, hseg, vseg, boxU, boxL - 1, 3);
                    }
                    break;
                default:
                    break;
            }
            return;
        }

        private void TraceTIN(TINData tin, List<PointD> cline, int tc, int inedge)
        {
            //inedge 0-bc;1-ca;2-ab 
            int outedge = -1;
            TINtriangle T = tin.triangles[tc],NT;
            PointZ pta = tin.points[T.pa], ptb = tin.points[T.pb], ptc = tin.points[T.pc];
            HashSet<int> com;

            switch (inedge)
            {
                case 0:
                    if (T.segca >= 0) outedge = 1;
                    else if (T.segab >= 0) outedge = 2;
                    break;
                case 1:
                    if (T.segbc >= 0) outedge = 0;
                    else if (T.segab >= 0) outedge = 2;
                    break;
                case 2:
                    if (T.segbc >= 0) outedge = 0;
                    else if (T.segca >= 0) outedge = 1;
                    break;
                default:
                    break;
            }
            switch (outedge)
            {
                case 0:
                    cline.Add(new PointD(tin.BiasX + Linear_Interpolate(ptb.X, ptc.X, T.segbc),
                            tin.BiasY + Linear_Interpolate(ptb.Y, ptc.Y, T.segbc)));
                    T.segbc = -1;
                    if (T.ntbc > 0)
                    {
                        NT = tin.triangles[T.ntbc];
                        com = new HashSet<int>();
                        com.Add(T.pb);
                        com.Add(T.pc);
                        while (com.Contains(NT.pa)) NT.Rotate();
                        NT.segbc = -1;
                        TraceTIN(tin, cline, T.ntbc, 0);
                    }
                    break;
                case 1:
                    cline.Add(new PointD(tin.BiasX + Linear_Interpolate(ptc.X, pta.X, T.segca),
                             tin.BiasY+ Linear_Interpolate(ptc.Y, pta.Y, T.segca)));
                    T.segca = -1;
                    if (T.ntca > 0)
                    {
                        NT = tin.triangles[T.ntca];
                        com = new HashSet<int>();
                        com.Add(T.pc);
                        com.Add(T.pa);
                        while (com.Contains(NT.pb)) NT.Rotate();
                        NT.segca = -1;
                        TraceTIN(tin, cline, T.ntca, 1);
                    }
                    break;
                case 2:
                    cline.Add(new PointD(tin.BiasX + Linear_Interpolate(pta.X, ptb.X, T.segab),
                            tin.BiasY + Linear_Interpolate(pta.Y, ptb.Y, T.segab)));
                    T.segab = -1;
                    if (T.ntab > 0)
                    {
                        NT = tin.triangles[T.ntab];
                        com = new HashSet<int>();
                        com.Add(T.pa);
                        com.Add(T.pb);
                        while (com.Contains(NT.pc)) NT.Rotate();
                        NT.segab = -1;
                        TraceTIN(tin, cline, T.ntab, 2);
                    }
                    break;
                default:
                    break;
            }
        }

        private double Linear_Interpolate(double s,double e,double ratio)
        {
            return s + ratio * (e - s);
        }
    }
}
