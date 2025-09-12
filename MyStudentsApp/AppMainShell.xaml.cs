namespace MyStudentsApp;

public partial class AppMainShell : Shell
{
    public AppMainShell()
	{
		InitializeComponent();
	}

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
        titulo.Text = "Acento";
        titulo.FontSize = 20;
        titulo.FontFamily = "text";
        Shell.Current.FlyoutIsPresented = false;
    }
}