using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronConsoleApplication
{
    class Matrix
    {
        private int n, m;
        private double[][] matrix;

        public Matrix(int n, int m)
        {
            this.n = n;
            this.m = m;
            this.matrix = new double[this.n][];
            for (int i = 0; i < this.n; i++)
            {
                this.matrix[i] = new double[this.m];
                for (int j = 0; j < this.m; j++) {
                    this.matrix[i][j] = 0;
                }
            }
        }

        public void set(int x, int y, double value)
        {
            this.matrix[x][y] = value;
        }

        public double get(int x, int y)
        {
            return this.matrix[x][y];
        }

        public int LenghtElement()
        {
            return m;
        }

        public int LenghtStroke()
        {
            return n;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder("");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    s.Append(matrix[i][j]);
                    s.Append("\t");
                }

                s.Append("\n");
            }

            return s.ToString();
        }

        public static Matrix operator *(Matrix x, int _const)
        {
            Matrix result = new Matrix(x.LenghtStroke(), x.LenghtElement());

            for (int i = 0; i < x.LenghtStroke(); i++)
            {
                for (int j = 0; j < x.LenghtElement(); j++)
                {
                    result.matrix[i][j] += x.matrix[i][j] * _const;
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix x, Matrix y)
        {
            if (x.LenghtElement() != y.LenghtStroke())
                throw new Exception("Число столюцов первой матрицы не равно числу строк второй.");

            Matrix result = new Matrix(x.LenghtStroke(), y.LenghtElement());

            for (int i = 0; i < x.LenghtStroke(); i++)
            {
                for (int j = 0; j < y.LenghtElement(); j++)
                {
                    for (int k = 0; k < x.LenghtElement(); k++)
                        result.matrix[i][j] += x.matrix[i][k] * y.matrix[k][j];
                }
            }

            return result;
        }

        public static Matrix operator +(Matrix x, Matrix y)
        {
            if (x.LenghtElement() != y.LenghtElement() || x.LenghtStroke() != y.LenghtStroke())
                throw new Exception("Число столбцов и строк 2-х матриц не равны.");

            Matrix result = new Matrix(x.LenghtStroke(), x.LenghtElement());

            for (int i = 0; i < x.LenghtStroke(); i++)
            {
                for (int j = 0; j < x.LenghtElement(); j++)
                {
                    result.matrix[i][j] = x.matrix[i][j] + y.matrix[i][j];
                }
            }

            return result;
        }

        public static Matrix operator -(Matrix x, Matrix y)
        {
            return x + (y * (-1));
        }
    }
}
