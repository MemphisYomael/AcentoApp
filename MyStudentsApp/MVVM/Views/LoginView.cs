using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;

namespace MyStudentsApp.MVVM.Views;

public class LoginView : ContentPage
{
	public LoginView(LoginViewModel loginViewModel)
	{
        Background = Background = new LinearGradientBrush
        {
            GradientStops = new GradientStopCollection
                        {
                            new GradientStop { Color = Color.FromArgb("#febb5a"), Offset = 0.0f },
                            new GradientStop { Color = Color.FromArgb("#48c1ec"), Offset = 0.2f },
                            new GradientStop { Color = Color.FromArgb("#163848"), Offset = 1.0f },
                        }
        };
        BindingContext = loginViewModel;
		Title = "Iniciar Sesión";
        Content = new Grid
		{
			Padding = 10,
			RowSpacing = 10,
			//BackgroundColor = Color.FromArgb("212529"),
			BackgroundColor = Colors.Transparent,
            RowDefinitions = new RowDefinitionCollection
			{
new RowDefinition(new GridLength(0.3, GridUnitType.Star)),

				new RowDefinition(), // el resto para el formulario
			},
			Children =
			{

							new Border()
							{
								Padding = 10,
								BackgroundColor = Color.FromArgb("#212529"),
								StrokeShape = new RoundRectangle
								{
									CornerRadius = new CornerRadius(10),

								},

								StrokeThickness = 2,

								Content = new Grid
								{
									RowDefinitions = new RowDefinitionCollection
									{
										new RowDefinition(new GridLength(0.4, GridUnitType.Star)),
										new RowDefinition(new GridLength(0.6, GridUnitType.Star)),
									},
									Children = {
											new Label
											{
												Text = "Ingresa las credenciales asignadas por tu institución",
												TextColor = Colors.White,
												FontSize = 20,
												HorizontalTextAlignment = TextAlignment.Center
											}
										.Center()
										.Font(family: "text"),

											new Border()
											{
												BackgroundColor = Color.FromArgb("212529"),
												StrokeShape = new RoundRectangle
												{
													CornerRadius = new CornerRadius(10),

												},

												StrokeThickness = 2,
												Content =
											new ScrollView
											{
												BackgroundColor = Color.FromArgb("212529"),
												Margin = new Thickness(0,10),

												Content = new HorizontalStackLayout
												{   Spacing = 10,
													Children =
													{
														new Image
														{
															Source = "acento.png"
                                                        }
#if WINDOWS
														.Width(40),
#else
														.Width(60),
#endif
														new Image
														{
															Source = "mysoftlogo.png"
														}
#if WINDOWS
														.Width(40)
#else
														.Width(60)
#endif
													}
												}

											}
										}.Row(1),
											}
									}


								}.Row(0),

				new Border()
				{
					Padding = new Thickness(10,50, 10, 10),
					StrokeShape = new RoundRectangle
					{
						CornerRadius = new CornerRadius(10),
						Shadow = new Shadow
						{
							Radius = 10,
							Offset = new Point(5, 5),
							Brush = Brush.White
						},
					},
					StrokeThickness = 2,
					BackgroundColor = Colors.Wheat,
					Content = new VerticalStackLayout()
					{
						Spacing = 30,
						Children =
						{

							new Border()
							{
								Padding = 10,
								Margin = 10,
								BackgroundColor = Colors.White,
								StrokeShape = new RoundRectangle
								{
									CornerRadius = new CornerRadius(10),

								},
								Shadow = new Shadow
									{
										Radius = 10,
										Offset = new Point(5, 5),
										Brush = Brush.Black
									},
								StrokeThickness = 2,

								Content = new Entry
									{
										Placeholder = "Introduce tu correo electronico",
										PlaceholderColor = Colors.Grey,
										TextColor = Colors.Black
									}
								.Font(family: "text", size: 18)
								.Bind(Entry.TextProperty, "GuardarSesionModel.Email"),
							},

							new Border()
							{
								Padding = 10,
								Margin = 10,
								BackgroundColor = Colors.White,
								StrokeShape = new RoundRectangle
								{
									CornerRadius = new CornerRadius(10),

								},
								Shadow = new Shadow
									{
										Radius = 10,
										Offset = new Point(5, 5),
										Brush = Brush.Black
									},
								StrokeThickness = 2,

								Content = new Entry
								{
									Placeholder = "Introduce tu Contraseña",
									IsPassword = true,
									PlaceholderColor = Colors.Grey,
									TextColor = Colors.Black

								}
								.Font(family: "text", size: 18)
								.Bind(Entry.TextProperty, "GuardarSesionModel.Password"),
							},

							new Button
							{
								Margin = 10,
							}.Text("Iniciar Sesion")
							.Font("text")
							.BindCommand("loginCommand")

						}
					}
				}.Row(1),

                
			}


		};
		


    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        //if (BindingContext is LoginViewModel vm)
        //{
        //    vm.PreferencesInitialize();
        //}
    }


}