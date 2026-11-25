namespace Tarea9_Camara_Fotografia_Geolocalizacion.Services;

public class DeviceService
{
    public async Task<bool> SolicitarPermisoCamaraAsync()
    {
        var estado =
            await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (estado != PermissionStatus.Granted)
            estado = await Permissions.RequestAsync<Permissions.Camera>();

        return estado == PermissionStatus.Granted;
    }

    public async Task<bool> SolicitarPermisoUbicacionAsync()
    {
        var estado =
            await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (estado != PermissionStatus.Granted)
            estado = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return estado == PermissionStatus.Granted;
    }

    public async Task<string?> TomarFotoAsync()
    {
        var foto = await MediaPicker.Default.CapturePhotoAsync();

        if (foto == null)
            return null;

        using var stream = await foto.OpenReadAsync();
        using var memoria = new MemoryStream();

        await stream.CopyToAsync(memoria);

        return Convert.ToBase64String(memoria.ToArray());
    }

    public async Task<string?> SeleccionarFotoAsync()
    {
        var foto = await MediaPicker.Default.PickPhotoAsync();

        if (foto == null)
            return null;

        using var stream = await foto.OpenReadAsync();
        using var memoria = new MemoryStream();

        await stream.CopyToAsync(memoria);

        return Convert.ToBase64String(memoria.ToArray());
    }

    public async Task<Location?> ObtenerUbicacionAsync()
    {
        return await Geolocation.Default.GetLocationAsync(
            new GeolocationRequest(
                GeolocationAccuracy.Medium,
                TimeSpan.FromSeconds(10)));
    }

    public async Task AbrirMapaAsync(double latitud, double longitud)
    {
        var ubicacion = new Location(latitud, longitud);

        await Map.Default.OpenAsync(
            ubicacion,
            new MapLaunchOptions
            {
                Name = "Ubicación de la persona"
            });
    }
}
