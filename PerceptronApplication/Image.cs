using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronApplication
{
    class Image
    {
        private int type;
        private Matrix image;

        public Image(int n, double c)
        {
            this.type = 0;
            this.image = new Matrix(n + 1, 1);
            this.image.set(n, 0, c);
        }

        public int Lenght()
        {
            return this.image.LenghtStroke() - 1;
        }

        public int getType()
        {
            return type;
        }

        public void setType(int value)
        {
            type = value;
        }

        public Matrix get()
        {
            return image;
        }

        public double get(int x)
        {
            return image.get(x , 0);
        }

        public void set(int x, double value)
        {
            image.set(x, 0, value);
        }

        public void fillConsole()
        {
            Console.Write("Класс образа: ");
            this.type = Int16.Parse(Console.ReadLine());

            for (int i = 0; i < Lenght(); i++)
            {
                Console.Write("Cвойство №" + (i + 1) + ": ");
                this.image.set(i, 0, Int16.Parse(Console.ReadLine()));
            }
        }

        public override string ToString()
        {
            return "\nОбраз (Класс " + type + ") (послнджгнн число С)\n" + image.ToString();
        } 
    }
}
