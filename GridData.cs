using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISDemo
{
    public struct Pair
    {
        public int id;
        public double val;
        public Pair(int id_,double val_)
        {
            id = id_;
            val = val_;
        }
        
    }

    public class GridData
    {
        public double Xmin, Ymax, res;
        public int rows, cols;
        public int selr=-1, selc=-1;
        public List<List<double>> val;
        public GridData(int rows_, int cols_, double Xmin_ = -1, double Ymax_ = -1, double res_ = -1)
        {
            rows = rows_;
            cols = cols_;

            val = new List<List<double>>();
            for (int i = 0; i < rows; i++)
            {
                val.Add(new List<double>());
                for (int j = 0; j < cols; j++)
                {
                    val[i].Add(0);
                }
            }
            Xmin = Xmin_;
            Ymax = Ymax_;
            res = res_;
        }

        public double Xmax
        {
            get { return Xmin + (cols-1)*res; }
        }

        public double Ymin
        {
            get { return Ymax - (rows - 1) * res; }
        }

        public double GetX(double r)
        {
            return Xmin + r * res;
        }

        public double GetY(double c)
        {
            return Ymax - c * res;
        }

       

        public void Interpolate_IDW(List<PointRecord> points)
        {
            int nbs = 10,npoints = points.Count;
            double gridX, gridY, vsum, wsum ;
            for(int r=0;r<rows;r++)
            {
                gridY = GetY(r);
                for(int c=0;c<cols;c++)
                {
                    gridX = GetX(c);
                    List<Pair> dists = new List<Pair>();
                    for (int i = 0; i < npoints; i++)
                    {
                        dists.Add(new Pair(i, GeomTools.Dist2D(gridX, gridY, points[i].X, points[i].Y)));
                    }
                    dists.Sort((x, y) => { return x.val.CompareTo(y.val); });

                    wsum = 0;
                    vsum = 0;
                    for (int j=0;j<nbs;j++)
                    {
                        wsum += 100 / (dists[j].val * dists[j].val);
                        vsum += 100 / (dists[j].val * dists[j].val) * points[dists[j].id].Z;
                    }
                    val[r][c] = vsum / wsum;
                }
                
            }
        }

        public void Interpolate_Direc(List<PointRecord> points)
        {
            int npoints = points.Count,zone;
            double gridX, gridY, vsum, wsum,curX,curY;
            for (int r = 0; r < rows; r++)
            {
                gridY = GetY(r);
                for (int c = 0; c < cols; c++)
                {
                    gridX = GetX(c);
                    //8个45°扇形，从第一象限，逆时针编号0-7
                    int[] nnid= new int[8];
                    double[] nndist = new double[8];
                    for(int d=0;d<8;d++)
                    {
                        nnid[d] = -1;
                        nndist[d] = 1e10;
                    }
                    for (int i = 0; i < npoints; i++)
                    {
                        curX = points[i].X;
                        curY = points[i].Y;
                        if(curX>= gridX)
                        {
                            if(curY>= gridY)
                            {
                                if (Math.Abs(curX - gridX) >= Math.Abs(curY - gridY)) zone = 0;
                                else zone = 1;
                            }
                            else
                            {
                                if (Math.Abs(curX - gridX) >= Math.Abs(curY - gridY)) zone = 3;
                                else zone = 2;
                            }
                        }
                        else
                        {
                            if (curY >= gridY)
                            {
                                if (Math.Abs(curX - gridX) >= Math.Abs(curY - gridY)) zone = 7;
                                else zone = 6;
                            }
                            else
                            {
                                if (Math.Abs(curX - gridX) >= Math.Abs(curY - gridY)) zone = 4;
                                else zone = 5;
                            }
                        }
                        if(GeomTools.Dist2D(curX,curY,gridX,gridY)<nndist[zone])
                        {
                            nndist[zone] = GeomTools.Dist2D(curX, curY, gridX, gridY);
                            nnid[zone] = i;
                        }
                    }
                    

                    wsum = 0;
                    vsum = 0;
                    for (int d = 0; d <8; d++)
                    {
                        if (nnid[d] == -1) continue;
                        wsum += 100 / (nndist[d]* nndist[d]);
                        vsum += 100 / (nndist[d] * nndist[d]) * points[nnid[d]].Z;
                    }
                    val[r][c] = vsum / wsum;
                }

            }
        }

        public GridData Dense(int cut)
        {
            GridData newgrid = new GridData((rows - 1) * cut + 1, (cols - 1) * cut + 1, Xmin, Ymax, res / cut);
            for(int r=0;r<newgrid.rows;r++)
            {
                for(int c=0;c<newgrid.cols;c++)
                {
                    double retr = 1.0 * (r % cut) / cut;
                    double retc = 1.0 * (c % cut) / cut;
                    if (r%cut==0)
                    {
                        if(c%cut==0)
                        {
                            newgrid.val[r][c] = val[r / cut][c / cut];
                        }
                        else
                        {
                            newgrid.val[r][c] = (1 - retc) * val[r / cut][c / cut] +
                                retc * val[r / cut][c / cut + 1];
                        }
                    }
                    else
                    {
                        if (c % cut == 0)
                        {
                            newgrid.val[r][c] = (1 - retr) * val[r / cut][c / cut] +
                                retr * val[r / cut+1][c / cut];
                        }
                        else
                        {
                            newgrid.val[r][c] = (1 - retc)*((1 - retr) * val[r / cut][c / cut] +
                                retr * val[r / cut + 1][c / cut])+retc*((1 - retr) * val[r / cut][c / cut+1] +
                                retr * val[r / cut + 1][c / cut+1]);
                        }
                    }
                }
            }
            return newgrid;

        }
    }
}
