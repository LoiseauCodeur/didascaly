using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Didascaly.Api.Services;

namespace Didascaly.Api.Hubs;

public class TheaterHub : Hub
{
    private readonly RoomManager _roomManager;

    public TheaterHub(RoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        
        int currentLine = _roomManager.GetCurrentLine(roomId);
        
        await Clients.Caller.SendAsync("ReceiveLineUpdate", currentLine);
        
        await Clients.Group(roomId).SendAsync("PlayerJoined", Context.ConnectionId);
    }

    public async Task NextLine(string roomId)
    {
        int newLineIndex = _roomManager.AdvanceLine(roomId);
        
        await Clients.Group(roomId).SendAsync("ReceiveLineUpdate", newLineIndex);
    }
}