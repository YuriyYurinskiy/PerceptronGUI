using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronApplication
{
    class Perceptron
    {
        // Матрица весов
        private Matrix weights;
        // Список образов
        private List<Image> images;

        private Matrix d;

        int step,
            not_error,
            threshold;

        bool complete;


        public Perceptron(List<Image> images, int number_type_images)
        {
            this.images = images;
            this.weights = new Matrix(number_type_images, images[0].Lenght() + 1);
            this.step = 0;
            this.not_error = 0;
            this.threshold = 1000;
            this.complete = false;
        }

        private bool changeWeights(Image image, Matrix _out)
        {
            bool error = false;
            for (int i = 0; i < weights.LenghtStroke(); i++)
            {
                if (i == image.getType())
                {
                    if (_out.get(i, 0) <= 0)
                    {
                        for (int j = 0; j < weights.LenghtElement(); j++)
                        {
                            weights.set(i, j, weights.get(i, j) + image.get(j));
                        }
                        error = true;
                    }                   
                } else
                {
                    if (_out.get(i, 0) >= 0)
                    {
                        for (int j = 0; j < weights.LenghtElement(); j++)
                        {
                            weights.set(i, j, weights.get(i, j) - image.get(j));
                        }
                        error = true;
                    }
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

        public void stepStudy()
        {
            if (complete)
            {
                return;
            }

            Image work = images[step % images.Count];
            step++; // изменяем шаг выполняемой итерации

            d = getD(work);
            if (changeWeights(work,d))
            {
                not_error = 0;
            }
            else
            {
                not_error++;
                if (not_error >= images.Count)
                {
                    complete = true;
                    return;
                }
            }

            complete = false;
            return;
        }

        public void study()
        {
            if (complete)
            {
                return;
            }

            do
            {
                stepStudy();

                if (step > threshold)
                {
                    complete = false;
                    return;
                }
            } while (!complete);
        }

        public Matrix getWeights()
        {
            return weights;
        }

        public int getStep()
        {
            return step;
        }

        public bool getComplete()
        {
            return complete;
        }

        public Matrix getDStep()
        {
            return d;
        }

        public void addThreshold()
        {
            threshold += 1000;
            study();
        }

        public int getThreshold()
        {
            return threshold;
        }


    }
}
