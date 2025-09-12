using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using MyStudentsApp.MVVM.Models.ContentViews;
using MyStudentsApp.MVVM.Models.SkiaControls;
using SkiaSharp;

namespace MyStudentsApp.MVVM.Views;

public partial class vista : ContentPage
{
	public vista()
	{
		InitializeComponent();
        //Content = new Grid
        //{
        //    BackgroundColor = Colors.LightGray,
        //    ColumnDefinitions = new ColumnDefinitionCollection
        //    {
        //        new ColumnDefinition(GridLength.Star),
        //        new ColumnDefinition(GridLength.Star),
        //        new ColumnDefinition(GridLength.Star),

        //    },
        //    RowDefinitions = new RowDefinitionCollection
        //    {
        //        new RowDefinition(GridLength.Star),
        //        new RowDefinition(GridLength.Star),
        //        new RowDefinition(GridLength.Star)


        //    },
        //    Children =
        //    {
        //        new Label().Text("Vista de Notificaciones").Row(0).Column(1),
        //       //new AvatarView
        //       // {
        //       //     Text = "SH",
        //       //     BackgroundColor = Colors.DarkBlue,
        //       //     TextColor = Colors.White,
        //       //     FontSize = 64,
        //       //     ImageSource = "https://img.freepik.com/free-vector/businessman-character-avatar-isolated_24877-60111.jpg?semt=ais_hybrid&w=740"
        //       // },

        //        new Grid{
        //            Children = {
        //            new NotificationBadgeView
        //            {
        //                Margin = new Thickness(0, 10, 10, 0),
        //                BadgeNumber = 1,
        //                BadgeColor = SKColors.Red,
        //                TextColor = SKColors.White,
        //                BadgeRadius = 22f,
        //            }.End().ZIndex(2),

        //            new TarjetaUsuarioControl
        //            {
        //                UserName = "Juan Pérez",
        //                UserPosition = "Desarrollador",
        //                AvatarColor = Colors.DarkOrange,
        //                TextColor = Colors.Black,
        //                BackgroundColor = Colors.LightGray
        //            }
        //            }
        //        }.Row(1).Column(0).ColumnSpan(3).Height(100)
        //    }
        //};
	}


    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Aquí puedes agregar lógica adicional que necesites al aparecer la vista
        
    }



}