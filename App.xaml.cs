using Tarea9_Camara_Fotografia_Geolocalizacion.Views;

namespace Tarea9_Camara_Fotografia_Geolocalizacion;

public partial class App : Application
{
    private readonly RegistroPage _registroPage;

    public App(RegistroPage registroPage)
    {
        InitializeComponent();
        _registroPage = registroPage;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_registroPage);
    }
}