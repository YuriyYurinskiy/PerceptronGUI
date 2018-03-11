using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronApplication
{
    class Transport
    {
        private readonly String XXX = "xxx";
        private readonly String BAZ = "baz";
        private readonly bool MIN = true;
        private readonly bool MAX = false;

        private int m, // Число строк в матрице - число образов
            n, // Число столбцов в матрице - число признаков
            k; // Число базовых элементов

        private double[][] c;
        private double[] a, b;

        private double[][] maximize, minimize, zz, res;

        public Transport(double[][] c, int m, int n)
        {
            this.c = c;
            this.m = m;
            this.n = n;

            this.k = m + n - 1;

            this.a = new double[m];
            this.b = new double[n];

            for (int i = 0; i < m; i++)
            {
                this.a[i] = n;
            }
            for (int i = 0; i < n; i++)
            {
                this.b[i] = m;
            }
        }

        public double[][] getRes()
        {
            return res;
        }

        public void Init()
        {
            Dictionary<String, double[][]> data = SolveTask(c, a, b, MIN);

            minimize = data[XXX];

            data = SolveTask(c, a, b, MAX);

            maximize = data[XXX];

            zz = SubMatrix(minimize, maximize);

            res = MultiMatrix(c, T(zz));
        }

        private double[][] T(double[][] m1)
        {
            double[][] res = new double[m1[0].Length][];
            for (int i = 0; i < m1[0].Length; i++)
            {
                res[i] = new double[m1.Length];
                for (int j = 0; j < m1.Length; j++)
                {
                    res[i][j] = m1[j][i];
                }
            }

            return res;
        }


        private double[][] AddMatrix(double[][] m1, double[][] m2)
        {
            double[][] res = new double[m][];
            for (int i = 0; i < m; i++)
            {
                res[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    res[i][j] = m1[i][j] + m2[i][j];
                }
            }

            return res;
        }

        private double[][] SubMatrix(double[][] m1, double[][] m2)
        {
            double[][] res = new double[m1.Length][];
            for (int i = 0; i < m1.Length; i++)
            {
                res[i] = new double[m1[0].Length];
                for (int j = 0; j < m1[0].Length; j++)
                {
                    res[i][j] = m1[i][j] - m2[i][j];
                }
            }
                
            return res;
        }

        private double[][] MultiMatrix(double[][] m1, double[][] m2)
        {
            double[][] res = new double[m1.Length][];
            for (int i = 0; i < m1.Length; i++)
            {
                res[i] = new double[m2[0].Length];
                for (int j = 0; j < m2[0].Length; j++)
                {
                    res[i][j] = 0;
                    for (int k = 0; k < m2.Length; k++)
                    {
                        res[i][j] += m1[i][k] * m2[k][j];
                    }
                }
            }
            
            return res;
        }

        private Dictionary<String, double[][]> SolveTask(double[][]c, double[] a, double[] b, bool min)
        {
            Dictionary<String, double[][]> ss = NorthWest(c, a, b);
            double[][] x = ss[XXX];
            double[][] baz = ss[BAZ];

            double[] pot = Potential(c, baz);
            double[][] new1 = Newyazki(c, pot);
            int[] nw;

            if (min)
            {
                nw = NewPlusMin(new1);
            } else
            {
                nw = NewPlusMax(new1);
            }

            double fff = nw[0];
            int k = 0;

            while (fff == 1)
            {
                double[][] baz1 = Wich(nw[1], nw[2], baz);
                int[] contur = Contur(nw[1], nw[2], x, baz1);
                ss = ChangePotential(nw[1], nw[2], contur, c, x, baz);
                x = ss[XXX];
                baz = ss[BAZ];
                pot = Potential(c, baz);
                new1 = Newyazki(c, pot);

                if (min)
                {
                    nw = NewPlusMin(new1);
                } else
                {
                    nw = NewPlusMax(new1);
                }

                fff = nw[0];
                k++;
            }

            return ss;
        }

        private Dictionary<String, double[][]> NorthWest(double[][] c, double[] a, double[] b)
        {
            double[] aa = new double[a.Length];
            Array.Copy(a, aa, a.Length);

            double[] bb = new double[b.Length];
            Array.Copy(b, bb, b.Length);

            double[][] xxx = new double[m][];
            for (int ii = 0; ii < m; ii++)
                xxx[ii] = new double[n];

            double[][] baz = new double[m][];
            for (int ii = 0; ii < m; ii++)
                baz[ii] = new double[n];

            int f = 0, i = 0, j = 0;

            while (f == 0)
            {
                xxx[i][j] = Math.Min(aa[i], bb[j]);
                aa[i] -= xxx[i][j];
                bb[j] -= xxx[i][j];
                baz[i][j] = 1;
                int ff = 0;

                if (aa[i] < 0.1 && bb[j] > 0.1)
                    ff = 1;
                if (aa[i] > 0.1 && bb[j] < 0.1)
                    ff = 2;
                if (aa[i] < 0.1 && bb[j] < 0.1)
                    ff = 3;

                if (ff == 1)
                    i++;
                if (ff == 2)
                    j++;
                if (ff == 3)
                {
                    if (i < m - 1)
                        baz[i + 1][j] = 1;
                    i++;
                    j++;
                }

                if (i > m - 1 || j > n - 1)
                    f = 1;
            }

            Dictionary<String, double[][]> map = new Dictionary<string, double[][]>();
            map.Add(XXX, xxx);
            map.Add(BAZ, baz);

            return map;
        }
        
        private double[] Potential(double[][] c, double[][] baz)
        {
            double[] pot = new double[m + n];
            for (int i = 0; i < m + n; i++)
                pot[i] = 1000;

            pot[0] = 0;

            bool flag = true;

            while (flag)
            {
                flag = false;
                for (int i = 0; i < m; i++)
                {
                    if (pot[i] < 1000)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (baz[i][j] == 1)
                            {
                                pot[m + j] = c[i][j] - pot[i];
                            }
                        }
                    }
                    if (pot[i] == 1000)
                    {
                        flag = true;
                    }
                }
                flag = false;
                for (int j = 0; j < n; j++)
                {
                    if (pot[m + j] < 1000)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            if (baz[i][j] == 1)
                            {
                                pot[i] = c[i][j] - pot[m + j];
                            }
                        }
                    }
                    if (pot[m + j] == 1000)
                    {
                        flag = true;
                    }
                }
            }

            return pot;
        }
        
        private Dictionary<string, double[][]> ChangePotential(int ii, int jj, int[] contur, double[][] c, double[][] x, double[][] baz)
        {
            double[][] xxx = new double[x.Length][];
            for (int i = 0; i < x.Length; i++)
            {
                xxx[i] = new double[x[i].Length];
                for (int j = 0; j < x[i].Length; j++)
                {
                    xxx[i][j] = x[i][j];
                }
            }

            double[][] baz1 = new double[baz.Length][];
            for (int i = 0; i < baz.Length; i++)
            {
                baz1[i] = new double[baz[i].Length];
                for (int j = 0; j < baz[i].Length; j++)
                {
                    baz1[i][j] = baz[i][j];
                }
            }

            double cc = 0;
            int im = 1000;
            int jm = 1000;
            int t = 0;

            for (int i = 1; i <= contur[n + m + 3]; i++)
            {
                int iii = contur[t];
                int jjj = contur[t + 1];
                int iiii = contur[t + 2];
                xxx[iii][jjj] += contur[n + m + 2];
                xxx[iiii][jjj] -= contur[n + m + 2];

                if ( c[iiii][jjj] >= cc && xxx[iiii][jjj] == 0)
                {
                    im = iiii;
                    jm = jjj;
                }

                t += 2;
            }

            baz1[ii][jj] = 1;
            baz1[im][jm] = 0;

            Dictionary<String, double[][]> map = new Dictionary<string, double[][]>();
            map.Add(XXX, xxx);
            map.Add(BAZ, baz1);

            return map;
        }
        
        private double[][] Newyazki(double[][] c, double[] pot)
        {
            double[][] newyazki = new double[m][];
            for (int i = 0; i < m; i++)
                newyazki[i] = new double[n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    newyazki[i][j] = pot[i] + pot[m + j] - c[i][j];
                }
            }

            return newyazki;
        }
        
        private int[] NewPlusMin(double[][] new1)
        {
            int[] nw = new int[3];
            double nn = 0;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (new1[i][j] > nn)
                    {
                        nw[1] = i;
                        nw[2] = j;
                        nn = new1[i][j];
                        nw[0] = 1;
                    }
                }
            }

            return nw;
        }

        private int[] NewPlusMax(double[][] new1)
        {
            int[] nw = new int[3];
            double nn = 0;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (new1[i][j] < nn)
                    {
                        nw[1] = i;
                        nw[2] = j;
                        nn = new1[i][j];
                        nw[0] = 1;
                    }
                }
            }

            return nw;
        }

        private double[][] Wich(int x, int y, double[][] baz)
        {
            double[][] baz1 = new double[baz.Length][];
            for (int i = 0; i < baz.Length; i++)
            {
                baz1[i] = new double[baz[i].Length];
                for (int j = 0; j < baz[i].Length; j++)
                {
                    baz1[i][j] = baz[i][j];
                }
            }

            baz1[x][y] = 1;

            int flag = 1;

            while (flag == 1)
            {
                flag = 0;
                for (int i = 0; i < m; i++)
                {
                    int s = 0;
                    int jj = -1;
                    for (int j = 0; j < n; j++)
                    {
                        if (baz1[i][j] > 0)
                        {
                            s++;
                            jj = j;
                        }
                    }
                    if (s == 1)
                    {
                        flag = 1;
                        baz1[i][jj] = 0;
                    }
                }
                for (int j = 0; j < n; j++)
                {
                    int s = 0;
                    int ii = -1;
                    for (int i = 0; i < m; i++)
                    {
                        if (baz1[i][j] > 0)
                        {
                            s++;
                            ii = i;
                        }
                    }
                    if (s == 1)
                    {
                        flag = 1;
                        baz1[ii][j] = 0;
                    }
                }
            }

            return baz1;
        }
        
        private int[] Contur(int ii, int jj, double[][] x, double[][] baz)
        {
            int[] contur = new int[n + m + 3 + 1];
            contur[0] = ii;
            contur[1] = jj;
            int it = ii;
            int jt = jj;

            double[][] baz1 = new double[baz.Length][];
            for (int i = 0; i < baz.Length; i++)
            {
                baz1[i] = new double[baz[i].Length];
                for (int j = 0; j < baz[i].Length; j++)
                {
                    baz1[i][j] = baz[i][j];
                }
            }

            int u = 0;
            int k = 1;

            contur[n + m + 2] = 1000;
            while (u == 0)
            {
                k++;
                int i = 0;
                contur[n + m + 3] = k / 2;
                int f1 = 0;
                int ff = 0;

                while (f1 == 0)
                {
                    if (baz1[i][jt] == 1 && k == 2 && i == it)
                    {
                        ff = 1;
                    }
                    if (baz1[i][jt] == 1 && ff == 0)
                    {
                        it = i;
                        f1 = 1;
                    }

                    ff = 0;
                    i++;
                }

                baz1[it][jt] = 0;
                contur[k] = it;
                k++;

                if (x[it][jt] < contur[n + m + 2])
                {
                    contur[n + m + 2] = (int)x[it][jt];
                }

                int j = 0;
                int f2 = 0;

                while (f2 == 0)
                {
                    if (baz1[it][j] == 1)
                    {
                        jt = j;
                        f2 = 1;
                    }
                    j++;
                }

                contur[k] = jt;
                baz1[it][jt] = 0;

                if (it == ii && jt == jj)
                {
                    u = 1;
                }
            }

            return contur;
        }

    }
}
