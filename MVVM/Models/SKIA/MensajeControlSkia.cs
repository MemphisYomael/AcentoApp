using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Acento.MVVM.Models.SKIA
{
    public class MensajeControlSkia : SKCanvasView
    {
        public static readonly BindableProperty MensajeProperty = BindableProperty.Create(
            nameof(Mensaje),
            typeof(string),
            typeof(MensajeControlSkia),
            default(string));
        public string Mensaje
        {
            get => (string)GetValue(MensajeProperty);
            set => SetValue(MensajeProperty, value);
        }

        public static readonly BindableProperty senderUserNameProperty = BindableProperty.Create(
            nameof(senderUserName),
            typeof(string),
            typeof(MensajeControlSkia),
            default(string));

        public string senderUserName
        {
            get => (string)GetValue(senderUserNameProperty);
            set => SetValue(senderUserNameProperty, value);
        }

        public static readonly BindableProperty backgroundColorProperty = BindableProperty.Create(
            nameof(backgroundColor),
            typeof(Color),
            typeof(MensajeControlSkia),
            Colors.Transparent);

        public Color backgroundColor
        {
            get => (Color)GetValue(backgroundColorProperty);
            set => SetValue(backgroundColorProperty, value);
        }

        public static readonly BindableProperty multimediaUrlProperty = BindableProperty.Create(
            nameof(fotoUrl),
            typeof(string),
            typeof(MensajeControlSkia),
            default(string));

        public string fotoUrl
        {
            get => (string)GetValue(multimediaUrlProperty);
            set => SetValue(multimediaUrlProperty, value);
        }

        public MensajeControlSkia()
        {
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            var canvas = e.Surface.Canvas;
            var info = e.Info;
            canvas.Clear(Colors.Transparent.ToSKColor());

            // Create SKFont for text rendering
            using var font = new SKFont
            {
                Size = 12,
                Typeface = SKTypeface.Default
            };

            var paint = new SKPaint
            {
                Color = backgroundColor.ToSKColor(),
                IsAntialias = true,
            };

            var paintSombra = new SKPaint
            {
                Color = Colors.Black.ToSKColor(),
                IsAntialias = true,
            };
            canvas.DrawRect(new SKRect(0, 0, info.Width - (int)(info.Width * 0.05), (int)(info.Height * 0.05)), paint);
            canvas.DrawRect(new SKRect(0, 0, info.Width - (int)(info.Width), (int)(info.Height)), paintSombra);

            // Draw the message text
            canvas.DrawText(Mensaje, info.Width / 2f, info.Height / 2f, SKTextAlign.Center, font, new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true
            });

            if (!string.IsNullOrEmpty(fotoUrl))
            {
                // Load and draw the image if the URL is valid
                using var bitmap = SKBitmap.Decode(fotoUrl);
                if (bitmap != null)
                {
                    canvas.DrawBitmap(bitmap, new SKRect(0, 0, info.Width, info.Height));
                }
            }
        }
    }
}
