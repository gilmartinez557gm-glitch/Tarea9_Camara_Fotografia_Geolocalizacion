namespace Tarea9_Camara_Fotografia_Geolocalizacion;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<Services.PersonaService>();
        builder.Services.AddSingleton<Services.DatabaseService>();
        builder.Services.AddSingleton<Services.DeviceService>();

        builder.Services.AddSingleton<ViewModels.RegistroViewModel>();
        builder.Services.AddSingleton<Views.RegistroPage>();

        return builder.Build();
    }
}
