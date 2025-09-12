using System.Xml;
using MyStudentsApp.MVVM.Models.SkiaControls;

namespace MyStudentsApp.MVVM.Models.ContentViews;

public partial class TarjetaUsuarioControl : ContentView
{

	

    public static readonly BindableProperty UserNameProperty =
       BindableProperty.Create(nameof(UserName), typeof(string), typeof(NotificationBadgeView), string.Empty, propertyChanged: OnPropertiesChanged);

    public static readonly BindableProperty UserPositionProperty =
        BindableProperty.Create(nameof(UserPosition), typeof(string), typeof(NotificationBadgeView), string.Empty, propertyChanged: OnPropertiesChanged);

    public static readonly BindableProperty AvatarColorProperty =
        BindableProperty.Create(nameof(AvatarColor), typeof(Color), typeof(NotificationBadgeView), Colors.LightGray, propertyChanged: OnPropertiesChanged);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(NotificationBadgeView), Colors.Black, propertyChanged: OnPropertiesChanged);

    public static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(NotificationBadgeView), Colors.White, propertyChanged: OnPropertiesChanged);

    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(NotificationBadgeView), null, propertyChanged: OnPropertiesChanged);

    public string UserName
    {
        get => (string)GetValue(UserNameProperty);
        set => SetValue(UserNameProperty, value);
    }

    public string UserPosition
    {
        get => (string)GetValue(UserPositionProperty);
        set => SetValue(UserPositionProperty, value);
    }

    public Color AvatarColor
    {
        get => (Color)GetValue(AvatarColorProperty);
        set => SetValue(AvatarColorProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    private static void OnPropertiesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TarjetaUsuarioControl)bindable; // Cambiar el tipo de control a TarjetaUsuarioControl
        control.UpdateUI();
    }

    public TarjetaUsuarioControl()
    {
        InitializeComponent();
        UpdateUI();
    }
    private void UpdateUI()
    {
        // Actualizar el Color de fondo del Border principal
        mainBorder.BackgroundColor = BackgroundColor;

        // Lógica para el AvatarView (imagen o iniciales)
        if (!string.IsNullOrWhiteSpace(ImageUrl))
        {
            avatar.ImageSource = new UriImageSource
            {
                Uri = new Uri(ImageUrl),
                CachingEnabled = true,
                CacheValidity = TimeSpan.FromDays(7)
            }
            ;
            avatar.Text = string.Empty; // Asegurarse de que el texto esté vacío si hay una imagen
            avatar.BackgroundColor = Colors.Transparent; // El color de fondo del AvatarView solo aplica al texto
        }
        else
        {
            avatar.ImageSource = null;
            avatar.Text = GetInitials(UserName);
            avatar.BackgroundColor = AvatarColor;
        }

        // Actualizar el texto y colores de las etiquetas
        nameLabel.Text = UserName;
        nameLabel.TextColor = TextColor;
        positionLabel.Text = UserPosition;
        positionLabel.TextColor = TextColor;
    }

    /// <summary>
    /// Obtiene las iniciales a partir de un nombre completo.
    /// </summary>
    /// <param name="name">El nombre de la persona.</param>
    /// <returns>Una cadena con las iniciales (ej. "JD" para "John Doe").</returns>
    private string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "?";

        var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length > 1)
        {
            return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
        }
        return name.Length > 1 ? name.Substring(0, 2).ToUpper() : name.ToUpper();
    }
}