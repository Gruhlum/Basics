using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UIGrid
{
    public class CellDisplay : MonoBehaviour
    {
        [SerializeField] private Image img = default;


        public void Setup(Vector3 position, Vector2 size)
        {
            img.color = Utility.GenerateRandomColor(0.8f);
            img.SetAlpha(0.2f);
            img.GetComponent<RectTransform>().sizeDelta = size;

            transform.position = position;
            //img.GetComponent<RectTransform>().sizeDelta = n;
        }
        //protected override void DrawItem(Cell cell)
        //{
            
        //    
        //    //Debug.Log(result);
           
        //    transform.position = result;
        //}
    }
}