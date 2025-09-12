using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace MyStudentsApp.MVVM.Models.SkiaControls
{
    // 1. NotificationBadgeView
    public class NotificationBadgeView : SKCanvasView
    {
        public static readonly BindableProperty BadgeNumberProperty =
            BindableProperty.Create(nameof(BadgeNumber), typeof(int), typeof(NotificationBadgeView), 0,
                propertyChanged: (bindable, oldVal, newVal) => ((NotificationBadgeView)bindable).InvalidateSurface());

        public int BadgeNumber
        {
            get => (int)GetValue(BadgeNumberProperty);
            set => SetValue(BadgeNumberProperty, value);
        }

        public SKColor BadgeColor { get; set; } = SKColors.Red;
        public SKColor TextColor { get; set; } = SKColors.White;
        public float BadgeRadius { get; set; } = 12f;

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            if (BadgeNumber <= 0) return;

            // Determinar posición: arriba derecha
            float cx = e.Info.Width - BadgeRadius;
            float cy = BadgeRadius;

            // Dibujar círculo
            using var paint = new SKPaint
            {
                Color = BadgeColor,
                IsAntialias = true
            };
            canvas.DrawCircle(cx, cy, BadgeRadius, paint);

            // Dibujar texto
            string text = BadgeNumber > 99 ? "99+" : BadgeNumber.ToString();
            using var textPaint = new SKPaint
            {
                Color = TextColor,
                TextSize = BadgeRadius + 4,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                Typeface = SKTypeface.Default
            };
            var textY = cy + textPaint.TextSize / 3;
            canvas.DrawText(text, cx, textY, textPaint);
        }
    }
}
