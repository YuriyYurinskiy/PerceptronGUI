using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PerceptronApplication
{
    class Draw
    {
        public static int STEP = 20,
                          SHIFT = 2;

        public static SolidColorBrush color = new SolidColorBrush(Colors.Black);

        public static SolidColorBrush getColor(int color)
        {
            switch (color)
            {
                case 0:
                    return new SolidColorBrush(Colors.Blue);
                case 1:
                    return new SolidColorBrush(Colors.Red);
                case 2:
                    return new SolidColorBrush(Colors.Green);
                case 3:
                    return new SolidColorBrush(Colors.Purple);
                case 4:
                    return new SolidColorBrush(Colors.Gold);
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }

        // Начало координат
        public static Point getCenter(Canvas canvas)
        {
            return new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
        }

        // START Координатные  
        public static void drawCoord(Canvas canvas)
        {
            double x0 = canvas.ActualWidth / 2;
            double y0 = canvas.ActualHeight / 2;

            drawLine(canvas, 0, y0, canvas.ActualWidth, y0);
            drawLine(canvas, x0, 0, x0, canvas.ActualHeight);
        }

        public static void drawCoord(Canvas canvas, Point point)
        {
            drawLine(canvas, 0, point.Y, canvas.ActualWidth, point.Y);
            drawLine(canvas, point.X, 0, point.X, canvas.ActualHeight);
        }

        public static void drawStetOnCoord(Canvas canvas)
        {
            Point point = getCenter(canvas);

            double x0 = point.X, x1 = point.X;
            double y0 = point.Y, y1 = point.Y;

            while(x1 > STEP)
            {
                x0 += STEP;
                drawLine(canvas, x0, point.Y - SHIFT, x0, point.Y + SHIFT);

                x1 -= STEP;
                drawLine(canvas, x1, point.Y - SHIFT, x1, point.Y + SHIFT);
            }

            while (y1 > STEP)
            {
                y0 += STEP;
                drawLine(canvas, point.X - SHIFT, y0, point.X + SHIFT, y0);

                y1 -= STEP;
                drawLine(canvas, point.X - SHIFT, y1, point.X + SHIFT, y1);
            }
        }

        public static void drawLine(Canvas canvas, double x0, double y0, double x1, double y1)
        {
            var xStep1 = new Line
            {
                Stroke = color,
                StrokeThickness = 1,
                X1 = x0, Y1 = y0,
                X2 = x1, Y2 = y1,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            canvas.Children.Add(xStep1);
        }

        // END Координатные

        public static void drawNumber(Canvas canvas, int step)
        {

            TextBlock textBlock = new TextBlock();
            textBlock.Text = step.ToString();
            textBlock.Foreground = color;

            Canvas.SetLeft(textBlock, 5);
            Canvas.SetTop(textBlock, 5);

            canvas.Children.Add(textBlock);
        }

        // Точка на координатной плоскости 
        public static void drawPoint(Canvas canvas, double x0, double y0, SolidColorBrush fill)
        {
            int size = 4;

            var dot = new Ellipse
            {
                Fill = fill,
                Width = size,
                Height = size
            };

            Point p = getCenter(canvas);

            dot.SetValue(Canvas.LeftProperty, p.X + (x0 * STEP) - size / 2);
            dot.SetValue(Canvas.TopProperty, p.Y - (y0 * STEP) - size / 2);
            canvas.Children.Add(dot);
        }

        // Прямая на координатной плоскости 
        public static void drawLine(Canvas canvas, double x0, double y0, double x1, double y1, SolidColorBrush fill)
        {
            int size = 2;

            Point p = getCenter(canvas);

            var line = new Line
            {
                Stroke = fill,
                StrokeThickness = size,
                X1 = p.X + (x0 * STEP), Y1 = p.Y - (y0 * STEP),
                X2 = p.X + (x1 * STEP), Y2 = p.Y - (y1 * STEP),
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            canvas.Children.Add(line);

        }
        
        // Точки на координатной плоскости 
        public static void drawPoints(Canvas canvas, List<Image> images)
        {
            foreach (Image image in images)
            {
                drawPoint(canvas, image.get(0), image.get(1), getColor(image.getType()));
            }
        }
        
        // Прямые на координатной плоскости 
        public static void drawWeights(Canvas canvas, Matrix weights)
        {
            int type = 0;
            foreach (double[] weight in weights.get())
            {
                double x_1, y_1, x_2, y_2;
                double X = canvas.ActualWidth / STEP / 2,
                    Y = canvas.ActualHeight / STEP / 2;

                try
                {
                    if (weight[0] == 0 && weight[1] == 0)
                    {
                        //throw new Exception("Не могу построить прямую типа " + type);
                        break;
                    }
                    else if (weight[0] == 0)
                    {
                        x_1 = -X;
                        x_2 = X;
                        y_1 = -weight[2] / weight[1];
                        y_2 = -weight[2] / weight[1];
                    }
                    else if (weight[1] == 0)
                    {
                        y_1 = -Y;
                        y_2 = Y;
                        x_1 = -weight[2] / weight[0];
                        x_2 = -weight[2] / weight[0];
                    }
                    else
                    {
                        x_1 = -X;
                        x_2 = X;
                        y_1 = (-weight[2] - weight[0] * x_1) / weight[1];
                        y_2 = (-weight[2] - weight[0] * x_2) / weight[1];
                        if (y_1 >= Y)
                        {
                            y_1 = Y;
                            x_1 = (-weight[2] - weight[1] * y_1) / weight[0];
                        } else if (y_1 <= -Y)
                        {
                            y_1 = -Y;
                            x_1 = (-weight[2] - weight[1] * y_1) / weight[0];
                        }

                        if (y_2 >= Y)
                        {
                            y_2 = Y;
                            x_2 = (-weight[2] - weight[1] * y_2) / weight[0];
                        } else if(y_2 <= -Y)
                        {
                            y_2 = -Y;
                            x_2 = (-weight[2] - weight[1] * y_2) / weight[0];
                        }
                    }

                    drawLine(canvas, x_1, y_1, x_2, y_2, getColor(type));
                } catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString());
                } finally
                {
                    type++;
                }
            }
        }
    }
}
