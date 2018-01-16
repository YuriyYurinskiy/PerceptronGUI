using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronConsoleApplication
{
    class Perceptron
    {
        private Matrix weights;
        private List<Image> images;

        public Perceptron(List<Image> images, int number_type_images)
        {
            this.images = images;

            this.weights = new Matrix(number_type_images, images[0].Lenght() + 1);
        }

        private bool changeWeights(Image image, Matrix _out)
        {
            bool error = false;
            for (int i = 0; i < weights.LenghtStroke(); i++)
            {
                if (check(_out, image.getType()))
                {
                    break;
                }
                else if (i == image.getType())
                {
                    for (int j = 0; j < weights.LenghtElement(); j++)
                    {
                        weights.set(i, j, weights.get(i, j) + image.get(j));
                    }
                    error = true;
                }
                else
                {
                    for (int j = 0; j < weights.LenghtElement(); j++)
                    {
                        weights.set(i, j, weights.get(i, j) - image.get(j));
                    }
                    error = true;
                }
            }

            return error;
        }

        private bool check(Matrix _out, int type)
        {
            if (_out.get(type, 0) < 0)
                return false;

            for (int i = 0; i <_out.LenghtStroke(); i++)
            {
                if (_out.get(i, 0) >= 0 && i != type)
                    return false;
            }

            return true;
        }

        private Matrix getD(Image image)
        {
            return weights * image.get();
        }

        public void study()
        {
            bool flag;
            int error = 0, step = 1;
            do
            {
                flag = true;
                foreach (Image image in images)
                {
                    if (changeWeights(image, getD(image)))
                    {
                        error = 0;
                    }
                    else
                    {
                        error++;
                        if (error >= images.Count)
                        {
                            flag = false;
                            break;
                        }
                    }
                    step++;
                    Console.WriteLine("Step " + step);
                }
                
            } while (flag && step < 1000);

            Console.WriteLine(weights.ToString());
        }
    }
}
