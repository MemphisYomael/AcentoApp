using CommunityToolkit.Maui.Markup;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.Models.ContentViews;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Shared.DTOShared;
using SkiaSharp;

namespace MyStudentsApp.MVVM.Views;

public class ListadoEstudiantesProfesores : ContentPage
{
    SearchBar searchBar = new SearchBar
    {
        //PlaceholderColor = Color.FromArgb("#48c1ec"),
        Placeholder = "Buscar Estudiante",
        PlaceholderColor = Colors.White,
        //BackgroundColor = Color.FromArgb("#ffffff"),
        //TextColor = Color.FromArgb("#cae166"),
        TextColor = Colors.Black,
        HorizontalOptions = LayoutOptions.Fill,
        VerticalOptions = LayoutOptions.Center,
    };

    public ListadoEstudiantesProfesores(ListadoDeEstudiantesViewModel viewModel)
	{
        Title = "Listado de Estudiantes";
        searchBar.TextChanged += (sender, e) =>
        {
            if (BindingContext is ListadoDeEstudiantesViewModel viewModel)
            {
                viewModel.buscar.Execute(e.NewTextValue);
            }
        };
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
                    //Stroke = new SolidColorBrush(Colors.White),
                    Content = new Grid
                    {
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        },
                        Children =
                        {
                            //new Label
                            //{

                            //    FontSize = 24,
                            //    TextColor = Color.FromArgb("#000000"),
                            //    HorizontalOptions = LayoutOptions.Center,
                            //    VerticalOptions = LayoutOptions.Center
                            //}.Row(0)
                            //.Font("text")
                            //.Bind(Label.TextProperty, "titulo")
                            //,
                            
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
							//BackgroundColor = Colors.Transparent,
							CommandParameter = new Binding("."),
                        };

                        var ChatSwipeItem = new SwipeItem
                        {
                            IconImageSource = "chat.png",
                            //BackgroundColor = Color.FromArgb("#294f52"),
                            Command = new Command(async (object student) =>
                            {
                                if (student is EstudianteResponseDTO studentDto)
                                {
                                    // Usar navegación Shell en lugar de Navigation.PushAsync
                                    await Shell.Current.GoToAsync("chatZone", 
                                        new Dictionary<string, object>
                                        {
                                            ["nombre"] = studentDto.nombres + " " + studentDto.apellidos,
                                            ["usuarioId"] = studentDto.usuarioId!
                                        });
                                }
                            }),
                            CommandParameter = new Binding("."),

                        }
                        .Bind(SwipeItem.CommandParameterProperty, ".")
                        ;


                        TarjetaUsuarioControl border = new TarjetaUsuarioControl
                        {
                            AvatarColor = Colors.DarkOrange,
                            TextColor = Colors.Black,
                            //BackgroundColor = Color.FromArgb("d6cc8e"),
                            BackgroundColor = Colors.White,
                            Margin = new Thickness(0,10,0,0)
                        }
                        .Bind(TarjetaUsuarioControl.UserPositionProperty, "curso")
                        .Bind(
                                TarjetaUsuarioControl.UserNameProperty,
                                ".",
                                convert: (EstudianteResponseDTO? e) => e != null ? $"{e.nombres} {e.apellidos}" : string.Empty
                            );

                            swipeView.RightItems = new SwipeItems { ChatSwipeItem, infoSwipeItem };
                            //swipeView.LeftItems = new SwipeItems { infoSwipeItem };

                        swipeView.Content = border;
                        return swipeView;
                    }),
                    BackgroundColor = Colors.Transparent
                }.Bind(CollectionView.ItemsSourceProperty, "EstudiantesFiltrados"),
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
        if (BindingContext is ListadoDeEstudiantesViewModel viewModel)
        {

            viewModel.buscar = new Command((SearchText) =>
            {
                string busquedaTexto = (string)SearchText;

                if (string.IsNullOrEmpty(busquedaTexto))
                {
                    viewModel.EstudiantesFiltrados = viewModel.Estudiantes;
                    return;
                }
                try
                {
                    var listaFiltrada = viewModel.Estudiantes
                     .Where(x =>
                         (x?.nombres ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                         (x?.apellidos ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                         (x?.curso ?? string.Empty).Contains(busquedaTexto, StringComparison.OrdinalIgnoreCase)
                     )
                     .ToList();


                    viewModel.EstudiantesFiltrados = listaFiltrada;
                }
                catch (Exception ex)
                {
                    viewModel.EstudiantesFiltrados = new List<EstudianteResponseDTO>();
                }
            });
            viewModel.titulo = "Listado de estudiantes";

            viewModel.ObtenerEstudiantes();

            viewModel.EstudiantesFiltrados = viewModel.Estudiantes;
            // Lógica adicional si es necesaria
        }
        base.OnAppearing();
    }
}
