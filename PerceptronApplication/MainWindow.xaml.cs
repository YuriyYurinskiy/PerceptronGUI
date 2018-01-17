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
            btnDraw.Click += BtnDraw_Click;

            textC.Text = "1";
            textAttr.Text = "2";
            textImages.Text = "3";

            btnStudy.IsEnabled = false;
            btnStepStudy.IsEnabled = false;
        }

        private void BtnDraw_Click(object sender, RoutedEventArgs e)
        {
            drawGridWeight();
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

            buildGridWeight();
        }

        private void BtnStudy_Click(object sender, RoutedEventArgs e)
        {
            p.study();
            if (p.getComplete())
            {
                textLog.Text += "\nОбучение завершилось успешно на " + p.getStep() + " итерации";
            }
            else
            {
                textLog.Text += "\nСпустя " + p.getStep() + " итераций персепртрон не обучился";
            }

            btnStudy.IsEnabled = false;
            btnStepStudy.IsEnabled = false;

            buildGridWeight();
        }

        public void CreatePerceptron()
        {
            p = new Perceptron(images, 3);

            textLog.Text = "Начинаем обучение персептрона";

            btnStudy.IsEnabled = true;
            btnStepStudy.IsEnabled = true;

            buildGridWeight();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            parseImages();
            CreatePerceptron();
        }



        private void TextC_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeValue();
        }

        private void TextImages_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeValue();
        }

        private void TextAttr_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeValue();
        }

        //Изменение значений обработка
        private void ChangeValue()
        {
            int numImages, numCols, c;
            if (Int32.TryParse(textImages.Text, out numImages) &&
                Int32.TryParse(textAttr.Text, out numCols) &&
                Int32.TryParse(textC.Text, out c))
            {
                fillImages(numImages, numCols, c);
            }
        }

        private void fillImages(int numImages, int numCols, int c)
        {
            images.Clear();
            for (int i = 0; i < numImages; i++)
            {
                images.Add(new Image(numCols, c));
            }
            buildGridImages(numCols);
        }

        private void buildGridImages(int numCols)
        {
            if (gridImages != null)
            {
                dt.Columns.Clear();
                for (int i = 1; i <= numCols; i++)
                {
                    dt.Columns.Add("Column " + i.ToString());
                }
                dt.Columns.Add("Type");

                dt.Rows.Clear();

                foreach (Image image in images)
                {
                    var row = dt.NewRow();
                    for (int i = 1; i <= numCols; i++)
                    {
                        row["Column " + i.ToString()] = image.get(i - 1);
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
            int numImages, numCols, c;
            if (Int32.TryParse(textImages.Text, out numImages) &&
                Int32.TryParse(textAttr.Text, out numCols) &&
                Int32.TryParse(textC.Text, out c))
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
                        row["" + j] = weights.get(i,j);
                    }

                    dtWeight.Rows.Add(row);
                }

                gridWeights.DataContext = null;
                gridWeights.DataContext = dtWeight.DefaultView;
            }
        }

        private void plot()
        {
            canvas.Children.Clear();

            Draw.drawCoord(canvas);
            Draw.drawStetOnCoord(canvas);
        }

        private void drawGridWeight()
        {
            plot();

            if (Int32.Parse(textAttr.Text) == 2)
            {

            }
            else
            {
                throw new Exception("Не возможно построить");
            }
        }
    }
}
