using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.Basics.UIGrid
{
    public class CellDisplayController : MonoBehaviour
    {
        [SerializeField] private Spawner<CellDisplay> cellDisplaySpawner = default;

        public void DisplayCells<T>(List<Cell<T>> cells) where T : ISpawnable<T>
        {
            foreach (var cell in cells)
            {
                Vector3 result = cell.CalculateViewportPosition();
                result.z = -Camera.main.transform.position.z;
                result = Camera.main.ViewportToWorldPoint(result);
                cellDisplaySpawner.Spawn().Setup(result, cell.GetSize());
            }
        }
    }
}