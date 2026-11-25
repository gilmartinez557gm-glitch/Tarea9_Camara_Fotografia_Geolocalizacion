using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Tarea9_Camara_Fotografia_Geolocalizacion.Models;
using Tarea9_Camara_Fotografia_Geolocalizacion.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tarea9_Camara_Fotografia_Geolocalizacion.ViewModels;

public partial class RegistroViewModel : ObservableObject
{
    private readonly PersonaService _personaService;
    private readonly DatabaseService _databaseService;
    private readonly DeviceService _deviceService;

    public RegistroViewModel(
        PersonaService personaService,
        DatabaseService databaseService,
        DeviceService deviceService)
    {
        _personaService = personaService;
        _databaseService = databaseService;
        _deviceService = deviceService;

        _ = CargarPersonasAsync();
    }

    [ObservableProperty]
    private string identidad = string.Empty;

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string edad = string.Empty;

    [ObservableProperty]
    private string sexo = string.Empty;

    [ObservableProperty]
    private string correo = string.Empty;

    [ObservableProperty]
    private string telefono = string.Empty;

    [ObservableProperty]
    private string direccion = string.Empty;

    [ObservableProperty]
    private string ocupacion = string.Empty;

    [ObservableProperty]
    private DateTime fechaRegistro = DateTime.Today;

    [ObservableProperty]
    private string fotoBase64 = string.Empty;

    [ObservableProperty]
    private ImageSource? fotoImagen;

    [ObservableProperty]
    private double latitud;

    [ObservableProperty]
    private double longitud;

    [ObservableProperty]
    private string ubicacionTexto = "Ubicación no capturada";

    public ObservableCollection<Persona> Personas { get; } = new();

    public List<string> Sexos { get; } = new()
    {
        "Masculino",
        "Femenino"
    };

    partial void OnFotoBase64Changed(string value)
    {
        FotoImagen = string.IsNullOrEmpty(value)
            ? null
            : ImageSource.FromStream(() =>
                new MemoryStream(Convert.FromBase64String(value)));
    }

    private async Task CargarPersonasAsync()
    {
        try
        {
            var personas = await _databaseService.ObtenerPersonasAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Personas.Clear();

                foreach (var persona in personas)
                    Personas.Add(persona);
            });
        }
        catch (Exception ex)
        {
            await MostrarMensajeAsync("Error", ex.Message);
        }
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (!int.TryParse(Edad, out int edadNumero))
        {
            await MostrarMensajeAsync("Validación", "Ingrese una edad válida.");
            return;
        }

        var persona = new Persona
        {
            Identidad = Identidad.Trim(),
            Nombre = Nombre.Trim(),
            Edad = edadNumero,
            Sexo = Sexo,
            Correo = Correo.Trim(),
            Telefono = Telefono.Trim(),
            Direccion = Direccion.Trim(),
            Ocupacion = Ocupacion.Trim(),
            FechaRegistro = FechaRegistro,
            FotoBase64 = FotoBase64,
            Latitud = Latitud,
            Longitud = Longitud
        };

        string? error = _personaService.ValidarPersona(persona);

        if (error != null)
        {
            await MostrarMensajeAsync("Validación", error);
            return;
        }

        if (await _databaseService.ExisteIdentidadAsync(persona.Identidad))
        {
            await MostrarMensajeAsync(
                "Registro duplicado",
                "Esta identidad ya está registrada.");
            return;
        }

        try
        {
            await _databaseService.GuardarPersonaAsync(persona);

            Personas.Add(persona);

            await MostrarMensajeAsync(
                "Registro exitoso",
                "La persona se guardó correctamente.");

            LimpiarFormulario();
        }
        catch (Exception ex)
        {
            await MostrarMensajeAsync("Error al guardar", ex.Message);
        }
    }

    [RelayCommand]
    private async Task CapturarFotoAsync()
    {
        try
        {
            if (!await _deviceService.SolicitarPermisoCamaraAsync())
            {
                await MostrarMensajeAsync(
                    "Permiso denegado",
                    "Debes permitir el acceso a la cámara.");
                return;
            }

            string? foto = await _deviceService.TomarFotoAsync();

            if (foto != null)
                FotoBase64 = foto;
        }
        catch (Exception ex)
        {
            await MostrarMensajeAsync("Error de cámara", ex.Message);
        }
    }

    [RelayCommand]
    private async Task SeleccionarFotoAsync()
    {
        try
        {
            string? foto = await _deviceService.SeleccionarFotoAsync();

            if (foto != null)
                FotoBase64 = foto;
        }
        catch (Exception ex)
        {
            await MostrarMensajeAsync("Error de fotografía", ex.Message);
        }
    }

    [RelayCommand]
    private async Task ObtenerUbicacionAsync()
    {
        try
        {
            if (!await _deviceService.SolicitarPermisoUbicacionAsync())
            {
                await MostrarMensajeAsync(
                    "Permiso denegado",
                    "Debes permitir el acceso a la ubicación.");
                return;
            }

            var ubicacion = await _deviceService.ObtenerUbicacionAsync();

            if (ubicacion == null)
            {
                await MostrarMensajeAsync(
                    "Ubicación",
                    "No fue posible obtener la ubicación.");
                return;
            }

            Latitud = ubicacion.Latitude;
            Longitud = ubicacion.Longitude;

            UbicacionTexto =
                $"Latitud: {Latitud:F6} | Longitud: {Longitud:F6}";
        }
        catch (Exception ex)
        {
            await MostrarMensajeAsync("Error de ubicación", ex.Message);
        }
    }

    [RelayCommand]
    private async Task AbrirMapaAsync(Persona? persona)
    {
        try
        {
            double lat = persona?.Latitud ?? Latitud;
            double lon = persona?.Longitud ?? Longitud;

            if (lat == 0 && lon == 0)
            {
                await MostrarMensajeAsync(
                    "Ubicación",
                    "No hay coordenadas disponibles.");
                return;
            }

            await _deviceService.AbrirMapaAsync(lat, lon);
        }
        catch (Exception ex)
        {
            await MostrarMensajeAsync("Error al abrir mapa", ex.Message);
        }
    }

    private void LimpiarFormulario()
    {
        Identidad = string.Empty;
        Nombre = string.Empty;
        Edad = string.Empty;
        Sexo = string.Empty;
        Correo = string.Empty;
        Telefono = string.Empty;
        Direccion = string.Empty;
        Ocupacion = string.Empty;
        FechaRegistro = DateTime.Today;
        FotoBase64 = string.Empty;
        Latitud = 0;
        Longitud = 0;
        UbicacionTexto = "Ubicación no capturada";
    }

    private static async Task MostrarMensajeAsync(
        string titulo,
        string mensaje)
    {
        var pagina = Application.Current?.Windows.FirstOrDefault()?.Page;

        if (pagina != null)
            await pagina.DisplayAlert(titulo, mensaje, "Aceptar");
    }
}
