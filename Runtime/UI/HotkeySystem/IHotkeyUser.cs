using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames
{
    public interface IHotkeyUser
    {
        protected abstract void OnHotkeyPressed();
    }
}