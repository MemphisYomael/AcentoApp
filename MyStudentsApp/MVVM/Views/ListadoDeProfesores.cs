using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.Models.ContentViews;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Shared.DTOShared;

namespace MyStudentsApp.MVVM.Views;

public class ListadoDeProfesores : ContentPage
{
    SearchBar searchBar = new SearchBar
    {
        Placeholder = "Buscar Profesor",
        PlaceholderColor = Colors.White,
        TextColor = Colors.Black,
        HorizontalOptions = LayoutOptions.Fill,
        VerticalOptions = LayoutOptions.Center,
    };

    public ListadoDeProfesores(ListadoDeProfesoresViewModel viewModel)
    {
        Title = "Mis Profesores";
        searchBar.TextChanged += (sender, e) =>
        {
            if (BindingContext is ListadoDeProfesoresViewModel vm)
            {
                vm.buscar.Execute(e.NewTextValue);
            }
        };
        BindingContext = viewModel;
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
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition()
            },
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(new GridLength(0.1, GridUnitType.Star)),
                new RowDefinition(new GridLength(0.9, GridUnitType.Star))
            },

            Children =
            {
                new Border
                {
                    BackgroundColor = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                    Content = new Grid
                    {
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        },
                        Children =
                        {
                            searchBar
                            .Row(0)
                            .Font("text")
                        }
                    }
                }
                .Center()
                .Fill()
                .Row(0).Column(0),

                new Border
                {
                    Padding = new Thickness(10),
                    Content = new CollectionView
                    {
                        ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                        VerticalOptions = LayoutOptions.Start,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                        ItemTemplate = new DataTemplate(() =>
                        {
                            var swipeView = new SwipeView();

                            var infoSwipeItem = new SwipeItem
                            {
                                IconImageSource = "info_ios_blanco.png",
                                CommandParameter = new Binding("."),
                            };

                            var ChatSwipeItem = new SwipeItem
                            {
                                IconImageSource = "chat.png",
                                Command = new Command(async (object profesor) =>
                                {
                                    if (profesor is ProfesorResponseDTO profesorDto)
                                    {
                                        // Usar navegación Shell en lugar de Navigation.PushAsync
                                        await Shell.Current.GoToAsync("chatZone", 
                                            new Dictionary<string, object>
                                            {
                                                ["nombre"] = profesorDto.nombres + " " + profesorDto.apellidos,
                                                ["usuarioId"] = profesorDto.usuarioId!
                                            });
                                    }
                                }),
                                CommandParameter = new Binding("."),
                            }
                            .Bind(SwipeItem.CommandParameterProperty, ".");

                            TarjetaUsuarioControl border = new TarjetaUsuarioControl
                            {
                                AvatarColor = Colors.DarkBlue,
                                TextColor = Colors.Black,
                                BackgroundColor = Colors.White,
                                Margin = new Thickness(0, 10, 0, 0)
                            }
                            .Bind(TarjetaUsuarioControl.UserPositionProperty, ".", 
                                convert: (ProfesorResponseDTO? p) => p != null && p.isAdministrativo ? "Administrativo 🔑" : "Profesor 👨‍🏫")
                            .Bind(
                                TarjetaUsuarioControl.UserNameProperty,
                                ".",
                                convert: (ProfesorResponseDTO? p) => p != null ? $"{p.nombres} {p.apellidos}" : string.Empty
                            );

                            swipeView.RightItems = new SwipeItems { ChatSwipeItem, infoSwipeItem };

                            swipeView.Content = border;
                            return swipeView;
                        }),
                        BackgroundColor = Colors.Transparent
                    }.Bind(CollectionView.ItemsSourceProperty, "ProfesoresFiltrados"),
                    Stroke = new SolidColorBrush(Color.FromArgb("#ffffff")),
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                    BackgroundColor = Color.FromArgb("#163848"),
                }
                .Row(0).Column(0)
                .Row(1),
            }
        };
    }

    protected override void OnAppearing()
    {
        if (BindingContext is ListadoDeProfesoresViewModel viewModel)
        {
            viewModel.buscar = new Command((SearchText) =>
            {
                string busquedaTexto = (string)SearchText;

                if (string.IsNullOrEmpty(busquedaTexto))
                {
                    viewModel.ProfesoresFiltrados = viewModel.Profesores;
                    return;
                }
                try
                {
                    var listaFiltrada = viewModel.Profesores
                     .Where(p =>
                         (p?.nombres ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                         (p?.apellidos ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                         (p?.email ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase)
                     )
                     .ToList();

                    viewModel.ProfesoresFiltrados = listaFiltrada;
                }
                catch (Exception ex)
                {
                    viewModel.ProfesoresFiltrados = new List<ProfesorResponseDTO>();
                }
            });
            
            viewModel.titulo = "Mis Profesores";
            viewModel.ObtenerProfesores();
            viewModel.ProfesoresFiltrados = viewModel.Profesores;
        }
        base.OnAppearing();
    }
}
