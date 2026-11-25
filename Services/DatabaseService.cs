using SQLite;
using Tarea9_Camara_Fotografia_Geolocalizacion.Models;

namespace Tarea9_Camara_Fotografia_Geolocalizacion.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        string ruta = Path.Combine(
            FileSystem.AppDataDirectory,
            "Personas.db3");

        _database = new SQLiteAsyncConnection(ruta);
    }

    public async Task InicializarAsync()
    {
        await _database.CreateTableAsync<Persona>();
    }

    public async Task<List<Persona>> ObtenerPersonasAsync()
    {
        await InicializarAsync();

        return await _database.Table<Persona>().ToListAsync();
    }

    public async Task GuardarPersonaAsync(Persona persona)
    {
        await InicializarAsync();

        await _database.InsertAsync(persona);
    }

    public async Task<bool> ExisteIdentidadAsync(string identidad)
    {
        await InicializarAsync();

        return await _database.Table<Persona>()
            .Where(p => p.Identidad == identidad)
            .FirstOrDefaultAsync() != null;
    }
}
