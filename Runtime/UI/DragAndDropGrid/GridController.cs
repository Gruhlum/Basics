using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private List<DragAndDropGrid> grids = default;

        [SerializeField] private DragAndDropGrid activeGrid;
        public DragAndDropGrid HoverGrid
        {
            get
            {
                return hoverGrid;
            }
            set
            {
                hoverGrid = value;              
            }
        }
        [SerializeField] private DragAndDropGrid hoverGrid;

        private RectTransform activeItem;

        private void Reset()
        {
            grids = new List<DragAndDropGrid>();
            grids.AddRange(GetComponentsInChildren<DragAndDropGrid>());
            foreach (var grid in grids)
            {
                if (grid.gridController == null)
                {
                    grid.gridController = this;
                }
            }
        }
        public void GridItemSelected(DragAndDropGrid grid, RectTransform item)
        {          
            if (item != null)
            {
                activeItem = item;
                grid.transform.SetAsLastSibling();
                activeGrid = grid;
            }
            else
            {
                if (HoverGrid != null)
                {
                    activeGrid.SendItem(activeItem, HoverGrid);
                    HoverGrid.ReceiveItem(activeItem, activeGrid);                    
                }               
                HoverGrid = null;
                activeGrid = null;
                activeItem = null;
            } 
        }
        private void Update()
        {
            if (activeGrid == null)
            {
                return;
            }
            if (activeGrid.AllowSending == false)
            {
                return;
            }
            foreach (var grid in grids)
            {
                if (grid == activeGrid)
                {
                    continue;
                }
                if (grid.CanAcceptItem == false)
                {
                    continue;
                }
                if (grid.IsInsideGrid())
                {
                    HoverGrid = grid;
                    if (grid.ContainsItem(activeItem))
                    {
                        grid.RemoveItem(activeItem);
                    }
                    grid.InsertItem(activeItem);
                    break;
                }
                else if (grid == HoverGrid)
                {
                    HoverGrid.RemoveItem(activeItem);
                    HoverGrid.ResizeTransform();
                    HoverGrid.UpdateGridItems();
                    HoverGrid = null;
                }
            }           
        }
    }
}