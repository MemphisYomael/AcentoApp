using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.ViewModels;
using MyStudentsApp.Shared.DTOShared;

namespace MyStudentsApp.MVVM.Views;

public class TareasView : ContentPage
{
    private readonly TareasViewModel _viewModel;

    public TareasView(TareasViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        Title = "Tareas";
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
                    Text = "Tareas y entregas",
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
                    Content = new ScrollView
                    {
                        Content = new VerticalStackLayout
                        {
                            Spacing = 12,
                            Children =
                            {
                                TeacherCreateSection(),
                                TasksSection(),
                                StudentSubmitSection(),
                                TeacherSubmissionsSection(),
                                StudentGradesSection(),
                                new Label
                                {
                                    FontSize = 13,
                                    TextColor = Color.FromArgb("#EAF2F8")
                                }.Bind(Label.TextProperty, nameof(TareasViewModel.StatusMessage))
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

    private View TeacherCreateSection()
    {
        var picker = new Picker
        {
            Title = "Curso",
            ItemDisplayBinding = new Binding(nameof(CursoResponseDTO.nombre)),
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#17202A"),
            TitleColor = Color.FromArgb("#566573")
        }.Bind(Picker.ItemsSourceProperty, nameof(TareasViewModel.Cursos))
         .Bind(Picker.SelectedItemProperty, nameof(TareasViewModel.CursoSeleccionado), BindingMode.TwoWay);

        picker.SelectedIndexChanged += (_, _) =>
        {
            if (picker.SelectedItem is CursoResponseDTO curso)
            {
                _viewModel.SeleccionarCursoCommand.Execute(curso);
            }
        };

        return Section("Crear tarea", new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                new Border
                {
                    Padding = new Thickness(12, 2),
                    Stroke = Color.FromArgb("#BFCAD4"),
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    BackgroundColor = Colors.White,
                    Content = picker
                },
                InputBox(TextEntry("Título").Bind(Entry.TextProperty, nameof(TareasViewModel.NuevaTareaTitulo), BindingMode.TwoWay)),
                InputBox(EditorBox("Descripción").Bind(Editor.TextProperty, nameof(TareasViewModel.NuevaTareaDescripcion), BindingMode.TwoWay), 96),
                new Border
                {
                    Padding = new Thickness(12, 2),
                    Stroke = Color.FromArgb("#BFCAD4"),
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    BackgroundColor = Colors.White,
                    Content = new DatePicker
                    {
                        BackgroundColor = Colors.Transparent,
                        TextColor = Color.FromArgb("#17202A"),
                        Format = "dd/MM/yyyy"
                    }.Bind(DatePicker.DateProperty, nameof(TareasViewModel.NuevaTareaFecha), BindingMode.TwoWay)
                },
                new Label
                {
                    Text = "Estudiantes",
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#17202A")
                },
                    new CollectionView
                    {
                        HeightRequest = 180,
                        SelectionMode = SelectionMode.None,
                        EmptyView = new Label { Text = "Selecciona un curso para ver estudiantes.", TextColor = Color.FromArgb("#566573") },
                        ItemTemplate = new DataTemplate(() => new Grid
                        {
                            Padding = new Thickness(0, 4),
                            ColumnDefinitions =
                        {
                            new ColumnDefinition(GridLength.Auto),
                            new ColumnDefinition(GridLength.Star)
                        },
                        Children =
                        {
                            new CheckBox()
                                .Bind(CheckBox.IsCheckedProperty, nameof(SelectableEstudianteItem.IsSelected), BindingMode.TwoWay)
                                    .Column(0),
                            new Label
                            {
                                FontSize = 14,
                                TextColor = Color.FromArgb("#17202A"),
                                VerticalOptions = LayoutOptions.Center
                            }.Bind(Label.TextProperty, nameof(SelectableEstudianteItem.Nombre)).Column(1)
                        }
                    })
                }.Bind(CollectionView.ItemsSourceProperty, nameof(TareasViewModel.EstudiantesCurso)),
                PrimaryButton("Crear y asignar").Bind(Button.CommandProperty, nameof(TareasViewModel.CrearTareaCommand)),
                SecondaryButton("Enviar recordatorios de 24h").Bind(Button.CommandProperty, nameof(TareasViewModel.EnviarRecordatoriosCommand))
            }
        }).Bind(Border.IsVisibleProperty, nameof(TareasViewModel.IsTeacher));
    }

    private View TasksSection()
    {
        var tasks = new CollectionView
        {
            SelectionMode = SelectionMode.Single,
            EmptyView = new Label { Text = "No hay tareas para mostrar.", TextColor = Color.FromArgb("#566573") },
            ItemTemplate = new DataTemplate(() => new Border
            {
                Padding = 12,
                Margin = new Thickness(0, 5),
                Stroke = Color.FromArgb("#E9ECEF"),
                BackgroundColor = Colors.White,
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                Content = new VerticalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        new Label
                        {
                            FontSize = 16,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.FromArgb("#17202A")
                        }.Bind(Label.TextProperty, nameof(TareaResponseDTO.Title)),
                        new Label
                        {
                            FontSize = 13,
                            TextColor = Color.FromArgb("#566573"),
                            LineBreakMode = LineBreakMode.TailTruncation
                        }.Bind(Label.TextProperty, nameof(TareaResponseDTO.Description)),
                        new Label
                        {
                            FontSize = 12,
                            TextColor = Color.FromArgb("#1F7A8C")
                        }.Bind(Label.TextProperty, nameof(TareaResponseDTO.fechaEntrega), stringFormat: "Entrega: {0:g}")
                    }
                }
            })
        }.Bind(CollectionView.ItemsSourceProperty, nameof(TareasViewModel.Tareas));

        tasks.SelectionChanged += (_, e) =>
        {
            var tarea = e.CurrentSelection.FirstOrDefault() as TareaResponseDTO;
            tasks.SelectedItem = null;
            if (tarea != null)
            {
                _viewModel.SeleccionarTareaCommand.Execute(tarea);
            }
        };

        return Section("Mis tareas", tasks);
    }

    private View StudentSubmitSection()
    {
        return Section("Enviar entrega", new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                new Label
                {
                    TextColor = Color.FromArgb("#566573"),
                    FontSize = 13
                }.Bind(Label.TextProperty, nameof(TareasViewModel.TareaSeleccionada), convert: (TareaResponseDTO? t) => t == null ? "Selecciona una tarea." : $"Seleccionada: {t.Title}"),
                InputBox(TextEntry("Título de la entrega").Bind(Entry.TextProperty, nameof(TareasViewModel.EntregaTitulo), BindingMode.TwoWay)),
                InputBox(EditorBox("Comentario").Bind(Editor.TextProperty, nameof(TareasViewModel.EntregaDescripcion), BindingMode.TwoWay), 96),
                new HorizontalStackLayout
                {
                    Spacing = 10,
                    Children =
                    {
                        SecondaryButton("Archivo").Bind(Button.CommandProperty, nameof(TareasViewModel.SeleccionarArchivoCommand)),
                        new Label
                        {
                            FontSize = 13,
                            TextColor = Color.FromArgb("#566573"),
                            VerticalOptions = LayoutOptions.Center
                        }.Bind(Label.TextProperty, nameof(TareasViewModel.ArchivoSeleccionadoNombre))
                    }
                },
                PrimaryButton("Enviar").Bind(Button.CommandProperty, nameof(TareasViewModel.CrearEntregaCommand))
            }
        }).Bind(Border.IsVisibleProperty, nameof(TareasViewModel.IsStudent));
    }

    private View TeacherSubmissionsSection()
    {
        return Section("Entregas de la tarea", new CollectionView
        {
            EmptyView = new Label { Text = "Selecciona una tarea para ver entregas.", TextColor = Color.FromArgb("#566573") },
            ItemTemplate = new DataTemplate(() =>
            {
                var calificarButton = PrimaryButton("Calificar");
                calificarButton.Command = _viewModel.CalificarEntregaCommand;
                calificarButton.SetBinding(Button.CommandParameterProperty, ".");

                return new Border
                {
                    Padding = 12,
                    Margin = new Thickness(0, 5),
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
                                FontSize = 15,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Color.FromArgb("#17202A")
                            }.Bind(Label.TextProperty, nameof(EntregaTareaResponseDTO.Estudiante), convert: (EstudianteResponseDTO? e) => e == null ? "Estudiante" : $"{e.nombres} {e.apellidos}"),
                            new Label
                            {
                                FontSize = 13,
                                TextColor = Color.FromArgb("#566573")
                            }.Bind(Label.TextProperty, nameof(EntregaTareaResponseDTO.Descripcion)),
                            new Label
                            {
                                FontSize = 12,
                                TextColor = Color.FromArgb("#1F7A8C")
                            }.Bind(Label.TextProperty, nameof(EntregaTareaResponseDTO.EstaCalificada), convert: (bool ok) => ok ? "Calificada" : "Pendiente"),
                            calificarButton
                        }
                    }
                };
            })
        }.Bind(CollectionView.ItemsSourceProperty, nameof(TareasViewModel.EntregasTarea))).Bind(Border.IsVisibleProperty, nameof(TareasViewModel.IsTeacher));
    }

    private View StudentGradesSection()
    {
        return Section("Mis entregas y calificaciones", new CollectionView
        {
            EmptyView = new Label { Text = "Aún no tienes entregas registradas.", TextColor = Color.FromArgb("#566573") },
            ItemTemplate = new DataTemplate(() => new Border
            {
                Padding = 12,
                Margin = new Thickness(0, 5),
                Stroke = Color.FromArgb("#E9ECEF"),
                BackgroundColor = Colors.White,
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                Content = new VerticalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        new Label
                        {
                            FontSize = 15,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.FromArgb("#17202A")
                        }.Bind(Label.TextProperty, nameof(EntregaTareaResponseDTO.Titulo)),
                        new Label
                        {
                            FontSize = 12,
                            TextColor = Color.FromArgb("#1F7A8C")
                        }.Bind(Label.TextProperty, nameof(EntregaTareaResponseDTO.calificacion), stringFormat: "Calificación: {0}"),
                        new Label
                        {
                            FontSize = 13,
                            TextColor = Color.FromArgb("#566573")
                        }.Bind(Label.TextProperty, nameof(EntregaTareaResponseDTO.Comentarios))
                    }
                }
            })
        }.Bind(CollectionView.ItemsSourceProperty, nameof(TareasViewModel.EntregasEstudiante))).Bind(Border.IsVisibleProperty, nameof(TareasViewModel.IsStudent));
    }

    private static Border Section(string title, View content)
    {
        return new Border
        {
            Padding = 14,
            Stroke = Color.FromArgb("#FFFFFF"),
            StrokeThickness = 1,
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    new Label
                    {
                        Text = title,
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#17202A")
                    },
                    content
                }
            }
        };
    }

    private static Entry TextEntry(string placeholder)
    {
        return new Entry
        {
            Placeholder = placeholder,
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#17202A"),
            PlaceholderColor = Color.FromArgb("#85929E"),
            HeightRequest = 44
        };
    }

    private static Editor EditorBox(string placeholder)
    {
        return new Editor
        {
            Placeholder = placeholder,
            AutoSize = EditorAutoSizeOption.TextChanges,
            MinimumHeightRequest = 80,
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#17202A"),
            PlaceholderColor = Color.FromArgb("#85929E")
        };
    }

    private static Border InputBox(View content, double? heightRequest = null)
    {
        var border = new Border
        {
            Padding = new Thickness(12, 2),
            Stroke = Color.FromArgb("#BFCAD4"),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            BackgroundColor = Colors.White,
            Content = content
        };

        if (heightRequest.HasValue)
        {
            border.HeightRequest = heightRequest.Value;
        }

        return border;
    }

    private static Button PrimaryButton(string text)
    {
        return new Button
        {
            Text = text,
            BackgroundColor = Color.FromArgb("#48c1ec"),
            TextColor = Colors.White,
            CornerRadius = 8,
            HeightRequest = 42
        };
    }

    private static Button SecondaryButton(string text)
    {
        return new Button
        {
            Text = text,
            BackgroundColor = Color.FromArgb("#EAF2F8"),
            TextColor = Color.FromArgb("#17202A"),
            CornerRadius = 8,
            HeightRequest = 42
        };
    }
}
