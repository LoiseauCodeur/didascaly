namespace Didascaly.Core.Models;

public class Player
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Pseudo { get; set; } = string.Empty;
    public string CharacterName { get; set; } = string.Empty; 
    public bool IsRobot { get; set; } = false; 
}