using Acento.MVVM.Models.Estudents;
using Acento.MVVM.Models.SKIA;
using Acento.MVVM.ViewModels;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Controls.Shapes;

namespace Acento.MVVM.Views;

public class StudentsChatView : ContentPage
{
	object itemSeleccionado;

	CollectionView zonaMensajes;
	public StudentsChatView()
	{

		BindingContext = new ChatDeEstudianteViewModel();

        //Microsoft.Maui.Controls.NavigationPage.SetHasNavigationBar(this, false);
        //Shell.SetNavBarIsVisible(this, false);


        Content = new Grid
		{
			BackgroundColor = Colors.Black,
			RowDefinitions = new RowDefinitionCollection
			{
				new RowDefinition { Height = new GridLength(0.83, GridUnitType.Star)  },
				new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star)  },
			},
			ColumnDefinitions = new ColumnDefinitionCollection
			{
				new ColumnDefinition { Width = new GridLength(0.9, GridUnitType.Star) },
				new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) },
			},

			Children =
			{
                new CollectionView
                {
                    SelectionMode = SelectionMode.None,
                    SelectedItem = itemSeleccionado,
                    ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepLastItemInView,
                    BackgroundColor = Colors.Black,
                    Margin = new Thickness(10, 0, 10, 0),
                    ItemTemplate = new DataTemplate(() =>
                    {
                    var swipeView = new Microsoft.Maui.Controls.SwipeView();
                        swipeView.Margin = 3;
					// Swipe Items (Derecha)
					var deleteSwipeItem = new SwipeItem
                    {
                        BackgroundColor = Colors.Black,
                        CommandParameter = new Binding("."),
                        IconImageSource = "more_vert.png"
                    };
					//deleteSwipeItem.SetBinding(SwipeItem.CommandProperty, new Binding("BindingContext.DeleteCommand", source: MyCollectionView));

					var editSwipeItem = new SwipeItem
                    {
                        Text = "Tarea",
						//BackgroundColor = Colors.Green,
						CommandParameter = new Binding("."),
                        IconImageSource = "homework.png"
                    };
					//editSwipeItem.SetBinding(SwipeItem.CommandProperty, new Binding("BindingContext.EditCommand", source: MyCollectionView));

					var swipeItemsR = new SwipeItems {editSwipeItem };
                    var swipeItemsL = new SwipeItems { deleteSwipeItem};

                    swipeItemsR.Mode = SwipeMode.Reveal;
                                            swipeItemsL.Mode = SwipeMode.Reveal;

                    swipeView.RightItems = swipeItemsR;
                        swipeView.LeftItems = swipeItemsL;

                        Label fechaLabel = new Label
                        {
#if !WINDOWS
							FontSize = 8,
#endif
							TextColor = Colors.Gray,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center
                        }
                        .Bind(Label.TextProperty, "CreatedAt", convert: (DateTime value) => value != null ? value.Hour + ":" + value.Minute : "")
                        ;


                    Border mensaje = new Border
                    {
                        BackgroundColor = Colors.LightGray,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                        Stroke = new SolidColorBrush(Colors.White),
                        StrokeThickness = 1,
                        HeightRequest = 80,
                        Content = new Grid{

                            Padding = new Thickness(10),
                            ColumnDefinitions = new ColumnDefinitionCollection
                            {
                                new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) },
                                new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) },
                            },
                            RowDefinitions = new RowDefinitionCollection
                            {
                                new RowDefinition { Height = new GridLength(0.2, GridUnitType.Star)  },
                                new RowDefinition { Height = new GridLength(0.8, GridUnitType.Star)  },
                            },

                            Children = {

                                new CheckBox
                                {
                                    BackgroundColor = Colors.Transparent,
									//IsChecked = "{Binding IsRead}",
									IsEnabled = false, // Deshabilitado para evitar cambios
									//Color = Colors.LightGray, // Color del CheckBox
									//HorizontalOptions = LayoutOptions.Start,
									//VerticalOptions = LayoutOptions.Center
									
									
                                }
                                .Bind(CheckBox.IsCheckedProperty, "IsRead")
                                .Row(0).RowSpan(2)
                                ,


                            new Label
                            {
                                TextColor = Colors.Black,
                                FontSize = 16,
                                Padding = new Thickness(10),
								//HorizontalTextAlignment = i % 2 == 0 ? TextAlignment.Start : TextAlignment.End,

							}
                            .Column(1)
                            .ColumnSpan(2)
                            .Row(1)
                            .Bind(Label.TextProperty, "Message")
                            .Bind<Label, bool, TextAlignment>(
                                Label.HorizontalTextAlignmentProperty,
                                "isStudent",
                                convert: (isStudent) => isStudent ? TextAlignment.Start : TextAlignment.End
                            ),

                            fechaLabel.Column(0).Row(0)

                            }
                        }
                                            }
                    .Bind<Border, bool, HorizontalAlignment>(
                            Label.HorizontalTextAlignmentProperty,
                            "isStudent",
                            convert: (isStudent) => isStudent ? HorizontalAlignment.Left : HorizontalAlignment.Right)
                    .Bind<Border, int, Color>(Border.BackgroundColorProperty, "tareaId", convert: (value) => (int)value > 0 ? Colors.LightGreen : Colors.LightGray)

                                    ;

                    swipeView.Content = mensaje;

                    return swipeView;
                    })
                }
                .Bind(CollectionView.ItemsSourceProperty, "Messages")
                .Column(0).ColumnSpan(3).Row(0),

                new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 1,
                    Content = new Grid
                    {
                        Padding = new Thickness(10),
                        BackgroundColor = Colors.White,
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) },
                        },
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star)  },

                        },


                        Children =
                        {
                            new Microsoft.Maui.Controls.Entry
                            {
                                Placeholder = "Escribe un mensaje...",
                                BackgroundColor = Colors.Transparent,
                                TextColor = Colors.Black,
                                PlaceholderColor = Color.FromArgb("#c9e365"),
                                FontSize = 16
                            }.Column(0)
                            .Row(0)
                            .ColumnSpan(2),

                            new ImageButton
                            {
                                Source = "send.png",
                                BackgroundColor = Colors.Transparent,
                            }
                            .Height(30)
                            .Width(30)
                            .Column(2)
                            .Row(0)
                        }
                    }

                }
                .Column(0)
                .ColumnSpan(3)
                .Row(1),


            }
        };
    }
}