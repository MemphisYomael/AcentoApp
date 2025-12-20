using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.Models.ContentViews;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Shared.DTOShared;

namespace MyStudentsApp.MVVM.Views;

public class GestionProfesoresView : ContentPage
{
    private readonly GestionProfesoresViewModel _viewModel;
    
    public GestionProfesoresView(GestionProfesoresViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
        Title = "Gestión de Profesores";
        
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
                                Placeholder = "Buscar profesor...",
                                PlaceholderColor = Colors.White,
                                TextColor = Colors.Black,
                                HorizontalOptions = LayoutOptions.Fill,
                                VerticalOptions = LayoutOptions.Center
                            }
                            .Column(0)
                            .Bind(SearchBar.TextProperty, nameof(GestionProfesoresViewModel.BuscarCommand), mode: BindingMode.OneWayToSource),
                            

                        }
                    }
                }
                .Row(0),

                // Lista de profesores o formulario
                new Grid
                {
                    Children =
                    {
                        // Lista de profesores
                        CrearListaProfesores()
                            .Bind(VisualElement.IsVisibleProperty, nameof(GestionProfesoresViewModel.MostrarFormularioEdicion), 
                                  convert: (bool mostrar) => !mostrar),
                        
                        // Formulario de edición
                        CrearFormularioEdicion()
                            .Bind(VisualElement.IsVisibleProperty, nameof(GestionProfesoresViewModel.MostrarFormularioEdicion))
                    }
                }
                .Row(1)
            }
        };
    }

    private Border CrearListaProfesores()
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
                    .Bind(SwipeItem.CommandProperty, nameof(GestionProfesoresViewModel.EditarCommand), source: _viewModel)
                    .Bind(SwipeItem.CommandParameterProperty, ".");

                    var deleteSwipeItem = new SwipeItem
                    {
                        Text = "Eliminar",
                        BackgroundColor = Colors.Red,
                        IconImageSource = "delete.png"
                    }
                    .Bind(SwipeItem.CommandProperty, nameof(GestionProfesoresViewModel.EliminarCommand), source: _viewModel)
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
                                        convert: (ProfesorResponseDTO? p) => 
                                            p != null && !string.IsNullOrEmpty(p.nombres) 
                                                ? p.nombres.Substring(0, 1).ToUpper() 
                                                : "P")
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
                                    convert: (ProfesorResponseDTO? p) => 
                                        p != null ? $"{p.nombres} {p.apellidos}" : string.Empty),

                                new Label
                                {
                                    FontSize = 14,
                                    TextColor = Colors.Gray
                                }
                                .Row(1).Column(1)
                                .Bind(Label.TextProperty, nameof(ProfesorResponseDTO.email)),

                                new Label
                                {
                                    FontSize = 12,
                                    TextColor = Colors.DarkOrange,
                                    FontAttributes = FontAttributes.Bold
                                }
                                .Row(2).Column(1)
                                .Bind(Label.TextProperty, nameof(ProfesorResponseDTO.isAdministrativo),
                                    convert: (bool esAdmin) => esAdmin ? "🔑 Administrativo" : "👨‍🏫 Profesor")
                            }
                        }
                    };

                    swipeView.RightItems = new SwipeItems { editSwipeItem, deleteSwipeItem };
                    swipeView.Content = tarjeta;
                    return swipeView;
                })
            }
            .Bind(CollectionView.ItemsSourceProperty, nameof(GestionProfesoresViewModel.ProfesoresFiltrados))
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
                        .Bind(Label.TextProperty, nameof(GestionProfesoresViewModel.EsNuevoProfesor),
                            convert: (bool esNuevo) => esNuevo ? "Nuevo Profesor" : "Editar Profesor"),

                        CrearCampo("Nombres *", nameof(GestionProfesoresViewModel.Nombres)),
                        CrearCampo("Apellidos", nameof(GestionProfesoresViewModel.Apellidos)),
                        CrearCampo("Email *", nameof(GestionProfesoresViewModel.Email), Keyboard.Email),
                        CrearCampo("Teléfono", nameof(GestionProfesoresViewModel.Telefono), Keyboard.Telephone),
                        CrearCampo("Contraseña", nameof(GestionProfesoresViewModel.Contrasena), isPassword: true),

                        new Frame
                        {
                            BackgroundColor = Colors.White,
                            CornerRadius = 10,
                            Padding = new Thickness(15, 10),
                            Content = new Grid
                            {
                                ColumnDefinitions = new ColumnDefinitionCollection
                                {
                                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                                    new ColumnDefinition(GridLength.Auto)
                                },
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "Es Administrativo",
                                        TextColor = Colors.Black,
                                        VerticalOptions = LayoutOptions.Center
                                    }
                                    .Column(0),
                                    
                                    new Switch
                                    {
                                        OnColor = Color.FromArgb("#48c1ec"),
                                        VerticalOptions = LayoutOptions.Center
                                    }
                                    .Column(1)
                                    .Bind(Switch.IsToggledProperty, nameof(GestionProfesoresViewModel.IsAdministrativo))
                                }
                            }
                        },

                        new DatePicker
                        {
                            BackgroundColor = Colors.White,
                            TextColor = Colors.Black,
                            Format = "dd/MM/yyyy"
                        }
                        .Bind(DatePicker.DateProperty, nameof(GestionProfesoresViewModel.Cumpleanos)),

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
                                .Bind(Button.CommandProperty, nameof(GestionProfesoresViewModel.CancelarCommand)),

                                new Button
                                {
                                    Text = "Guardar",
                                    BackgroundColor = Color.FromArgb("#48c1ec"),
                                    TextColor = Colors.White,
                                    CornerRadius = 10,
                                    HeightRequest = 50
                                }
                                .Column(1)
                                .Bind(Button.CommandProperty, nameof(GestionProfesoresViewModel.GuardarCommand))
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
