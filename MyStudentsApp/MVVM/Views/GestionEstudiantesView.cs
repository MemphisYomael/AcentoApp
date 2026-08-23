using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Shared.DTOShared;

namespace MyStudentsApp.MVVM.Views;

public class GestionEstudiantesView : ContentPage
{
    private readonly GestionEstudiantesViewModel _viewModel;
    
    public GestionEstudiantesView(GestionEstudiantesViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
        Title = "Gestión de Estudiantes";
        
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
                new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            },
            Children =
            {
                // Barra de búsqueda y botón nuevo
                new Border
                {
                    BackgroundColor = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                    Content = new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                            new ColumnDefinition(new GridLength(60, GridUnitType.Absolute))
                        },
                        Children =
                        {
                            new SearchBar
                            {
                                Placeholder = "Buscar estudiante...",
                                PlaceholderColor = Colors.White,
                                TextColor = Colors.Black,
                                HorizontalOptions = LayoutOptions.Fill,
                                VerticalOptions = LayoutOptions.Center
                            }
                            .Column(0)
                            .Bind(SearchBar.TextProperty, nameof(GestionEstudiantesViewModel.BuscarCommand), mode: BindingMode.OneWayToSource),

                        }
                    }
                }
                .Row(0),

                // Lista de estudiantes o formulario
                new Grid
                {
                    Children =
                    {
                        // Lista de estudiantes
                        CrearListaEstudiantes()
                            .Bind(VisualElement.IsVisibleProperty, nameof(GestionEstudiantesViewModel.MostrarFormularioEdicion), 
                                  convert: (bool mostrar) => !mostrar),
                        
                        // Formulario de edición
                        CrearFormularioEdicion()
                            .Bind(VisualElement.IsVisibleProperty, nameof(GestionEstudiantesViewModel.MostrarFormularioEdicion))
                    }
                }
                .Row(1)
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarEstudiantes();
    }

    private Border CrearListaEstudiantes()
    {
        return new Border
        {
            Padding = new Thickness(10),
            Stroke = new SolidColorBrush(Colors.White),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
            BackgroundColor = Color.FromArgb("#163848"),
            Content = new CollectionView
            {
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                ItemTemplate = new DataTemplate(() =>
                {
                    var swipeView = new SwipeView();

                    var editSwipeItem = new SwipeItem
                    {
                        Text = "Editar",
                        BackgroundColor = Color.FromArgb("#48c1ec"),
                        IconImageSource = "edit.png"
                    }
                    .Bind(SwipeItem.CommandProperty, nameof(GestionEstudiantesViewModel.EditarCommand), source: _viewModel)
                    .Bind(SwipeItem.CommandParameterProperty, ".");

                    var deleteSwipeItem = new SwipeItem
                    {
                        Text = "Eliminar",
                        BackgroundColor = Colors.Red,
                        IconImageSource = "delete.png"
                    }
                    .Bind(SwipeItem.CommandProperty, nameof(GestionEstudiantesViewModel.EliminarCommand), source: _viewModel)
                    .Bind(SwipeItem.CommandParameterProperty, ".");

                    var tarjeta = new Border
                    {
                        BackgroundColor = Colors.White,
                        Margin = new Thickness(0, 10, 0, 0),
                        Padding = new Thickness(15),
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
                        Content = new Grid
                        {
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition(new GridLength(60, GridUnitType.Absolute)),
                                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
                            },
                            RowDefinitions = new RowDefinitionCollection
                            {
                                new RowDefinition(GridLength.Auto),
                                new RowDefinition(GridLength.Auto),
                                new RowDefinition(GridLength.Auto)
                            },
                            Children =
                            {
                                new Frame
                                {
                                    BackgroundColor = Colors.DarkOrange,
                                    CornerRadius = 30,
                                    WidthRequest = 50,
                                    HeightRequest = 50,
                                    Padding = 0,
                                    HasShadow = false,
                                    Content = new Label
                                    {
                                        FontSize = 20,
                                        FontAttributes = FontAttributes.Bold,
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.Center,
                                        TextColor = Colors.White
                                    }
                                    .Bind(Label.TextProperty, ".", 
                                        convert: (EstudianteResponseDTO? e) => 
                                            e != null && !string.IsNullOrEmpty(e.nombres) 
                                                ? e.nombres.Substring(0, 1).ToUpper() 
                                                : "E")
                                }
                                .Row(0).RowSpan(2).Column(0),

                                new Label
                                {
                                    FontSize = 18,
                                    FontAttributes = FontAttributes.Bold,
                                    TextColor = Colors.Black
                                }
                                .Row(0).Column(1)
                                .Bind(Label.TextProperty, ".", 
                                    convert: (EstudianteResponseDTO? e) => 
                                        e != null ? $"{e.nombres} {e.apellidos}" : string.Empty),

                                new Label
                                {
                                    FontSize = 14,
                                    TextColor = Colors.Gray
                                }
                                .Row(1).Column(1)
                                .Bind(Label.TextProperty, nameof(EstudianteResponseDTO.Email)),

                                new Label
                                {
                                    FontSize = 12,
                                    TextColor = Colors.DarkOrange,
                                    FontAttributes = FontAttributes.Bold
                                }
                                .Row(2).Column(1)
                                .Bind(Label.TextProperty, nameof(EstudianteResponseDTO.curso),
                                    convert: (string? curso) => !string.IsNullOrEmpty(curso) ? $"📚 {curso}" : "")
                            }
                        }
                    };

                    swipeView.RightItems = new SwipeItems { editSwipeItem, deleteSwipeItem };
                    swipeView.Content = tarjeta;
                    return swipeView;
                })
            }
            .Bind(CollectionView.ItemsSourceProperty, nameof(GestionEstudiantesViewModel.EstudiantesFiltrados))
        };
    }

    private ScrollView CrearFormularioEdicion()
    {
        return new ScrollView
        {
            Content = new Border
            {
                Padding = new Thickness(20),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                BackgroundColor = Color.FromArgb("#163848"),
                Content = new VerticalStackLayout
                {
                    Spacing = 15,
                    Children =
                    {
                        new Label
                        {
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Colors.White,
                            HorizontalOptions = LayoutOptions.Center
                        }
                        .Bind(Label.TextProperty, nameof(GestionEstudiantesViewModel.EsNuevoEstudiante),
                            convert: (bool esNuevo) => esNuevo ? "Nuevo Estudiante" : "Editar Estudiante"),

                        CrearCampo("Nombres *", nameof(GestionEstudiantesViewModel.Nombres)),
                        CrearCampo("Apellidos", nameof(GestionEstudiantesViewModel.Apellidos)),
                        CrearCampo("Email", nameof(GestionEstudiantesViewModel.Email), Keyboard.Email),
                        CrearCampo("Teléfono", nameof(GestionEstudiantesViewModel.Telefono), Keyboard.Telephone),
                        CrearCampo("Contraseña", nameof(GestionEstudiantesViewModel.Contrasena), isPassword: true),

                        new Label
                        {
                            Text = "Fecha de Nacimiento",
                            TextColor = Colors.White,
                            FontSize = 14
                        },

                        new DatePicker
                        {
                            BackgroundColor = Colors.White,
                            TextColor = Colors.Black,
                            Format = "dd/MM/yyyy"
                        }
                        .Bind(DatePicker.DateProperty, nameof(GestionEstudiantesViewModel.Cumpleanos)),

                        new Grid
                        {
                            ColumnSpacing = 10,
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
                            },
                            Children =
                            {
                                new Button
                                {
                                    Text = "Cancelar",
                                    BackgroundColor = Colors.Gray,
                                    TextColor = Colors.White,
                                    CornerRadius = 10,
                                    HeightRequest = 50
                                }
                                .Column(0)
                                .Bind(Button.CommandProperty, nameof(GestionEstudiantesViewModel.CancelarCommand)),

                                new Button
                                {
                                    Text = "Guardar",
                                    BackgroundColor = Color.FromArgb("#48c1ec"),
                                    TextColor = Colors.White,
                                    CornerRadius = 10,
                                    HeightRequest = 50
                                }
                                .Column(1)
                                .Bind(Button.CommandProperty, nameof(GestionEstudiantesViewModel.GuardarCommand))
                            }
                        }
                    }
                }
            }
        };
    }

    private Frame CrearCampo(string placeholder, string bindingPath, Keyboard? keyboard = null, bool isPassword = false)
    {
        return new Frame
        {
            BackgroundColor = Colors.White,
            CornerRadius = 10,
            Padding = new Thickness(15, 10),
            Content = new Entry
            {
                Placeholder = placeholder,
                TextColor = Colors.Black,
                PlaceholderColor = Colors.Gray,
                Keyboard = keyboard ?? Keyboard.Default,
                IsPassword = isPassword
            }
            .Bind(Entry.TextProperty, bindingPath)
        };
    }
}
