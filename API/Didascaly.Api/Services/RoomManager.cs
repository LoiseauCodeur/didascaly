using System.Collections.Concurrent;

namespace Didascaly.Api.Services;

public class RoomManager
{
    private readonly ConcurrentDictionary<string, int> _roomStates = new ConcurrentDictionary<string, int>();

    public int GetCurrentLine(string roomId)
    {
        int currentLine;
        bool exists = _roomStates.TryGetValue(roomId, out currentLine);
        
        if (exists)
            return currentLine;
        
        return 0;
    }

    public int AdvanceLine(string roomId)
    {
        int newLineIndex = _roomStates.AddOrUpdate(roomId, 1, (string key, int oldValue) => oldValue + 1);
        return newLineIndex;
    }
}