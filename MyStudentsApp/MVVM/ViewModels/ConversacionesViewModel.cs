using System.Collections.ObjectModel;
using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ConversacionesViewModel
    {
        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;
        private List<ConversacionResponseDTO> _todas = new();

        public ObservableCollection<ConversacionResponseDTO> Conversaciones { get; set; } = new();
        public string SearchText { get; set; } = string.Empty;
        public bool IsBusy { get; set; }
        public string? ErrorMessage { get; set; }
        public ICommand CargarCommand { get; }
        public ICommand BuscarCommand { get; }

        public ConversacionesViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            _gestionUsuarioService = gestionUsuarioService;
            CargarCommand = new Command(async () => await LoadAsync());
            BuscarCommand = new Command<string>(Filtrar);
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ErrorMessage = null;
                _todas = await _gestionUsuarioService.ObtenerConversacionesAsync();
                Filtrar(SearchText);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"No se pudieron cargar las conversaciones: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Filtrar(string? texto)
        {
            SearchText = texto ?? string.Empty;
            var query = _todas.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(c =>
                    (c.otherUserName ?? string.Empty).Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    (c.otherUserRole ?? string.Empty).Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    (c.lastMessage ?? string.Empty).Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            Conversaciones.Clear();
            foreach (var conversacion in query.OrderByDescending(c => c.lastMessageAt))
            {
                Conversaciones.Add(conversacion);
            }
        }
    }
}
