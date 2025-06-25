using Acento.MVVM.Models.Estudents;
using Acento.MVVM.Models.SKIA;
using Acento.MVVM.ViewModels;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;

namespace Acento.MVVM.Views;

public class ListaDeEstudiantes : ContentPage
{
	public ListaDeEstudiantes()
	{
        BindingContext = new ListadoDeEstudiantesViewModel();

        ToolbarItems.Add(new ToolbarItem
        {
            IconImageSource = "ios_menu.png",
            Order = ToolbarItemOrder.Primary,
            Command = new Command(async () =>
            {
                string opcion = await Application.Current.MainPage.DisplayActionSheet(
                    "MENU", "Cerrar", null, "Perfil", "Configuración", "Cerrar sesión");

                switch (opcion)
                {
                    case "Perfil":
                        //await Shell.Current.GoToAsync("perfil"); // o navega a una página
                        break;

                    case "Configuración":
                        //await Shell.Current.GoToAsync("configuracion");
                        break;

                    case "Cerrar sesión":
                        bool confirmar = await Application.Current.MainPage.DisplayAlert(
                            "¿Estás seguro?", "¿Deseas cerrar sesión?", "Sí", "No");
                        if (confirmar)
                        {
                            // Lógica de logout
                            //await Shell.Current.GoToAsync("//login");
                        }
                        break;

                    default:
                        // Usuario tocó "Cerrar" o canceló
                        break;
                }
            })
        });

    }

	protected override void OnAppearing()
	{
		base.OnAppearing();

		Content = new Grid
		{
			Padding = new Thickness(10),
            RowSpacing = 10,
            Background = new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
                        {
                            new GradientStop { Color = Color.FromArgb("#febb5a"), Offset = 0.0f },
                            new GradientStop { Color = Color.FromArgb("#48c1ec"), Offset = 0.2f },
                            new GradientStop { Color = Color.FromArgb("##163848"), Offset = 1.0f },
                        }
            },
			ColumnDefinitions = new ColumnDefinitionCollection
			{
				new ColumnDefinition { Width = GridLength.Star }
			},
			RowDefinitions = new RowDefinitionCollection
			{
				new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star)},
				new RowDefinition { Height = new GridLength(0.8, GridUnitType.Star) }
            },

			Children =
			{

				new Border
				{
                    BackgroundColor = Color.FromArgb("#000000"),
					StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
					Stroke = new SolidColorBrush(Colors.White),
                    Content = new Grid
                    {
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) },
                            new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) }
                        },
                        Children =
                        {
                            new Label
                            {
                                Text = "Lista de Estudiantes",
                                FontSize = 24,
                                TextColor = Color.FromArgb("#ffffff"),
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center
                            }.Row(0),
                            new SearchBar
                            {
                                PlaceholderColor = Color.FromArgb("#48c1ec"),
                                Placeholder = "Buscar Estudiante",
                                BackgroundColor = Color.FromArgb("#000000"),
                                TextColor = Color.FromArgb("#cae166"),
                                HorizontalOptions = LayoutOptions.Fill,
                                VerticalOptions = LayoutOptions.Center
                            }.Row(1)
                        }
                    }
                }
				.Center()
                .Fill()
				.Row(0).Column(0)/*.Bind(Entry.TextProperty, "SearchQuery")*/,

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
                            Command = new Command((student) =>
                            {
                               navegarChatEstudiante(student);
                            })
                        }
                        .Bind(SwipeItem.CommandParameterProperty, ".")
                        ;

                        var border = new Border
                        {
                            Margin = new Thickness(4),
                            MinimumHeightRequest = 100,
                            Padding = new Thickness(5),
                            Background = new LinearGradientBrush
                            {
                                GradientStops = new GradientStopCollection
                                        {
                                            new GradientStop { Color = Color.FromArgb("#d6cc8e"), Offset = 0.0f },
                                            new GradientStop { Color = Color.FromArgb("#d6cc8e"), Offset = 1.0f },
                                        }
                            },
                            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                            Stroke = new SolidColorBrush(Color.FromArgb("#48c1ec")),
                            StrokeLineJoin = PenLineJoin.Round,
                            StrokeThickness = 1,
                            Content = new Grid
                            {
                                Padding = new Thickness(10, 0, 10, 3),
                                RowDefinitions = new RowDefinitionCollection
                                {
                                    new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star)  },
                                    new RowDefinition { Height = new GridLength(0.6, GridUnitType.Star) },
                                    new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star)  },
                                },
                                ColumnDefinitions = new ColumnDefinitionCollection
                                {
                                    new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) },
                                    new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) },
                                    new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) },

                                },
                                Children =
                                {
                                     
                                    new Label
                                    {
                                        FontSize = 24,
                                        TextColor = Colors.White,
                                        HorizontalTextAlignment = TextAlignment.Center,
                                        HorizontalOptions = LayoutOptions.Start,
                                        VerticalOptions = LayoutOptions.Center,
                                        VerticalTextAlignment = TextAlignment.Center

                                    }.Bind(Label.TextProperty, "Name").Row(1).Column(0).ColumnSpan(2),

                                     new Label
                                    {
                                        Text = "Clase A",
                                        FontSize = 10,
                                        TextColor = Colors.White,
                                        HorizontalTextAlignment = TextAlignment.End,
                                    }.Row(2).Column(2),

                                     new AlertaLuzParpadeanteControlSkia
                                     {
                                         colorLuzProperty = SKColors.Blue,
                                         debeParpadearProperty = true,
                                         HorizontalOptions = LayoutOptions.End,  
                                     }.Row(0).Column(3).Height(10).Width(10),


                                }
                            }
                        };

                            swipeView.RightItems = new SwipeItems { ChatSwipeItem, infoSwipeItem };
                            //swipeView.LeftItems = new SwipeItems { infoSwipeItem };

                        swipeView.Content = border;
                        return swipeView;
                    }),
                    BackgroundColor = Colors.Transparent
                }.Bind(CollectionView.ItemsSourceProperty, "Estudiantes"),
					Stroke = new SolidColorBrush(Color.FromArgb("#ffffff")),
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Fill,
					StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
					BackgroundColor = Color.FromArgb("#163848"),

                }
                .Row(0).Column(2)
				.Row(1),


            }
		};
    }

    private void navegarChatEstudiante(object student)
    {
        Student estudiante = (Student)student;
        var page = new StudentsChatView();

        NavigationPage.SetTitleView(page,
           new Grid
           {
               Padding = new Thickness(0, 0, 15, 0),
               ColumnSpacing = 1,
               RowDefinitions = new RowDefinitionCollection
{
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star)  },
},
               ColumnDefinitions = new ColumnDefinitionCollection
{
                            new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) },
},

               Children =
{
                            new CirculoAvatar
                            {
                                NombreUsuario = estudiante.Name
                            }
//#if !WINDOWS
							.Width(35)
                            .Height(35)
//#endif
							.Column(1).Row(0),

                            new Label
                            {
                                Text = estudiante.Name,
                                TextColor = Colors.Black,
                                FontSize = 20,
                                Margin = new Thickness(2, 0, 0, 0),
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Start
                            }
                            .Column(2).Row(0),


                            new ImageButton
                            {
#if IOS
								Source = "ios_menu"
#elif ANDROID
								Source = "ios_menu",
                                WidthRequest = 30,
                                HeightRequest = 30,

#elif WINDOWS
								Source = "windows_menu.png"
#else
								Source = "ios_menu"
#endif
                            }
                            .Column(3).Row(0),
}
           });
        Navigation.PushAsync(page);
    }

    private void OnBackButtonClicked(object obj)
    {
        // Implement navigation logic here
        Navigation.PopAsync();
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Implement navigation logic here
        Navigation.PopAsync();
    }

}