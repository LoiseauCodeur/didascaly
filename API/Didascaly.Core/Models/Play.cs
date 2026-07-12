using System.Text.Json.Serialization;

namespace Didascaly.Core.Models;

public class Play
{
    public PlayMetadata Metadata { get; set; } = new();
    public List<CharacterDef> Characters { get; set; } = new();
    public List<ScriptItem> Script { get; set; } = new();
}

public class PlayMetadata
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}

public class CharacterDef
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class ScriptItem
{
    public int LineId { get; set; }
    public string Type { get; set; } = string.Empty; // "header" ou "dialogue"
    public string? Character { get; set; }
    public string? Didascaly { get; set; }
    public string? Content { get; set; }
}