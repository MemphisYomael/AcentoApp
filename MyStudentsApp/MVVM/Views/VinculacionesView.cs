using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Shared.DTOShared;

namespace MyStudentsApp.MVVM.Views;

public class VinculacionesView : ContentPage
{
    private readonly VinculacionesViewModel _viewModel;
    
    public VinculacionesView(VinculacionesViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
        Title = "Vinculaciones";
        
        Background = new LinearGradientBrush
        {
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb("#febb5a"), Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#48c1ec"), Offset = 0.2f },
                new GradientStop { Color = Color.FromArgb("#163848"), Offset = 1.0f },
            }
        };

        Content = new Grid
        {
            Padding = new Thickness(10),
            RowSpacing = 10,
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            },
            Children =
            {
                // Selector de curso
                CrearSelectorCurso().Row(0),
                
                // Tabs para cambiar entre Estudiantes y Profesores
                CrearTabs().Row(1),
                
                // Contenido principal
                CrearContenidoPrincipal().Row(2)
            }
        };
    }

    private Border CrearSelectorCurso()
    {
        return new Border
        {
            Padding = new Thickness(15),
            Stroke = new SolidColorBrush(Colors.White),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            BackgroundColor = Color.FromArgb("#163848"),
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    new Label
                    {
                        Text = "Selecciona un Curso",
                        TextColor = Colors.White,
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold
                    },
                    
                    new Picker
                    {
                        Title = "Seleccionar curso...",
                        TextColor = Colors.White,
                        TitleColor = Colors.White,
                        BackgroundColor = Color.FromArgb("#48c1ec"),
                        ItemDisplayBinding = new Binding("nombre")

                    }
                    .Bind(Picker.ItemsSourceProperty, nameof(VinculacionesViewModel.Cursos))
                    .Bind(Picker.SelectedItemProperty, nameof(VinculacionesViewModel.CursoSeleccionado))

                }
            }
        };
    }

    private Border CrearTabs()
    {
        return new Border
        {
            BackgroundColor = Color.FromArgb("#163848"),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            Stroke = new SolidColorBrush(Colors.White),
            Padding = new Thickness(5),
            Content = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                    new ColumnDefinition(new GridLength(1, GridUnitType.Star))
                },
                Children =
                {
                    new Button
                    {
                        Text = "Estudiantes",
                        CornerRadius = 10,
                        HeightRequest = 45
                    }
                    .Column(0)
                    .Bind(Button.BackgroundColorProperty, nameof(VinculacionesViewModel.MostrandoEstudiantes),
                        convert: (bool mostrar) => mostrar ? Color.FromArgb("#48c1ec") : Colors.Transparent)
                    .Bind(Button.TextColorProperty, nameof(VinculacionesViewModel.MostrandoEstudiantes),
                        convert: (bool mostrar) => mostrar ? Colors.White : Colors.Gray)
                    .Bind(Button.CommandProperty, nameof(VinculacionesViewModel.MostrarEstudiantesCommand)),
                    
                    new Button
                    {
                        Text = "Profesores",
                        CornerRadius = 10,
                        HeightRequest = 45
                    }
                    .Column(1)
                    .Bind(Button.BackgroundColorProperty, nameof(VinculacionesViewModel.MostrandoProfesores),
                        convert: (bool mostrar) => mostrar ? Color.FromArgb("#48c1ec") : Colors.Transparent)
                    .Bind(Button.TextColorProperty, nameof(VinculacionesViewModel.MostrandoProfesores),
                        convert: (bool mostrar) => mostrar ? Colors.White : Colors.Gray)
                    .Bind(Button.CommandProperty, nameof(VinculacionesViewModel.MostrarProfesoresCommand))
                }
            }
        };
    }

    private Grid CrearContenidoPrincipal()
    {
        return new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            },
            ColumnSpacing = 10,
            Children =
            {
                // Vista de Estudiantes
                CrearVistaEstudiantes()
                    .Column(0).ColumnSpan(2)
                    .Bind(VisualElement.IsVisibleProperty, nameof(VinculacionesViewModel.MostrandoEstudiantes)),
                
                // Vista de Profesores
                CrearVistaProfesores()
                    .Column(0).ColumnSpan(2)
                    .Bind(VisualElement.IsVisibleProperty, nameof(VinculacionesViewModel.MostrandoProfesores))
            }
        };
    }

    private Grid CrearVistaEstudiantes()
    {
        return new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            },
            ColumnSpacing = 10,
            Children =
            {
                CrearListaDisponibles("Estudiantes Disponibles", 
                    nameof(VinculacionesViewModel.EstudiantesDisponibles),
                    nameof(VinculacionesViewModel.VincularEstudianteCommand),
                    "+", Colors.Green)
                    .Column(0),
                
                CrearListaVinculados("Estudiantes en el Curso",
                    nameof(VinculacionesViewModel.EstudiantesDelCurso),
                    nameof(VinculacionesViewModel.DesvincularEstudianteCommand),
                    "-", Colors.Red)
                    .Column(1)
            }
        };
    }

    private Grid CrearVistaProfesores()
    {
        return new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            },
            ColumnSpacing = 10,
            Children =
            {
                CrearListaProfesoresDisponibles().Column(0),
                CrearListaProfesoresVinculados().Column(1)
            }
        };
    }

    private Border CrearListaDisponibles(string titulo, string itemsSourceBinding, string commandBinding, string buttonText, Color buttonColor)
    {
        return new Border
        {
            Padding = new Thickness(10),
            Stroke = new SolidColorBrush(Colors.White),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            BackgroundColor = Color.FromArgb("#163848"),
            Content = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(1, GridUnitType.Star))
                },
                Children =
                {
                    new Label
                    {
                        Text = titulo,
                        TextColor = Colors.White,
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(0, 0, 0, 10)
                    }.Row(0),
                    
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() =>
                        {
                            var grid = new Grid
                            {
                                ColumnDefinitions = new ColumnDefinitionCollection
                                {
                                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                                    new ColumnDefinition(GridLength.Auto)
                                },
                                Padding = new Thickness(10, 5)
                            };

                            var label = new Label
                            {
                                TextColor = Colors.White,
                                VerticalOptions = LayoutOptions.Center
                            }
                            .Column(0)
                            .Bind(Label.TextProperty, ".", 
                                convert: (EstudianteResponseDTO? e) => 
                                    e != null ? $"{e.nombres} {e.apellidos}" : string.Empty);

                            var button = new Button
                            {
                                Text = buttonText,
                                BackgroundColor = buttonColor,
                                TextColor = Colors.White,
                                WidthRequest = 40,
                                HeightRequest = 40,
                                CornerRadius = 20,
                                FontSize = 20,
                                FontAttributes = FontAttributes.Bold
                            }
                            .Column(1)
                            .Bind(Button.CommandProperty, commandBinding, source: _viewModel)
                            .Bind(Button.CommandParameterProperty, ".");

                            grid.Children.Add(label);
                            grid.Children.Add(button);
                            return grid;
                        })
                    }
                    .Row(1)
                    .Bind(CollectionView.ItemsSourceProperty, itemsSourceBinding)
                }
            }
        };
    }

    private Border CrearListaVinculados(string titulo, string itemsSourceBinding, string commandBinding, string buttonText, Color buttonColor)
    {
        return CrearListaDisponibles(titulo, itemsSourceBinding, commandBinding, buttonText, buttonColor);
    }

    private Border CrearListaProfesoresDisponibles()
    {
        return new Border
        {
            Padding = new Thickness(10),
            Stroke = new SolidColorBrush(Colors.White),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            BackgroundColor = Color.FromArgb("#163848"),
            Content = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(1, GridUnitType.Star))
                },
                Children =
                {
                    new Label
                    {
                        Text = "Profesores Disponibles",
                        TextColor = Colors.White,
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(0, 0, 0, 10)
                    }.Row(0),
                    
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() =>
                        {
                            var grid = new Grid
                            {
                                ColumnDefinitions = new ColumnDefinitionCollection
                                {
                                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                                    new ColumnDefinition(GridLength.Auto)
                                },
                                Padding = new Thickness(10, 5)
                            };

                            var label = new Label
                            {
                                TextColor = Colors.White,
                                VerticalOptions = LayoutOptions.Center
                            }
                            .Column(0)
                            .Bind(Label.TextProperty, ".", 
                                convert: (ProfesorResponseDTO? p) => 
                                    p != null ? $"{p.nombres} {p.apellidos}" : string.Empty);

                            var button = new Button
                            {
                                Text = "+",
                                BackgroundColor = Colors.Green,
                                TextColor = Colors.White,
                                WidthRequest = 40,
                                HeightRequest = 40,
                                CornerRadius = 20,
                                FontSize = 20,
                                FontAttributes = FontAttributes.Bold
                            }
                            .Column(1)
                            .Bind(Button.CommandProperty, nameof(VinculacionesViewModel.VincularProfesorCommand), source: _viewModel)
                            .Bind(Button.CommandParameterProperty, ".");

                            grid.Children.Add(label);
                            grid.Children.Add(button);
                            return grid;
                        })
                    }
                    .Row(1)
                    .Bind(CollectionView.ItemsSourceProperty, nameof(VinculacionesViewModel.ProfesoresDisponibles))
                }
            }
        };
    }

    private Border CrearListaProfesoresVinculados()
    {
        return new Border
        {
            Padding = new Thickness(10),
            Stroke = new SolidColorBrush(Colors.White),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            BackgroundColor = Color.FromArgb("#163848"),
            Content = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(1, GridUnitType.Star))
                },
                Children =
                {
                    new Label
                    {
                        Text = "Profesores en el Curso",
                        TextColor = Colors.White,
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(0, 0, 0, 10)
                    }.Row(0),
                    
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() =>
                        {
                            var grid = new Grid
                            {
                                ColumnDefinitions = new ColumnDefinitionCollection
                                {
                                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                                    new ColumnDefinition(GridLength.Auto)
                                },
                                Padding = new Thickness(10, 5)
                            };

                            var label = new Label
                            {
                                TextColor = Colors.White,
                                VerticalOptions = LayoutOptions.Center
                            }
                            .Column(0)
                            .Bind(Label.TextProperty, ".", 
                                convert: (ProfesorResponseDTO? p) => 
                                    p != null ? $"{p.nombres} {p.apellidos}" : string.Empty);

                            var button = new Button
                            {
                                Text = "-",
                                BackgroundColor = Colors.Red,
                                TextColor = Colors.White,
                                WidthRequest = 40,
                                HeightRequest = 40,
                                CornerRadius = 20,
                                FontSize = 20,
                                FontAttributes = FontAttributes.Bold
                            }
                            .Column(1)
                            .Bind(Button.CommandProperty, nameof(VinculacionesViewModel.DesvincularProfesorCommand), source: _viewModel)
                            .Bind(Button.CommandParameterProperty, ".");

                            grid.Children.Add(label);
                            grid.Children.Add(button);
                            return grid;
                        })
                    }
                    .Row(1)
                    .Bind(CollectionView.ItemsSourceProperty, nameof(VinculacionesViewModel.ProfesoresDelCurso))
                }
            }
        };
    }
}
