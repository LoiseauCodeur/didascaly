using Didascaly.Core.Models;

namespace Didascaly.Core.Interfaces;

public interface IPlayRepository
{
    Play? GetPlay(string title);
}