using SQLite;

namespace Tarea9_Camara_Fotografia_Geolocalizacion.Models;

public class Persona
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique]
    public string Identidad { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ocupacion { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }

    public string FotoBase64 { get; set; } = string.Empty;
    [Ignore]
    public ImageSource? FotoImagen =>
    string.IsNullOrEmpty(FotoBase64)
        ? null
        : ImageSource.FromStream(() =>
            new MemoryStream(Convert.FromBase64String(FotoBase64)));

    public double Latitud { get; set; }
    public double Longitud { get; set; }
}
