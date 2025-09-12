using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;

namespace MyStudentsApp.MVVM.Views.Formularios
{
    public class CrearProfesorView : ContentPage
    {
        public CrearProfesorView(CrearProfesorViewModel viewModel)
        {
            Title = "Crear Profesor";
            BindingContext = viewModel;
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
                                        Text = "👨‍🏫",
                                        FontSize = 40,
                                        HorizontalOptions = LayoutOptions.Center,
                                        FontFamily= "text",
                                    },
                                    new Label
                                    {
                                        Text = "Crear Profesor",
                                        FontSize = 28,
                                        FontAttributes = FontAttributes.Bold,
                                        TextColor = Color.FromArgb("#2c3e50"),
                                        HorizontalOptions = LayoutOptions.Center,
                                        FontFamily= "text"
                                    },
                                    new Label
                                    {
                                        Text = "Registra un nuevo docente en el sistema",
                                        FontSize = 14,
                                        TextColor = Color.FromArgb("#7f8c8d"),
                                        HorizontalOptions = LayoutOptions.Center,
                                        FontFamily= "text"
                                    }
                                }
                            }
                        },

                        // Información Personal
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
                                    new Label
                                    {
                                        Text = "📋 Información Personal",
                                        FontSize = 18,
                                        FontAttributes = FontAttributes.Bold,
                                        TextColor = Color.FromArgb("#2c3e50"),
                                        Margin = new Thickness(0, 0, 0, 10),
                                        FontFamily= "text"
                                    },
                                    
                                    // Nombres
                                    CrearCampoConIcono("👤", "Nombres", Keyboard.Default, "Profesor.nombres"),
                                    
                                    // Apellidos
                                    CrearCampoConIcono("👥", "Apellidos", Keyboard.Default, "Profesor.apellidos"),
                                    
                                    // Fecha de cumpleaños
                                    new VerticalStackLayout
                                    {
                                        Spacing = 8,
                                        Children =
                                        {
                                            new Label
                                            {
                                                Text = "🎂 Fecha de Nacimiento",
                                                FontSize = 14,
                                                FontAttributes = FontAttributes.Bold,
                                                TextColor = Color.FromArgb("#2c3e50"),
                                                FontFamily = "text"
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
                                                    TextColor = Color.FromArgb("#2c3e50"),
                                                    BackgroundColor = Colors.Transparent
                                                }.Bind(DatePicker.DateProperty, "Profesor.cumpleanos")
                                            }
                                        }
                                    }
                                }
                            }
                        },

                        // Información de Contacto
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
                                    new Label
                                    {
                                        Text = "📞 Información de Contacto",
                                        FontSize = 18,
                                        FontFamily = "text",
                                        FontAttributes = FontAttributes.Bold,
                                        TextColor = Color.FromArgb("#2c3e50"),
                                        Margin = new Thickness(0, 0, 0, 10),

                                    },
                                    
                                    // Email
                                    CrearCampoConIcono("✉️", "Email", Keyboard.Email, "Profesor.email"),
                                    
                                    // Teléfono
                                    CrearCampoConIcono("📱", "Teléfono", Keyboard.Telephone, "Profesor.telefono")
                                }
                            }
                        },

                        // Configuración de Cuenta
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
                                    new Label
                                    {
                                        Text = "⚙️ Configuración de Cuenta",
                                        FontSize = 18,
                                        FontAttributes = FontAttributes.Bold,
                                        TextColor = Color.FromArgb("#2c3e50"),
                                        Margin = new Thickness(0, 0, 0, 10),
                                        FontFamily = "text"
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
                                                                                        FontFamily= "text",

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
                                                    BackgroundColor = Colors.Transparent,
                                                    FontFamily = "text"
                                                }.Bind(Entry.TextProperty, "Profesor.contrasena")
                                            }
                                        }
                                    },

                                    // Switch Administrativo
                                    new Border
                                    {
                                        Padding = new Thickness(20, 15),
                                        BackgroundColor = Color.FromArgb("#f8f9fa"),
                                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                                        Stroke = Color.FromArgb("#e9ecef"),
                                        StrokeThickness = 1,
                                        Content = new Grid
                                        {
                                            ColumnDefinitions =
                                            {
                                                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                                                new ColumnDefinition { Width = GridLength.Auto }
                                            },
                                            Children =
                                            {
                                                new VerticalStackLayout
                                                {
                                                    Spacing = 5,
                                                    Children =
                                                    {
                                                        new Label
                                                        {
                                                            Text = "🔧 Permisos Administrativos",
                                                            FontSize = 14,
                                                            FontAttributes = FontAttributes.Bold,
                                                            TextColor = Color.FromArgb("#2c3e50"),
                                                            FontFamily = "text"
                                                        }.Column(0),
                                                        new Label
                                                        {
                                                            Text = "Otorga permisos de administrador al profesor",
                                                            FontSize = 12,
                                                            TextColor = Color.FromArgb("#6c757d"),
                                                            FontFamily = "text"
                                                        }.Column(0)
                                                    }
                                                }.Column(0),
                                                new Switch
                                                {
                                                    OnColor = Color.FromArgb("#28a745"),
                                                    ThumbColor = Colors.White,
                                                    VerticalOptions = LayoutOptions.Center
                                                }.Column(1).Bind(Switch.IsToggledProperty, "Profesor.isAdministrativo")
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
                                Text = "👨‍🏫 Crear Profesor",
                                FontSize = 18,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Colors.White,
                                BackgroundColor = Color.FromArgb("#007bff"),
                                HeightRequest = 50,
                                CornerRadius = 25,
                                FontFamily="text"
                                //Command = new Command(async () => await ((CrearProfesorViewModel)BindingContext).CrearProfesorAsync())
                            }.BindCommand("CrearProfesor")
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
                        TextColor = Color.FromArgb("#2c3e50"),
                                                                FontFamily= "text",

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
                                                                    FontFamily= "text",

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