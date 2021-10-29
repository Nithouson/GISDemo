using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISDemo
{
    public static class GeomTools
    {
        public static double Dist2D(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static double Area(PointZ a, PointZ b, PointZ c)
        {
            double x1 = b.X - a.X;
            double y1 = b.Y - a.Y;
            double x2 = c.X - a.X;
            double y2 = c.Y - a.Y;
            return 0.5 * Math.Abs(x1 * y2 - x2 * y1);
        }

        public static bool Point_in_Triangle(PointZ p, PointZ a, PointZ b, PointZ c, double eps = 1e-6)
        {
            // 点在三角形内或在边界上
            return Math.Abs(Area(p, a, b) + Area(p, a, c) + Area(p, b, c) - Area(a, b, c)) < eps;
        }

        public static double Angle(PointZ a,PointZ c,PointZ b)
        {
            double sa = Dist2D(b.X, b.Y, c.X, c.Y);
            double sb = Dist2D(a.X, a.Y, c.X, c.Y);
            double sc = Dist2D(a.X, a.Y, b.X, b.Y);
            double ang = Math.Acos((sa * sa + sb * sb - sc * sc) / (2 * sa * sb));
            return ang;
        }

        public static bool Concave(PointZ a,PointZ b,PointZ c,PointZ d)
        {
            //acbd是凹四边形 ab是内对角线
            PointZ p = new PointZ((c.X + d.X) / 2, (c.Y + d.Y) / 2);
            return !(Point_in_Triangle(p,a,b,c) || Point_in_Triangle(p, a, b, d));
        }

        public static bool Identical(PointD a,PointD b)
        {
            double eps = 1e-7;
            return Math.Abs(a.X - b.X) < eps && Math.Abs(a.Y - b.Y) < eps;
        }

        public static double det2(double a,double b,double c,double d)
        {
            return a * d - b * c;
        }

        public static double Direction_Angle(PointD c,PointD w)
        {
            //以c为中心点，w的方向角（0-2*pi）
            double dx = w.X - c.X, dy = w.Y - c.Y;
            double atan = Math.Atan2(dy, dx);
            if (dy < 0) return atan + 2 * Math.PI;
            else return atan;
        }

        public static PointD Intersect(PointD s1,PointD t1,PointD s2,PointD t2)
        {
            double u, v, x, y;
            double eps = 1e-7;
            double d = det2(t1.X - s1.X, s2.X - t2.X, t1.Y - s1.Y, s2.Y - t2.Y);
            if (Math.Abs(d) < eps) return new PointD(-9999,-9999);
            u = det2(s2.X - s1.X, s2.X - t2.X, s2.Y - s1.Y, s2.Y - t2.Y)/d;
            v = det2(t1.X - s1.X, s2.X - s1.X, t1.Y - s1.Y, s2.Y - s1.Y)/d;
            if (u>-eps && u<1+eps && v>-eps && v<1+eps)
            {
                x = s1.X + u * (t1.X - s1.X);
                y = s1.Y + u * (t1.Y - s1.Y);
                return new PointD(x, y);
            }
            else return new PointD(-9999, -9999);
        }

        public static double PolygonArea(List<PointD> p)
        {
            if(!Identical(p[0],p[p.Count-1]))
            {
                p.Add(p[0]);
            }
            double area = 0;
            for(int i=0;i<p.Count-1;i++)
            {
                area += 0.5 * (p[i].Y + p[i + 1].Y) * (p[i + 1].X - p[i].X);
            }
            return Math.Abs(area);
        }


        public static bool RayIntersectsSegment(PointD aPoint, PointD startPoint, PointD endPoint)
        {
            if (startPoint.Y == endPoint.Y)      //排除重合、平行、线段首尾点重合
                return false;
            if (startPoint.Y > aPoint.Y && endPoint.Y > aPoint.Y)      //线段在射线上方
                return false;
            if (startPoint.Y < aPoint.Y && endPoint.Y < aPoint.Y)      //线段在射线下方
                return false;
            if (aPoint.Y == Math.Min(startPoint.Y, endPoint.Y))          //射线过线段的下端点
                return false;
            if (aPoint.X > startPoint.X && aPoint.X > endPoint.X)      //线段在射线左方
                return false;
            double sX = startPoint.X + (aPoint.Y - startPoint.Y) * (endPoint.X - startPoint.X)
                / (endPoint.Y - startPoint.Y);          //求交
            if (sX < aPoint.X)                          //交点在射线左侧
                return false;
            return true;
        }

        public static bool Point_in_Polygon(PointD aPoint, PointD[] polygon)
        {

            //累计射线交点
            int sPointCount = polygon.Length;  //顶点的数目
            int sIntersectionCount = 0;             //射线交点数
            for (int i = 0; i < sPointCount - 1; i++)
            {
                if (RayIntersectsSegment(aPoint, polygon[i], polygon[i + 1]))
                    sIntersectionCount++;
            }
            if (RayIntersectsSegment(aPoint, polygon[sPointCount - 1], polygon[0]))
                sIntersectionCount++;

            return sIntersectionCount % 2 == 1;
        }
    }
}
