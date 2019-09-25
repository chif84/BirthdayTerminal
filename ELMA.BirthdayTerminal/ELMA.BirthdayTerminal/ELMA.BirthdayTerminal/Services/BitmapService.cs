using ELMA.BirthdayTerminal.Services;
using SkiaSharp;
using System;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(BitmapService))]

namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис работы с изображениями
    /// </summary>
    public class BitmapService : IBitmapService
    {
        /// <summary>
        /// Вывести текст на изображение
        /// </summary>
        public byte[] TextOut(byte[] image, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return image;
            
            using (var bitmap = SKBitmap.Decode(image))
            {
                var canvas = new SKCanvas(bitmap);

                var rect = new SKRect();

                var font = SKTypeface.FromFamilyName("Arial");
                var paint = new SKPaint
                {
                    Typeface = font,
                    TextSize = Convert.ToInt64(Device.GetNamedSize(NamedSize.Large, typeof(Label))),
                    IsAntialias = true,
                    Color = new SKColor(255, 255, 255, 255),
                    TextAlign = SKTextAlign.Center,
                    Style = SKPaintStyle.StrokeAndFill
                };

                paint.MeasureText(text, ref rect);
                canvas.DrawText(text, bitmap.Width / 2, 5 + rect.Height, paint);

                var skImage = SKImage.FromBitmap(bitmap);
                var data = skImage.Encode(SKEncodedImageFormat.Png, 100);
                return data.ToArray();
            }
        }
    }
}