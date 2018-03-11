using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PerceptronApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Image> images;
        Perceptron p;
        Transport t;

        int numImages, numCols;

        DataTable dt;
        DataTable dtWeight;

        public MainWindow()
        {
            images = new List<Image>();
            dt = new DataTable();
            dtWeight = new DataTable();

            InitializeComponent();

            textC.TextChanged += TextC_TextChanged;
            textAttr.TextChanged += TextAttr_TextChanged;
            textImages.TextChanged += TextImages_TextChanged;
            btnStart.Click += BtnStart_Click;
            btnStudy.Click += BtnStudy_Click;
            btnStepStudy.Click += BtnStepStudy_Click;
            btnNormize.Click += BtnNormize_Click;
            btnTransport.Click += BtnTransport_Click;

            textC.Text = "1";
            textAttr.Text = "2";
            textImages.Text = "4";
            textType.Text = "2";

            btnStudy.IsEnabled = false;
            btnStepStudy.IsEnabled = false;
            btnDraw.IsEnabled = false;
        }

        private void BtnTransport_Click(object sender, RoutedEventArgs e)
        {
            parseImages();

            foreach (Image image in images)
            {
                foreach(double[] d in image.get().get())
                {
                    textLog.Text += d[0] + " ";
                }

                textLog.Text += image.getType() + "\n";
            }

            double[][] c = new double[numImages][];

            for (int i = 0; i < numImages; i++)
            {
                c[i] = new double[numCols + 1];
                for (int j = 0; j < numCols + 1; j++)
                {
                    c[i][j] = images[i].get().get()[j][0];
                }
            }

            textLog.Text += "Проверка \n";

            foreach (double[] dd in c)
            {
                foreach (double d in dd)
                {
                    textLog.Text += d + " ";
                }

                textLog.Text += "\n";
            }

            t = new Transport(c, numImages, numCols + 1);
            t.Init();

            c = t.getRes();

            foreach (double[] dd in c)
            {
                foreach (double d in dd)
                {
                    textLog.Text += d + " ";
                }

                textLog.Text += "\n";
            }

            btnTransport.IsEnabled = false;
        }

        private void BtnNormize_Click(object sender, RoutedEventArgs e)
        {
            p.getWeights().Normize();
            buildGridWeight();
            drawCoord();
        }

        private void BtnStepStudy_Click(object sender, RoutedEventArgs e)
        {
            p.stepStudy();
            if(p.getComplete())
            {
                textLog.Text += "\nОбучение завершилось успешно на " + p.getStep() + " итерации";
                btnStudy.IsEnabled = false;
                btnStepStudy.IsEnabled = false;
            }
            else
            {
                textLog.Text += "\nВыполнена " + p.getStep() + " итерация";
            }

            textLog.Text += "\n" + p.getDStep().ToString();
            buildGridWeight();
            drawCoord();
        }

        private void BtnStudy_Click(object sender, RoutedEventArgs e)
        {
            if (p.getThreshold() < p.getStep())
            {
                p.addThreshold();
            }
            p.study();
            if (p.getComplete())
            {
                textLog.Text += "\nОбучение завершилось успешно на " + p.getStep() + " итерации";

                btnStudy.IsEnabled = false;
                btnStepStudy.IsEnabled = false;
            }
            else
            {
                textLog.Text += "\nСпустя " + p.getStep() + " итераций персепртрон не обучился";


                btnStudy.IsEnabled = true;
                btnStepStudy.IsEnabled = true;
            }

            buildGridWeight();
            drawCoord();
        }

        public void CreatePerceptron()
        {
            p = new Perceptron(images, Int32.Parse(textType.Text));

            textLog.Text = "Начинаем обучение персептрона";

            btnStudy.IsEnabled = true;
            btnStepStudy.IsEnabled = true;

            buildGridWeight();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            parseImages();
            CreatePerceptron();
            drawCoord();
        }



        private void TextC_TextChanged(object sender, TextChangedEventArgs e)
        {

            ChangeValue(false);
        }
        private void TextImages_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeValue(true);
        }
        private void TextAttr_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeValue(true);
        }

        //Изменение значений обработка
        private void ChangeValue(bool rebuilt)
        {
            double c;
            if (Int32.TryParse(textImages.Text, out numImages) &&
                Int32.TryParse(textAttr.Text, out numCols) &&
                Double.TryParse(textC.Text, out c))
            {
                fillImages(numImages, numCols, c, rebuilt);
            }
        }

        private void fillImages(int numImages, int numCols, double c, bool rebuilt)
        {
            images.Clear();
            for (int i = 0; i < numImages; i++)
            {
                images.Add(new Image(numCols, c));
            }
            if (rebuilt)
                buildGridImages(numCols);
        }

        private void buildGridImages(int numCols)
        {
            if (gridImages != null)
            {
                dt.Columns.Clear();
                for (int i = 1; i <= numCols; i++)
                {
                    dt.Columns.Add("#" + i.ToString());
                }
                dt.Columns.Add("Type");

                dt.Rows.Clear();

                foreach (Image image in images)
                {
                    var row = dt.NewRow();
                    for (int i = 1; i <= numCols; i++)
                    {
                        row["#" + i.ToString()] = image.get(i - 1);
                    }
                    row["Type"] = image.getType();

                    dt.Rows.Add(row);
                }

                gridImages.DataContext = null;
                gridImages.DataContext = dt.DefaultView;
            }
        }

        private void parseImages()
        {
            int numImages, numCols;
            double c;
            if (Int32.TryParse(textImages.Text, out numImages) &&
                Int32.TryParse(textAttr.Text, out numCols) &&
                Double.TryParse(textC.Text, out c))
            {

                for (int i = 0; i < numImages; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        images[i].set(j, Double.Parse(dt.Rows[i][j].ToString()));
                    }
                    images[i].setType(Int32.Parse(dt.Rows[i][numCols].ToString()));
                }
            }
        }

        private void buildGridWeight()
        {
            Matrix weights = p.getWeights();

            if (gridWeights != null && dtWeight != null && weights != null)
            {
                dtWeight.Columns.Clear();
                for (int i = 0; i < weights.LenghtElement(); i++)
                {
                    dtWeight.Columns.Add("" + i);
                }

                dtWeight.Rows.Clear();
                for(int i = 0; i < weights.LenghtStroke(); i++)
                {
                    var row = dtWeight.NewRow();
                    for (int j = 0; j < weights.LenghtElement(); j++)
                    {
                        row[j] = weights.get(i,j);
                    }

                    dtWeight.Rows.Add(row);
                }

                gridWeights.DataContext = null;
                gridWeights.DataContext = dtWeight.DefaultView;
            }
        }

        private void drawCoord()
        {
            if (Int32.Parse(textAttr.Text) == 2)
            {
                canvas.Children.Clear();

                Draw.drawCoord(canvas);
                Draw.drawStetOnCoord(canvas);
                Draw.drawNumber(canvas, p.getStep());
                Draw.drawPoints(canvas, images);
                Draw.drawWeights(canvas, p.getWeights());
            }
            else
            {
                //throw new Exception("Не возможно построить");
            }
        }
    }
}
