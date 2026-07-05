namespace Didascaly.Core.Models;

public class Room
{
    public string RoomId { get; set; } = Guid.NewGuid().ToString()[..6].ToUpper();
    public string PlayTitle { get; set; } = string.Empty;
    
    public List<Player> Players { get; set; } = new();
    public List<ScriptLine> Script { get; set; } = new();
    
    public int CurrentLineIndex { get; set; } = 0; 

    public ScriptLine? GetCurrentLine()
    {
        if (CurrentLineIndex < Script.Count)
            return Script[CurrentLineIndex];
        
        return null;
    }
}