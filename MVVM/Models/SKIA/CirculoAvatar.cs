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
    public class CirculoAvatar : SKCanvasView
    {
        public static readonly BindableProperty NombreUsuarioProperty =
            BindableProperty.Create(nameof(NombreUsuario), typeof(string), typeof(CirculoAvatar),
                defaultValue: "ACENTO", propertyChanged: OnUserChanged);

        private static void OnUserChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CirculoAvatar circuloAvatar && newValue is string newUserName)
            {
                circuloAvatar.NombreUsuario = newUserName;
                circuloAvatar.InvalidateSurface();
            }
        }

        public string? NombreUsuario
        {
            get => (string)GetValue(NombreUsuarioProperty);
            set => SetValue(NombreUsuarioProperty, value);
        }

        // Diccionario de colores para cada letra del alfabeto
        private readonly Dictionary<char, SKColor> _coloresPorLetra = new Dictionary<char, SKColor>
        {
            {'A', SKColors.Red},
            {'B', SKColors.Blue},
            {'C', SKColors.Green},
            {'D', SKColors.Orange},
            {'E', SKColors.Purple},
            {'F', SKColors.Teal},
            {'G', SKColors.Pink},
            {'H', SKColors.Brown},
            {'I', SKColors.Indigo},
            {'J', SKColors.Lime},
            {'K', SKColors.Maroon},
            {'L', SKColors.Navy},
            {'M', SKColors.Olive},
            {'N', SKColors.Coral},
            {'O', SKColors.Crimson},
            {'P', SKColors.DarkBlue},
            {'Q', SKColors.DarkGreen},
            {'R', SKColors.DarkOrange},
            {'S', SKColors.DarkViolet},
            {'T', SKColors.DeepPink},
            {'U', SKColors.DodgerBlue},
            {'V', SKColors.ForestGreen},
            {'W', SKColors.Gold},
            {'X', SKColors.HotPink},
            {'Y', SKColors.LightBlue},
            {'Z', SKColors.MediumPurple}
        };

        public CirculoAvatar()
        {
            WidthRequest = 100;
            HeightRequest = 100;
            PaintSurface += OnPaintSurface;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            if (string.IsNullOrWhiteSpace(NombreUsuario))
                return;

            // Obtener las primeras dos letras
            string iniciales = ObtenerIniciales(NombreUsuario);

            // Obtener color basado en la primera letra
            SKColor colorCirculo = ObtenerColorPorLetra(iniciales[0]);

            float radius = Math.Min(e.Info.Width, e.Info.Height) / 2f - 2; // -2 para margen
            float centerX = e.Info.Width / 2f;
            float centerY = e.Info.Height / 2f;

            // Dibujar el círculo de fondo
            var paintCirculo = new SKPaint
            {
                Color = colorCirculo,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            canvas.DrawCircle(centerX, centerY, radius, paintCirculo);

            // Dibujar el texto (iniciales)
            var paintTexto = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center,
                TextSize = radius * 0.8f, // Tamaño del texto proporcional al radio
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
            };

            // Calcular la posición Y para centrar verticalmente el texto
            var fontMetrics = paintTexto.FontMetrics;
            float textY = centerY - (fontMetrics.Ascent + fontMetrics.Descent) / 2;

            canvas.DrawText(iniciales, centerX, textY, paintTexto);
        }

        private string ObtenerIniciales(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "??";

            // Separar por espacios y tomar la primera letra de cada palabra
            var palabras = nombre.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (palabras.Length == 0)
                return "??";

            if (palabras.Length == 1)
            {
                // Si solo hay una palabra, tomar las primeras dos letras
                string palabra = palabras[0].ToUpper();
                return palabra.Length >= 2 ? palabra.Substring(0, 2) : palabra + "?";
            }
            else
            {
                // Si hay múltiples palabras, tomar la primera letra de las primeras dos palabras
                string primeraLetra = palabras[0].Length > 0 ? palabras[0][0].ToString().ToUpper() : "?";
                string segundaLetra = palabras[1].Length > 0 ? palabras[1][0].ToString().ToUpper() : "?";
                return primeraLetra + segundaLetra;
            }
        }

        private SKColor ObtenerColorPorLetra(char letra)
        {
            char letraUpper = char.ToUpper(letra);

            if (_coloresPorLetra.ContainsKey(letraUpper))
                return _coloresPorLetra[letraUpper];

            // Color por defecto si no se encuentra la letra
            return SKColors.Gray;
        }
    }
}