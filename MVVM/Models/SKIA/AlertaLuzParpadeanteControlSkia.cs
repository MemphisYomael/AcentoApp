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
    public class AlertaLuzParpadeanteControlSkia : SKCanvasView
    {
        public static readonly BindableProperty colorLuz = BindableProperty.Create(
            nameof(colorLuzProperty), // Fixed: Ensure the correct property name is used
            typeof(SKColor),
            typeof(AlertaLuzParpadeanteControlSkia),
            SKColors.Red,
            propertyChanged: OnColorChanged);

        public SKColor colorLuzProperty
        {
            get => (SKColor)GetValue(colorLuz);
            set => SetValue(colorLuz, value);
        }

        public SKColor ColorLuz
        {
            get => (SKColor)GetValue(colorLuz);
            set => SetValue(colorLuz, value);
        }

        private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is SKColor newColor)
            {
                var control = (AlertaLuzParpadeanteControlSkia)bindable;
                control.Paint.Color = newColor;
                control.InvalidateSurface();
            }
        }

        public static readonly BindableProperty debeParpadear = BindableProperty.Create(
            nameof(debeParpadearProperty), // Fixed: Ensure the correct property name is used
            typeof(bool),
            typeof(AlertaLuzParpadeanteControlSkia),
            false,
            propertyChanged: OnIsBlinkingChanged);

        public bool debeParpadearProperty
        {
            get => (bool)GetValue(debeParpadear);
            set => SetValue(debeParpadear, value);
        }

        public bool DebeParpadear
        {
            get => (bool)GetValue(debeParpadear);
            set => SetValue(debeParpadear, value);
        }

        private static void OnIsBlinkingChanged(BindableObject bindable, object oldValue, object newValue)  
        {
            var control = (AlertaLuzParpadeanteControlSkia)bindable;
            if ((bool)newValue)
            {
                control.StartBlinking();
            }
            else
            {
                control.StopBlinking();
            }
        }

        private System.Timers.Timer _blinkTimer;
        private double _elapsedMs = 0;
        private const double BlinkDurationMs = 3000.0; // 3 segundos
        private const double FrameIntervalMs = 1000.0 / 120.0; // 120 fps

        private void StartBlinking()
        {
            if (_blinkTimer == null)
            {
                _blinkTimer = new System.Timers.Timer(FrameIntervalMs);
                _blinkTimer.Elapsed += (s, e) =>
                {
                    Dispatcher.Dispatch(() =>
                    {
                        _elapsedMs += FrameIntervalMs;
                        if (_elapsedMs > BlinkDurationMs)
                            _elapsedMs -= BlinkDurationMs;

                        // Suavidad: alpha = (sin(2πt/T - π/2) + 1) / 2
                        double t = _elapsedMs / BlinkDurationMs;
                        double alpha = (Math.Sin(2 * Math.PI * t - Math.PI / 2) + 1) / 2; // 0..1
                        byte alphaByte = (byte)(alpha * 255);

                        var baseColor = ColorLuz;
                        Paint.Color = new SKColor(baseColor.Red, baseColor.Green, baseColor.Blue, alphaByte);

                        InvalidateSurface();
                    });
                };
            }
            _blinkTimer.Start();
        }

        private void StopBlinking()
        {
            if (_blinkTimer != null)
            {
                _blinkTimer.Stop();
                _blinkTimer.Dispose();
                _blinkTimer = null;
            }
            InvalidateSurface(); // Ensure the light is drawn in its final state
        }

        public AlertaLuzParpadeanteControlSkia()
        {
            // Set the size of the control
            WidthRequest = 100;
            HeightRequest = 100;
            // Set the paint color
            Paint.Color = SKColors.Red;
            // Register the paint event
            PaintSurface += OnPaintSurface;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);
            canvas.DrawCircle(e.Info.Width / 2, e.Info.Height / 2, Math.Min(e.Info.Width, e.Info.Height) / 2, Paint);
        }

        public SKPaint Paint { get; } = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        protected void Dispose(bool disposing)
        {
            if (disposing && _blinkTimer != null)
            {
                _blinkTimer.Stop();
                _blinkTimer.Dispose();
                _blinkTimer = null;
            }
        }
       
    }
}
