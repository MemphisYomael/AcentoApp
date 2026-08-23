using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;

namespace MyStudentsApp.MVVM.Views.Formularios;

public class CrearCursoView : ContentPage
{
    public CrearCursoView(CrearCursoViewModel viewModel)
    {
        BindingContext = viewModel;
        Title = "Crear Curso";
        
        Background = new LinearGradientBrush
        {
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb("#febb5a"), Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#48c1ec"), Offset = 0.2f },
                new GradientStop { Color = Color.FromArgb("#163848"), Offset = 1.0f },
            }
        };

        Content = new ScrollView
        {
            Padding = new Thickness(20),
            Content = new VerticalStackLayout
            {
                Spacing = 20,
                Children =
                {
                    new Label
                    {
                        Text = "Nuevo Curso",
                        FontSize = 32,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.White,
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness(0, 20, 0, 30)
                    },
                    
                    new Border
                    {
                        StrokeShape = new RoundRectangle { CornerRadius = 15 },
                        BackgroundColor = Color.FromArgb("#163848"),
                        Stroke = Colors.White,
                        Padding = new Thickness(20),
                        Content = new VerticalStackLayout
                        {
                            Spacing = 15,
                            Children =
                            {
                                new Label { Text = "Nombre del Curso *", TextColor = Colors.White, FontSize = 16 },
                                new Entry
                                {
                                    Placeholder = "Ej: Matemáticas 10mo",
                                    PlaceholderColor = Colors.Gray,
                                    TextColor = Colors.Black,
                                    BackgroundColor = Colors.White
                                }
                                .Bind(Entry.TextProperty, nameof(CrearCursoViewModel.Nombre)),
                                
                                new Label { Text = "Descripción", TextColor = Colors.White, FontSize = 16 },
                                new Editor
                                {
                                    Placeholder = "Descripción del curso...",
                                    PlaceholderColor = Colors.Gray,
                                    TextColor = Colors.Black,
                                    BackgroundColor = Colors.White,
                                    HeightRequest = 120
                                }
                                .Bind(Editor.TextProperty, nameof(CrearCursoViewModel.Descripcion)),
                                
                                new Button
                                {
                                    Text = "Crear Curso",
                                    BackgroundColor = Color.FromArgb("#48c1ec"),
                                    TextColor = Colors.White,
                                    FontSize = 18,
                                    FontAttributes = FontAttributes.Bold,
                                    CornerRadius = 10,
                                    HeightRequest = 50,
                                    Margin = new Thickness(0, 20, 0, 0)
                                }
                                .Bind(Button.CommandProperty, nameof(CrearCursoViewModel.GuardarCommand))
                            }
                        }
                    }
                }
            }
        };
    }
}
