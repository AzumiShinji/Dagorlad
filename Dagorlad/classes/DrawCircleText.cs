using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;

namespace Dagorlad.classes
{
    class DrawCircleText
    {
        private static HashSet<System.Drawing.Brush> custom_brushes = new HashSet<System.Drawing.Brush>();
        private static string[] color_array_hex = new string[] {
            "#952222","#ED254E",
            "#955822","#F9DC5C",
            "#958C22","#C2EABD",
            "#829522","#011936",
            "#669522","#465362",
            "#3A9522","#BB4430",
            "#22953A","#7EBDC2",
            "#22955D","#F3DFA2",
            "#229580","#EFE6DD",
            "#229295","#E4572E",
            "#226D95","#29335C",
            "#224F95","#F3A712",
            "#222495","#A8C686",
            "#432295","#669BBC",
            "#642295","#E4572E",
            "#842295","#29335C",
            "#952289","#F3A712",
            "#952274","#A8C686",
            "#95225C","#669BBC",
            "#952222","#669BBC",
        };
        private static void generateSolidBrush()
        {
            foreach (var s in color_array_hex)
            {
                System.Drawing.Brush brush = new System.Drawing.SolidBrush((System.Drawing.Color)new System.Drawing.ColorConverter().
                ConvertFromString(new System.Windows.Media.BrushConverter().ConvertToString(s)));
                custom_brushes.Add(brush);
            }
        }
    public static System.Drawing.Image DrawUserCircle(System.Drawing.Size PicSize, System.Drawing.Brush Brush, 
            string Texto, System.Drawing.Font Fuente, System.Drawing.Color BackColor, System.Drawing.Color Foreground,
            bool isgroup)
        {
            //Imagen Virtual sobre la que se dibuja y se retorna al final:
            System.Drawing.Image Canvas = new System.Drawing.Bitmap(PicSize.Width, PicSize.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(Canvas);

            //Tamaño del Circulo (rect):
            System.Drawing.Rectangle outerRect = new System.Drawing.Rectangle(-1, -1, Canvas.Width + 1, Canvas.Height + 1);
            System.Drawing.Rectangle rect = System.Drawing.Rectangle.Inflate(outerRect, -2, -2);
            //border
            System.Drawing.Rectangle rect_border = System.Drawing.Rectangle.Inflate(outerRect, -10, -10);
            //Todas las opciones en Alta Calidad:
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                //Fondo del Picture:
                g.FillRectangle(new System.Drawing.SolidBrush(BackColor),
                    new System.Drawing.RectangleF(0, 0, PicSize.Width, PicSize.Height));

                //Dibuja el Circulo:
                path.AddEllipse(rect);
                if(isgroup)
                path.AddEllipse(rect_border);
                g.FillPath(Brush, path);

                System.Drawing.SizeF stringSize = g.MeasureString(Texto, Fuente); //<- Obtiene el tamaño del Texto en pixeles                                                                                 
                int posX = Convert.ToInt32((PicSize.Width - stringSize.Width) / 2); //<- Calcula la posicion para centrar el Texto
                int posY = Convert.ToInt32((PicSize.Height - stringSize.Height) / 2);

                // Dibuja el Texto:
                g.DrawString(Texto, Fuente, new System.Drawing.SolidBrush(Foreground), new System.Drawing.Point(posX, posY));
            }
            return Canvas;
        }

        public static BitmapImage CreateProfilePicture(string text,bool isgroup)
        {
            var image = DrawUserCircle(new System.Drawing.Size(80, 80), PickBrush(), text, 
                new System.Drawing.Font("Arial", 18, System.Drawing.FontStyle.Bold), System.Drawing.Color.Transparent,
                System.Drawing.Color.WhiteSmoke,isgroup);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
        private static Brush PickBrush()
        {
            generateSolidBrush();
            int random = GenerateRandomNumber(custom_brushes.Count());
            return (Brush)custom_brushes.ElementAt(random);
        }
        private static Random random;
        private static object syncObj = new object();
        private static void InitRandomNumber(int seed)
        {
            random = new Random(seed);
        }
        private static int GenerateRandomNumber(int max)
        {
            lock (syncObj)
            {
                if (random == null)
                    random = new Random(); // Or exception...
                return random.Next(max);
            }
        }
    }
}
