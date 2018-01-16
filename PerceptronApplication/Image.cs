using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronConsoleApplication
{
    class Image
    {
        private int type;
        private Matrix image;

        public Image(int n, int c)
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

        public Matrix get()
        {
            return image;
        }

        public double get(int x)
        {
            return image.get(x , 0);
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
