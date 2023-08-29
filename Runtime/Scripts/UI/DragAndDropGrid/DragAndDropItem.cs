using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HexTecGames.Basics.UI
{
    public class DragAndDropItem : MonoBehaviour, IPointerDownHandler
    {
        public DragAndDropGrid Grid
        {
            get
            {
                return grid;
            }
            set
            {
                grid = value;
            }
        }
        private DragAndDropGrid grid;

        [SerializeField] private RectTransform rectT = default;

        private void Reset()
        {
            rectT = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Grid != null)
            {
                grid.ItemClicked(rectT);
            }           
        }
    }
}