using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Services;

namespace MyStudentsApp.MVVM.Views.Formularios
{
    public class CrearEstudianteView : ContentPage
    {
        public CrearEstudianteView(CrearEstudianteViewModel viewModelEstudianteCreacion)
        {
            BindingContext = viewModelEstudianteCreacion;
            Background = Background = new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
                        {
                            new GradientStop { Color = Color.FromArgb("#febb5a"), Offset = 0.0f },
                            new GradientStop { Color = Color.FromArgb("#48c1ec"), Offset = 0.2f },
                            new GradientStop { Color = Color.FromArgb("#163848"), Offset = 1.0f },
                        }
            };

            Content = CrearUi();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is CrearEstudianteViewModel viewModel)
            {
                // Lógica adicional si es necesaria
            }
        }

        public ScrollView CrearUi()
        {
            return new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = new Thickness(20),
                    Spacing = 20,
                    Children =
                    {
                        // Header con icono y título
                        new Border
                        {
                            Padding = new Thickness(20, 15),
                            BackgroundColor = Color.FromArgb("#ffffff"),
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
                            StrokeThickness = 0,
                            Shadow = new Shadow
                            {
                                Brush = Brush.Black,
                                Offset = new Point(0, 2),
                                Radius = 8,
                                Opacity = 0.1f
                            },
                            Content = new VerticalStackLayout
                            {
                                Spacing = 10,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "👨‍🎓",
                                        FontSize = 40,
                                        HorizontalOptions = LayoutOptions.Center
                                    },
                                    new Label
                                    {
                                        Text = "Crear Estudiante",
                                        FontSize = 28,
                                        FontAttributes = FontAttributes.Bold,
                                        TextColor = Color.FromArgb("#2c3e50"),
                                        HorizontalOptions = LayoutOptions.Center
                                    },
                                    new Label
                                    {
                                        Text = "Completa la información del nuevo estudiante",
                                        FontSize = 14,
                                        TextColor = Color.FromArgb("#7f8c8d"),
                                        HorizontalOptions = LayoutOptions.Center
                                    }
                                }
                            }
                        },

                        // Contenedor del formulario
                        new Border
                        {
                            Padding = new Thickness(20),
                            BackgroundColor = Color.FromArgb("#ffffff"),
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
                            StrokeThickness = 0,
                            Shadow = new Shadow
                            {
                                Brush = Brush.Black,
                                Offset = new Point(0, 2),
                                Radius = 8,
                                Opacity = 0.1f
                            },
                            Content = new VerticalStackLayout
                            {
                                Spacing = 20,
                                Children =
                                {
                                    // Email
                                    CrearCampoConIcono("✉️", "Email", Keyboard.Email, "EstudianteCreacionDTO.Email"),
                                    
                                    // Nombres
                                    CrearCampoConIcono("👤", "Nombres", Keyboard.Default, "EstudianteCreacionDTO.nombres"),
                                    
                                    // Apellidos
                                    CrearCampoConIcono("👥", "Apellidos", Keyboard.Default, "EstudianteCreacionDTO.apellidos"),
                                    
                                    // Teléfono
                                    CrearCampoConIcono("📱", "Teléfono", Keyboard.Telephone, "EstudianteCreacionDTO.PhoneNumber"),
                                    
                                    // Fecha de cumpleaños
                                    new VerticalStackLayout
                                    {
                                        Spacing = 8,
                                        Children =
                                        {
                                            new Label
                                            {
                                                Text = "🎂 Fecha de Cumpleaños",
                                                FontSize = 14,
                                                FontAttributes = FontAttributes.Bold,
                                                TextColor = Color.FromArgb("#2c3e50")
                                            },
                                            new Border
                                            {
                                                Padding = new Thickness(15, 12),
                                                BackgroundColor = Color.FromArgb("#f8f9fa"),
                                                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                                                Stroke = Color.FromArgb("#e9ecef"),
                                                StrokeThickness = 1,
                                                Content = new DatePicker
                                                {
                                                    Date = DateTime.Now,
                                                    TextColor = Color.FromArgb("#2c3e50"),
                                                    BackgroundColor = Colors.Transparent
                                                }.Bind(DatePicker.DateProperty, "EstudianteCreacionDTO.cumpleanos")
                                            }
                                        }
                                    },
                                    
                                    // Contraseña
                                    new VerticalStackLayout
                                    {
                                        Spacing = 8,
                                        Children =
                                        {
                                            new Label
                                            {
                                                Text = "🔒 Contraseña",
                                                FontSize = 14,
                                                FontAttributes = FontAttributes.Bold,
                                                TextColor = Color.FromArgb("#2c3e50")
                                            },
                                            new Border
                                            {
                                                Padding = new Thickness(15, 0),
                                                BackgroundColor = Color.FromArgb("#f8f9fa"),
                                                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                                                Stroke = Color.FromArgb("#e9ecef"),
                                                StrokeThickness = 1,
                                                Content = new Entry
                                                {
                                                    Placeholder = "Ingresa una contraseña segura",
                                                    IsPassword = true,
                                                    TextColor = Color.FromArgb("#2c3e50"),
                                                    PlaceholderColor = Color.FromArgb("#6c757d"),
                                                    BackgroundColor = Colors.Transparent
                                                }.Bind(Entry.TextProperty, "EstudianteCreacionDTO.contrasena")
                                            }
                                        }
                                    }
                                }
                            }
                        },

                        // Botón de crear
                        new Border
                        {
                            Padding = new Thickness(0),
                            BackgroundColor = Colors.Transparent,
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(25) },
                            StrokeThickness = 0,
                            Shadow = new Shadow
                            {
                                Brush = Brush.Black,
                                Offset = new Point(0, 4),
                                Radius = 10,
                                Opacity = 0.2f
                            },
                            Content = new Button
                            {
                                Text = "✨ Crear Estudiante",
                                FontSize = 18,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Colors.White,
                                BackgroundColor = Color.FromArgb("#28a745"),
                                HeightRequest = 50,
                                CornerRadius = 25
                            }.Bind(Button.CommandProperty, nameof(CrearEstudianteViewModel.CrearEstudianteCommand))
                        },

                        // Espaciado final
                        new BoxView { HeightRequest = 20, BackgroundColor = Colors.Transparent }
                    }
                }
            };
        }

        private VerticalStackLayout CrearCampoConIcono(string icono, string label, Keyboard keyboard, string bindingPath)
        {
            return new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    new Label
                    {
                        Text = $"{icono} {label}",
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#2c3e50")
                    },
                    new Border
                    {
                        Padding = new Thickness(15, 0),
                        BackgroundColor = Color.FromArgb("#f8f9fa"),
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                        Stroke = Color.FromArgb("#e9ecef"),
                        StrokeThickness = 1,
                        Content = new Entry
                        {
                            Placeholder = $"Ingresa {label.ToLower()}",
                            Keyboard = keyboard,
                            TextColor = Color.FromArgb("#2c3e50"),
                            PlaceholderColor = Color.FromArgb("#6c757d"),
                            BackgroundColor = Colors.Transparent
                        }.Bind(Entry.TextProperty, bindingPath)
                    }
                }
            };
        }
    }
}