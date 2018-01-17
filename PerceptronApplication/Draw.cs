using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PerceptronApplication
{
    class Draw
    {
        public static int SIZE = 1,
                          STEP = 20,
                          SHIFT = 2;

        public static SolidColorBrush color = new SolidColorBrush(Colors.Black);

        public static Point getCenter(Canvas canvas)
        {
            return new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
        }

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
                StrokeThickness = SIZE,
                X1 = x0,
                Y1 = y0,
                X2 = x1,
                Y2 = y1,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            canvas.Children.Add(xStep1);
        }
    }
}
