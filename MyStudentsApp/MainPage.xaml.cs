using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;

namespace MyStudentsApp
{
    public partial class MainPage : ContentPage
    {
        public GestionUsuariosServiceApp service { get; set; }

        string nombre;
        List<EscuelaResponseDto> escuelas;
        public MainPage()
        {
            service = new GestionUsuariosServiceApp();
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //try  
            //{  
            //escuelas = await service.obtenerEscuelasAsync();  
            //lblCount.Text = $"{escuelas.Count} escuelas";  
            //lblName.Text = escuelas.FirstOrDefault()?.nombre ?? "Sin nombre";  
            //}  
            //catch (Exception ex)  
            //{  
            //    await DisplayAlert("Error", ex.Message, "OK");  
            //}  
        }
    }
}