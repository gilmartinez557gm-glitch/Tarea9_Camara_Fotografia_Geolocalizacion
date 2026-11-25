using Tarea9_Camara_Fotografia_Geolocalizacion.ViewModels;

namespace Tarea9_Camara_Fotografia_Geolocalizacion.Views;

public partial class RegistroPage : ContentPage
{
    public RegistroPage(RegistroViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}