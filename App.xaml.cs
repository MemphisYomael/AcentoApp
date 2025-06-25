using Acento.MVVM.Views;
using CommunityToolkit.Maui.Markup;

namespace Acento
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var estudiantesPage = new ListaDeEstudiantes();

            NavigationPage.SetTitleView(estudiantesPage, new Grid
            {
                Padding = new Thickness(10, 0, 0, 0),
                ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Auto },   // Para el logo
                            new ColumnDefinition { Width = GridLength.Star }    // Para el título (o vacío)
                        },
                Children =
                        {
                            new Image
                            {
                                Source = "acento.png",
                                HeightRequest = 32,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Start
                            }
                            .Assign<Image, Image>(out var logo), // Especificar los argumentos de tipo explícitamente

                            new Label
                            {
                                Text = "cento",
                                VerticalOptions = LayoutOptions.Center,
                                Margin = new Thickness(-7, 0,0, 2),
                                FontSize = 22,
                                TextColor = Color.FromArgb("#000000"),

                            }
                            //.Bold()
                            .Column(1)
                        }
            });

            var nav = new NavigationPage(estudiantesPage)
            {
                BarBackground = Color.FromArgb("#ffffff"),
                BarTextColor = Color.FromArgb("#fdbd59")
            };

            return new Window(nav);
        }
    }
}
