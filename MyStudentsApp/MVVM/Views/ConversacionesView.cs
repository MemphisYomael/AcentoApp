using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Shared.DTOShared;

namespace MyStudentsApp.MVVM.Views;

public class ConversacionesView : ContentPage
{
    private readonly ConversacionesViewModel _viewModel;

    public ConversacionesView(ConversacionesViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Title = "Conversaciones";
        Background = new LinearGradientBrush
        {
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb("#febb5a"), Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#48c1ec"), Offset = 0.2f },
                new GradientStop { Color = Color.FromArgb("#163848"), Offset = 1.0f },
            }
        };

        var searchBar = new SearchBar
        {
            Placeholder = "Buscar conversación",
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#17202A"),
            PlaceholderColor = Color.FromArgb("#85929E")
        };
        searchBar.TextChanged += (_, e) => _viewModel.BuscarCommand.Execute(e.NewTextValue);

        var collection = new CollectionView
        {
            SelectionMode = SelectionMode.Single,
            EmptyView = new Label
            {
                Text = "No hay conversaciones todavía.",
                TextColor = Color.FromArgb("#EAF2F8"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            },
            ItemTemplate = new DataTemplate(CreateConversationTemplate)
        }.Bind(CollectionView.ItemsSourceProperty, nameof(ConversacionesViewModel.Conversaciones));

        collection.SelectionChanged += async (_, e) =>
        {
            var conversacion = e.CurrentSelection.FirstOrDefault() as ConversacionResponseDTO;
            collection.SelectedItem = null;
            if (conversacion?.otherUserId == null) return;

            await Shell.Current.GoToAsync("chatZone", new Dictionary<string, object>
            {
                ["nombre"] = conversacion.otherUserName ?? "Chat",
                ["usuarioId"] = conversacion.otherUserId
            });
        };

        Content = new Grid
        {
            Padding = 10,
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
                    Text = "Bandeja",
                    FontSize = 26,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#17202A")
                }.Row(0),

                new Border
                {
                    Padding = new Thickness(10),
                    Stroke = Colors.White,
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                    BackgroundColor = Color.FromArgb("#163848"),
                    Content = new Grid
                    {
                        Padding = new Thickness(8),
                        RowSpacing = 12,
                        RowDefinitions =
                        {
                            new RowDefinition(GridLength.Auto),
                            new RowDefinition(GridLength.Star),
                            new RowDefinition(GridLength.Auto)
                        },
                        Children =
                        {
                            new Border
                            {
                                Padding = new Thickness(12, 2),
                                Stroke = Color.FromArgb("#BFCAD4"),
                                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                                BackgroundColor = Colors.White,
                                Content = searchBar
                            }.Row(0),
                            collection.Row(1),
                            new Label
                            {
                                TextColor = Color.FromArgb("#EAF2F8"),
                                FontSize = 13
                            }.Bind(Label.TextProperty, nameof(ConversacionesViewModel.ErrorMessage)).Row(2)
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

    private static View CreateConversationTemplate()
    {
        var unreadBadge = new Border
        {
            Padding = new Thickness(8, 2),
            Stroke = Colors.Transparent,
            BackgroundColor = Color.FromArgb("#D64045"),
            HorizontalOptions = LayoutOptions.End,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Content = new Label
            {
                FontSize = 12,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold
            }.Bind(Label.TextProperty, nameof(ConversacionResponseDTO.unreadCount))
        }.Bind(Border.IsVisibleProperty, nameof(ConversacionResponseDTO.unreadCount), convert: (int count) => count > 0);

        return new Border
        {
            Padding = 14,
            Margin = new Thickness(0, 5),
            Stroke = Color.FromArgb("#E9ECEF"),
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Content = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                },
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto)
                },
                Children =
                {
                    new Label
                    {
                        FontSize = 17,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#17202A")
                    }.Bind(Label.TextProperty, nameof(ConversacionResponseDTO.otherUserName)).Row(0).Column(0),
                    unreadBadge.Row(0).Column(1),
                    new Label
                    {
                        FontSize = 12,
                        TextColor = Color.FromArgb("#1F7A8C")
                    }.Bind(Label.TextProperty, nameof(ConversacionResponseDTO.otherUserRole)).Row(1).Column(0),
                    new Label
                    {
                        FontSize = 14,
                        TextColor = Color.FromArgb("#566573"),
                        LineBreakMode = LineBreakMode.TailTruncation
                    }.Bind(Label.TextProperty, nameof(ConversacionResponseDTO.lastMessage)).Row(2).Column(0),
                    new Label
                    {
                        FontSize = 11,
                        TextColor = Color.FromArgb("#85929E")
                    }.Bind(Label.TextProperty, nameof(ConversacionResponseDTO.lastMessageAt), stringFormat: "{0:g}").Row(2).Column(1)
                }
            }
        };
    }
}
