using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    public class CellDisplayController : MonoBehaviour
    {
        [SerializeField] private Spawner<CellDisplay> cellDisplaySpawner = default;

        [SerializeField] private Canvas canvas = default;


        public void DisplayCells<T>(List<Cell<T>> cells) where T : ISpawnable<T>
        {
            foreach (var cell in cells)
            {
                cellDisplaySpawner.Spawn().Setup(cell.X, cell.Y, cell.GetPosition() * canvas.transform.localScale, cell.Size);
            }
        }
    }
}