using System;

namespace HexTecGames.Basics
{
    public interface ISpawnable<T>
    {
        event Action<T> OnDeactivated;
    }
}