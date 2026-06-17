using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;

namespace MyStudentsApp.MVVM.Views;

public class DashboardView : ContentPage
{
    private readonly DashboardViewModel _viewModel;

    public DashboardView(DashboardViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Title = "Inicio";
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
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star)
            },
            Children =
            {
                new Label
                {
                    FontSize = 26,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#17202A")
                }.Bind(Label.TextProperty, nameof(DashboardViewModel.Titulo)).Row(0),

                new Border
                {
                    Padding = new Thickness(10),
                    Stroke = Colors.White,
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                    BackgroundColor = Color.FromArgb("#163848"),
                    Content = new ScrollView
                    {
                        Content = new VerticalStackLayout
                        {
                            Padding = new Thickness(8),
                            Spacing = 12,
                            Children =
                            {
                                new Button
                                {
                                    Text = "Actualizar",
                                    BackgroundColor = Color.FromArgb("#48c1ec"),
                                    TextColor = Colors.White,
                                    CornerRadius = 8,
                                    HeightRequest = 44
                                }.Bind(Button.CommandProperty, nameof(DashboardViewModel.CargarCommand)),

                                new Grid
                                {
                                    ColumnSpacing = 10,
                                    RowSpacing = 10,
                                    ColumnDefinitions =
                                    {
                                        new ColumnDefinition(GridLength.Star),
                                        new ColumnDefinition(GridLength.Star)
                                    },
                                    RowDefinitions =
                                    {
                                        new RowDefinition(GridLength.Auto),
                                        new RowDefinition(GridLength.Auto),
                                        new RowDefinition(GridLength.Auto)
                                    },
                                    Children =
                                    {
                                        MetricCard("Mensajes", nameof(DashboardViewModel.MensajesSinLeer)).Row(0).Column(0),
                                        MetricCard("Tareas", nameof(DashboardViewModel.TareasPendientes)).Row(0).Column(1),
                                        MetricCard("Por calificar", nameof(DashboardViewModel.EntregasPorCalificar)).Row(1).Column(0),
                                        MetricCard("Vencen pronto", nameof(DashboardViewModel.ProximosVencimientos)).Row(1).Column(1),
                                        MetricCard("Estudiantes", nameof(DashboardViewModel.EstudiantesActivos)).Row(2).Column(0),
                                        MetricCard("Calificadas", nameof(DashboardViewModel.EntregasCalificadas)).Row(2).Column(1)
                                    }
                                },

                                new Label
                                {
                                    Text = "Actividad reciente",
                                    FontSize = 18,
                                    FontAttributes = FontAttributes.Bold,
                                    TextColor = Color.FromArgb("#17202A"),
                                    Margin = new Thickness(0, 8, 0, 0)
                                },

                                new CollectionView
                                {
                                    SelectionMode = SelectionMode.None,
                                    ItemTemplate = new DataTemplate(() => new Border
                                    {
                                        Padding = 12,
                                        Margin = new Thickness(0, 4),
                                        Stroke = Color.FromArgb("#E9ECEF"),
                                        BackgroundColor = Colors.White,
                                        StrokeShape = new RoundRectangle { CornerRadius = 10 },
                                        Content = new Label
                                        {
                                            FontSize = 14,
                                            TextColor = Color.FromArgb("#2C3E50")
                                        }.Bind(Label.TextProperty, ".")
                                    })
                                }.Bind(CollectionView.ItemsSourceProperty, nameof(DashboardViewModel.Alertas)),

                                new Label
                                {
                                    TextColor = Colors.DarkRed,
                                    FontSize = 13
                                }.Bind(Label.TextProperty, nameof(DashboardViewModel.ErrorMessage))
                            }
                        }
                    }
                }.Row(1)
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    private static Border MetricCard(string title, string bindingPath)
    {
        return new Border
        {
            Padding = 14,
            MinimumHeightRequest = 96,
            Stroke = Color.FromArgb("#E9ECEF"),
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Content = new VerticalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    new Label
                    {
                        Text = title,
                        FontSize = 13,
                        TextColor = Color.FromArgb("#566573")
                    },
                    new Label
                    {
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#17202A")
                    }.Bind(Label.TextProperty, bindingPath)
                }
            }
        };
    }
}
