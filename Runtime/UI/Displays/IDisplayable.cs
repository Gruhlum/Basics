using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public interface IDisplayable
    {
        Sprite Icon
        {
            get;
        }
        string Name
        {
            get;
        }
        string Description
        {
            get;
        }
    }
}