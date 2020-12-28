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

namespace Dagorlad_7.classes
{
    public class UserPictureProfileCreator
    {
        private static System.Drawing.Image DrawUserCircle(System.Drawing.Size PicSize, System.Drawing.Brush Brush,
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
                if (isgroup)
                    path.AddEllipse(rect_border);
                g.FillPath(Brush, path);

                System.Drawing.SizeF stringSize = g.MeasureString(Texto, Fuente); //<- Obtiene el tamaño del Texto en pixeles                                                                                 
                int posX = Convert.ToInt32((PicSize.Width - stringSize.Width) / 2); //<- Calcula la posicion para centrar el Texto
                int posY = Convert.ToInt32((PicSize.Height - stringSize.Height) / 2);

                // Dibuja el Texto:
                g.DrawString(Texto, Fuente, new System.Drawing.SolidBrush(Foreground), new System.Drawing.Point(posX, posY));

                //christmas hat  
                var dt_now = DateTime.Now;
                if ((dt_now.Month == 12 && dt_now.Day >= 20) || (dt_now.Month == 1 && dt_now.Day <= 13))
                    g.DrawImage(ConvertFromBitmapImageToBitmapData(new Uri(@"pack://application:,,,/Dagorlad;component/images/hat_40.png", UriKind.Absolute)), new Point(0, 0));
            }
            
            return Canvas;
        }
        private static Bitmap ConvertFromBitmapImageToBitmapData(Uri uri)
        {
            BitmapImage image = new BitmapImage(uri);
            int stride = image.PixelWidth * 4;
            byte[] buffer = new byte[stride * image.PixelHeight];
            image.CopyPixels(buffer, stride, 0);
            System.Drawing.Bitmap bitmap =
                new System.Drawing.Bitmap(
                    image.PixelWidth,
                    image.PixelHeight,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Imaging.BitmapData bitmapData =
                bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    bitmap.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(
                buffer, 0, bitmapData.Scan0, buffer.Length);
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
        private static string ConvertText(string text)
        {
            string result = String.Empty;
            if (text.Length > 3)
            {
                List<char> list = new List<char>();
                text.Split(' ').ToList().ForEach(i => list.Add(i[0]));
                foreach (var s in list)
                {
                    result += s;
                }
            }
            else
            {
                result = text;
            }
            return result;
        }
        private static List<Brush> SelectedBrushesList = new List<Brush>();
        public static BitmapImage CreateUserPictureProfile(string text, bool isgroup)
        {
            Brush pickedbrush = PickBrush();
            while (SelectedBrushesList.Contains(pickedbrush))
            {
                pickedbrush=PickBrush();
            }
            SelectedBrushesList.Add(pickedbrush);
            var color = ((SolidBrush)pickedbrush).Color;
            var forecolor = GetReadableForeColor(color);
            int fontsize = 20;
            if (isgroup) fontsize = 42;
            var image = DrawUserCircle(new System.Drawing.Size(64, 64), pickedbrush, ConvertText(text),
                new System.Drawing.Font("Arial", fontsize, System.Drawing.FontStyle.Bold,GraphicsUnit.Pixel,1,true), System.Drawing.Color.Transparent,
                forecolor, isgroup);
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
        private static Color GetReadableForeColor(Color color)
        {
            int d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (luminance > 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            return Color.FromArgb(d, d, d);
        }
        private static int PerceivedBrightness(Color c)
        {
            return (int)Math.Sqrt(
            c.R * c.R * .241 +
            c.G * c.G * .691 +
            c.B * c.B * .068);
        }
        private static Brush PickBrush()
        {
            Brush result = Brushes.Transparent;
            Random rnd = new Random();
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();
            int random = GenerateRandomNumber(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            return result;
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
                    random = new Random();
                return random.Next(max);
            }
        }
    }
}
