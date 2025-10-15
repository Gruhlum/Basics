using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UIGrid
{
    public class CellDisplay : MonoBehaviour
    {
        [SerializeField] private Image img = default;
        [SerializeField] private TMP_Text textGUI = default;

        public void Setup(int x, int y, Vector3 position, Vector2 size)
        {
            img.color = RandomUtility.GenerateRandomColor(0.8f);
            textGUI.text = $"{x},{y}";
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