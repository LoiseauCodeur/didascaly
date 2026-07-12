using System.Text.Json;
using Didascaly.Core.Interfaces;
using Didascaly.Core.Models;

namespace Didascaly.Api.Services;

public class PlayRepository : IPlayRepository
{
    private readonly Play? _maladeImaginaire;

    public PlayRepository()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Le_Malade_Imaginaire.json");
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _maladeImaginaire = JsonSerializer.Deserialize<Play>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

    public Play? GetPlay(string title)
    {
        return _maladeImaginaire;
    }
}