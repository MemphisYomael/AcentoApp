using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Shared.DTOShared;

namespace MyStudentsApp.MVVM.Views;

[QueryProperty(nameof(Nombre), "nombre")]
[QueryProperty(nameof(UsuarioId), "usuarioId")]
public partial class ChatZoneView : ContentPage
{
    private string _nombrePersona;
    private string _usuarioId;
    private ChatZoneViewModel _viewModel;
    private Label _headerLabel;

    public string Nombre
    {
        get => _nombrePersona;
        set
        {
            _nombrePersona = value;
            Title = _nombrePersona;
            if (_headerLabel != null)
                _headerLabel.Text = _nombrePersona;
            OnPropertyChanged();
        }
    }

    public string UsuarioId
    {
        get => _usuarioId;
        set
        {
            _usuarioId = value;
            OnPropertyChanged();
        }
    }

    public ICommand EnviarMensajeCommand { get; set; }

    public ChatZoneView(ChatZoneViewModel zoneViewModel)
    {
        _viewModel = zoneViewModel;
        BindingContext = _viewModel;
        EnviarMensajeCommand = new Command(async () => await Enviar());

        Background = new LinearGradientBrush
        {
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb("#febb5a"), Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#48c1ec"), Offset = 0.2f },
                new GradientStop { Color = Color.FromArgb("#163848"), Offset = 1.0f },
            }
        };
        InitializeComponent();

        _headerLabel = new Label
        {
            Text = _nombrePersona ?? "Chat",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
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
                new RowDefinition(GridLength.Auto), // Barra superior
                new RowDefinition(new GridLength(1, GridUnitType.Star)), // Mensajes
                new RowDefinition(GridLength.Auto) // Caja de texto
            },
            Children =
            {
                // Barra superior con nombre
                new Border
                {
                    BackgroundColor = Colors.White,
                    Padding = 12,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(10)
                    },
                    Content = new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition(GridLength.Auto),
                            new ColumnDefinition(GridLength.Star)
                        },
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition(GridLength.Auto),
                            new RowDefinition(GridLength.Auto)
                        },
                        Children =
                        {
                            new Button
                            {
                                Text = "‹",
                                FontSize = 22,
                                WidthRequest = 42,
                                HeightRequest = 38,
                                Padding = 0,
                                BackgroundColor = Color.FromArgb("#EAF2F8"),
                                TextColor = Colors.Black,
                                CornerRadius = 8,
                                Command = new Command(async () => await Shell.Current.GoToAsync(".."))
                            }.Row(0).Column(0).RowSpan(2),
                            _headerLabel.Row(0).Column(1),
                            new Label
                            {
                                FontSize = 11,
                                TextColor = Color.FromArgb("#1F7A8C"),
                                HorizontalOptions = LayoutOptions.Center
                            }.Bind(Label.TextProperty, nameof(ChatZoneViewModel.StatusMessage))
                             .Row(1).Column(1)
                        }
                    }
                }.Row(0),

                // CollectionView para mensajes
                new CollectionView
                {
                    EmptyView = new Label
                    {
                        Text = "Aún no hay mensajes.",
                        TextColor = Colors.White,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    },
                    ItemTemplate = new DataTemplate(() =>
                    {
                        var border = new Border()
                        {
                            Padding = 10,
                            Margin = new Thickness(5),
                            Stroke = Colors.Transparent,
                            StrokeThickness = 1,
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(10)
                            },
                        };
                        border.SetBinding(Border.BindingContextProperty, ".");

                        border.Bind(Border.BackgroundColorProperty, "recipientUsuarioId", convert: (string usuarioId) =>
                        {
                            if (usuarioId != null && usuarioId == _usuarioId) return Color.FromArgb("#DCF8C6");
                            return Colors.White;
                        });
                        border.Content = renderizarMensage();
                        return border;
                    })

                }.Bind(CollectionView.ItemsSourceProperty, "Mensajes")
                .Row(1),

                // Caja de texto para escribir mensajes
                new Border
                {
                    BackgroundColor = Colors.White,
                    Padding = 10,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(25)
                    },
                    Content = new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                            new ColumnDefinition(GridLength.Auto)
                        },
                        Children =
                        {
                            new Entry
                            {
                                Placeholder = "Escribe un mensaje...",
                                BackgroundColor = Colors.Transparent,
                                TextColor = Colors.Black,
                                PlaceholderColor = Colors.Gray,
                                FontSize = 16
                            }.Bind(Entry.TextProperty, "textoEscritoInput")
                             .Column(0),
                            new Button
                            {
                                Text = "Enviar",
                                BackgroundColor = Color.FromArgb("#48c1ec"),
                                TextColor = Colors.White,
                                CornerRadius = 20,
                                Padding = new Thickness(20, 10),
                                FontSize = 14,
                                Command = EnviarMensajeCommand
                            }
                             .Column(1)
                        }
                    }
                }.Row(2)
            }
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (!string.IsNullOrEmpty(_usuarioId))
        {
            _viewModel?.TraerMensajesAsync(_usuarioId);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // IMPORTANTE: Limpiar eventos cuando se sale del chat
        _viewModel?.Cleanup();
    }

    private async Task Enviar()
    {
        if (!string.IsNullOrEmpty(_usuarioId))
        {
            await _viewModel.EnviarMensajeAsync(_usuarioId);
        }
    }

    private View renderizarMensage()
    {
        return new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star)
            },
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            },
            Children =
            {
                new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.Black
                }.Bind(Label.TextProperty, "senderNombre")
                 .Row(0).Column(0),
                new Label
                {
                    TextColor = Colors.Black
                }.Bind(Label.TextProperty, "mensaje")
                .Bind(Label.HorizontalOptionsProperty, "senderNombre", convert: (string nombre) =>
                 {
                            if (nombre != null && !string.IsNullOrEmpty(_nombrePersona) && _nombrePersona.Contains(nombre)) return LayoutOptions.End;
                            return LayoutOptions.Start;
                 })
                 .Row(1).Column(0).ColumnSpan(2),
                new Label
                {
                    FontSize = 10,
                    TextColor = Colors.Gray
                }.Bind(Label.TextProperty, "enviado", BindingMode.OneWay, stringFormat: "{0:g}")
                 .Row(0).Column(1).End()
            }
        };
    }
}
