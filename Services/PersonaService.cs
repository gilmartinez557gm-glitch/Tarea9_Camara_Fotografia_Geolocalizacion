using System.Net.Mail;
using Tarea9_Camara_Fotografia_Geolocalizacion.Models;

namespace Tarea9_Camara_Fotografia_Geolocalizacion.Services;

public class PersonaService
{
    public string? ValidarPersona(Persona persona)
    {
        if (string.IsNullOrWhiteSpace(persona.Identidad) ||
            persona.Identidad.Length != 13 ||
            !persona.Identidad.All(char.IsDigit))
            return "La identidad debe contener 13 dígitos.";

        if (string.IsNullOrWhiteSpace(persona.Nombre) ||
            persona.Nombre.Trim().Length < 3)
            return "Ingrese un nombre válido.";

        if (persona.Edad < 0 || persona.Edad > 120)
            return "Ingrese una edad válida.";

        if (string.IsNullOrWhiteSpace(persona.Sexo))
            return "Seleccione el sexo.";

        if (!MailAddress.TryCreate(persona.Correo, out _))
            return "Ingrese un correo electrónico válido.";

        string telefono = new string(
            persona.Telefono.Where(char.IsDigit).ToArray());

        if (telefono.Length < 8)
            return "Ingrese un teléfono válido.";

        if (string.IsNullOrWhiteSpace(persona.Direccion))
            return "Ingrese la dirección.";

        if (string.IsNullOrWhiteSpace(persona.Ocupacion))
            return "Ingrese la ocupación.";

        return null;
    }
}

