using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public enum Constrain { Column, Row };
    public enum ControlMode { Calculations, Buttons }

    public class DragAndDropGrid : LayoutGroup
    {
        //[SerializeField] private Transform itemsT = default;

        public Constrain Constrain;

        [Min(1)]
        public int MaxLength = 3;
        //TODO: public bool AllowRearranging;

        [Header("Other Grids")]
        public GridController gridController = default;
        public bool AllowSending = true;
        public bool AllowRecieving = true;

        public int SpacingX;
        public int SpacingY;

        [Header("Items")]
        public ControlMode ControlMode;

        public int OverwriteWidth;
        public int OverwriteHeight;

        public int MinItems;
        public int MaxItems;

        public bool AddEmptySpace;
        [SerializeField] private Spawner<Image> emptySpaceSpawner = default;

        private float TotalSpacingX
        {
            get
            {
                return SpacingX + itemWidth;
            }
        }
        private float TotalSpacingY
        {
            get
            {
                return SpacingY + itemHeight;
            }
        }
        private float itemWidth;
        private float itemHeight;
        [HideInInspector][SerializeField] private RectTransform rectT;
        private List<RectTransform> items = new List<RectTransform>();
        public RectTransform SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != null)
                {
                    selectedItem.transform.SetSiblingIndex(items.IndexOf(selectedItem));
                }
                selectedItem = value;
                if (selectedItem != null)
                {
                    selectedItem.transform.SetAsLastSibling();
                }
                if (gridController != null)
                {
                    gridController.GridItemSelected(this, selectedItem);
                }
                SelectedItemChanged?.Invoke(selectedItem);
            }
        }
        private RectTransform selectedItem;

        public bool CanAcceptItem
        {
            get
            {
                return AllowRecieving && (items.Count < MaxItems || MaxItems <= 0);
            }
        }
        // private DrivenRectTransformTracker tracker;

        public event Action<RectTransform, DragAndDropGrid> ReceivedItem;
        public event Action<RectTransform, DragAndDropGrid> SentItem;
        public event Action<RectTransform> SelectedItemChanged;


        protected new void Reset()
        {
            rectT = GetComponent<RectTransform>();
            //tracker.Add(this, rectT, DrivenTransformProperties.SizeDelta);
        }
        protected new void Awake()
        {
            if (Application.isPlaying)
            {
                var children = GetChildren();
                if (children != null && children.Count > 0)
                {
                    foreach (var child in children)
                    {
                        AddItem(child);
                    }
                }
            }           
        }
        protected new void OnValidate()
        {
            SpawnEmptySlots();
        }
        private void Update()
        {
            if (!Application.isPlaying)
            {
                //ValidateChildren();
                //Refresh();
                //Debug.Log(GetRowCount() + " - Col: " + GetColumnCount());
                return;
            }

            if (ControlMode == ControlMode.Calculations && Input.GetMouseButtonDown(0) && IsInsideGrid())
            {
                int index = GetIndexOfMouse();
                if (index < items.Count)
                {
                    SelectedItem = items[index];
                }
            }
            else if (SelectedItem != null)
            {
                if (Input.GetMouseButton(0))
                {
                    if (IsInsideGrid())
                    {
                        items.Remove(selectedItem);
                        InsertItem(selectedItem);
                    }
                    DragItem();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    SelectedItem = null;
                }
            }
        }

        private List<RectTransform> GetChildren()
        {
            var results = new List<RectTransform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var result = transform.GetChild(i);
                if (emptySpaceSpawner.Any(x => x.gameObject == result.gameObject))
                {
                    continue;
                }
                results.Add(result.GetComponent<RectTransform>());
            }          
            return results;
        }
        public void RemoveItem(RectTransform rectT)
        {
            if (SelectedItem == rectT)
            {
                SelectedItem = null;
            }
            items.Remove(rectT);
        }
        public List<RectTransform> GetItems()
        {
            List<RectTransform> results = new List<RectTransform>();
            results.AddRange(items);
            return results;
        }
        public void AddItems(List<RectTransform> items)
        {
            foreach (var rectT in items)
            {
                items.Add(rectT);
                rectT.SetParent(transform);
                rectT.SetAsLastSibling();
            }
        }
        public void AddItem(RectTransform rectT)
        {
            if (ControlMode == ControlMode.Buttons)
            {
                DragAndDropItem item = rectT.GetComponent<DragAndDropItem>();
                if (item == null)
                {
                    Debug.LogWarning("Missing Button Component");
                    return;
                }
                item.Grid = this;
            }
            items.Add(rectT);
            rectT.SetParent(transform);
            rectT.SetAsLastSibling();
        }
        public void InsertItem(RectTransform rectT, bool check = false)
        {
            items.Insert(GetIndexOfMouse(), rectT);
        }
        public bool ContainsItem(RectTransform rectT)
        {
            return items.Contains(rectT);
        }
        public void ReceiveItem(RectTransform item, DragAndDropGrid other)
        {
            item.SetParent(transform);
            ReceivedItem?.Invoke(item, other);
        }
        public void SendItem(RectTransform item, DragAndDropGrid other)
        {
            RemoveItem(item);
            SentItem?.Invoke(item, other);
        }
        public void ItemClicked(RectTransform item)
        {
            SelectedItem = item;
        }
        private int GetColumnCount()
        {
            int itemCount = GetTotalItems();
            switch (Constrain)
            {
                case Constrain.Column:
                    if (GetRowCount() == 1)
                    {
                        return Mathf.Min(itemCount, MaxLength);
                    }
                    else return Mathf.Max((int)((itemCount) / (float)GetRowCount() + 0.99f), MaxLength);
                case Constrain.Row:
                    return (MaxLength - 1 + itemCount) / MaxLength;
                default:
                    return 0;
            }
        }
        private int GetRowCount()
        {
            int itemCount = GetTotalItems();
            switch (Constrain)
            {
                case Constrain.Column:
                    return (MaxLength - 1 + itemCount) / MaxLength;
                case Constrain.Row:
                    if (GetColumnCount() == 1)
                    {
                        return Mathf.Min(itemCount, MaxLength);
                    }
                    else return Mathf.Max((int)((itemCount) / (float)GetColumnCount() + 0.99f), MaxLength);
                default:
                    return 0;
            }
        }
        private void DragItem()
        {
            if (SelectedItem != null)
            {
                Vector3 newPosition = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
                newPosition.z = SelectedItem.transform.position.z;
                SelectedItem.transform.position = newPosition;
            }
        }
        private void RecalculateItemSize()
        {
            if (!Application.isPlaying)
            {
                RecalculateItemSize(GetChildren());
            }
            else RecalculateItemSize(items);
        }
        private void RecalculateItemSize(List<RectTransform> targets)
        {
            if (OverwriteWidth > 0)
            {
                itemWidth = OverwriteWidth;
            }
            else if (targets.Count > 0)
            {
                itemWidth = targets.Max(x => x.sizeDelta.x);
            }

            if (OverwriteHeight > 0)
            {
                itemHeight = OverwriteHeight;
            }
            else if (targets.Count > 0)
            {
                itemHeight = targets.Max(x => x.sizeDelta.y);
            }
        }
        public void ResizeTransform()
        {
            if (rectT == null)
            {
                return;
            }
            if (GetTotalItems() == 0)
            {
                rectT.sizeDelta = Vector2.zero;
            }
            else rectT.sizeDelta
                    = new Vector2(GetColumnCount() * TotalSpacingX, GetRowCount() * TotalSpacingY)
                    - new Vector2(SpacingX, SpacingY)
                    + new Vector2(padding.left + padding.right, padding.top + padding.bottom);
        }
        private void SpawnEmptySlots()
        {          
            emptySpaceSpawner.DestroyAll();

            if (!AddEmptySpace)
            {
                return;
            }

            int slotsToSpawn = Mathf.Min(MaxItems, MinItems - GetTotalItems());

            for (int i = 0; i < slotsToSpawn; i++)
            {
                emptySpaceSpawner.Spawn();
            }
        }
        public bool IsInsideGrid()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rectT, Input.mousePosition, Camera.main))
            {
                return true;
            }
            return false;
        }
        private int GetIndexOfMouse()
        {
            Vector2 mousePos = Input.mousePosition;
            //mousePos = rectT.InverseTransformPoint(mousePos);
            //mousePos.Scale(rectT.lossyScale);
            //Debug.Log(mousePos);
            //Rect rect = rectT.rect;
            //rect.mi = new Vector2(0f, 1f);
            //rect.max = new Vector2(0f, 1f);
            Vector3[] corners = new Vector3[4];
            rectT.GetWorldCorners(corners);
            Vector2 topRightCorner = Camera.main.WorldToScreenPoint(corners[1]);

            //Debug.Log(topRightCorner);
            //Vector2 topRightCorner = Camera.main.WorldToScreenPoint(transform.position);
            //topRightCorner.x -= rectT.sizeDelta.x / 2f - BorderX / 2f;
            //topRightCorner.y += rectT.sizeDelta.y / 2f - BorderY / 2f;

            Vector2 difference = mousePos - topRightCorner;
            //Debug.Log(topRightCorner + " - " + mousePos + " - " + difference);
            //Vector2 difference = mousePos;
            int index = 0;
            index += Mathf.Min(GetColumnCount(), (int)((difference.x) / TotalSpacingX));
            index += Mathf.Min(GetRowCount(), (int)(-difference.y / TotalSpacingY)) * MaxLength;
            //index += (int)(-difference.y / TotalSpacingY) * MaxLength;
            //Debug.Log(topRightCorner + " " + mousePos);
            index = Mathf.Min(index, items.Count);
            //Debug.Log(index);
            return index;
        }

        public int GetTotalItems()
        {
            if (Application.isPlaying)
            {
                return rectChildren.Count;
            }
            else return GetChildren().Count;
        }

        public override void CalculateLayoutInputVertical()
        {
            //Debug.Log("CalculateLayoutInputVertical");
            RecalculateItemSize();
            int posX = 0;
            int posY = 0;
            for (int i = 0; i < GetTotalItems(); i++)
            {
                RectTransform child = rectChildren[i];
                SetChildAlongAxis(child, 0, TotalSpacingX * posX + padding.left);
                SetChildAlongAxis(child, 1, TotalSpacingY * posY + padding.top);
                if (Constrain == Constrain.Column)
                {
                    if (posX > MaxLength - 2)
                    {
                        posX = 0;
                        posY++;
                    }
                    else posX++;
                }
                else if (Constrain == Constrain.Row)
                {
                    if (posY > MaxLength - 2)
                    {
                        posY = 0;
                        posX++;
                    }
                    else posY++;
                }
            }
        }
        public override void SetLayoutHorizontal()
        {
            //Debug.Log("SetLayoutHorizontal");
            ResizeTransform();
        }

        public override void SetLayoutVertical()
        {
            //Debug.Log("SetLayoutVertical");
            ResizeTransform();
        }

        //public override void SetLayoutHorizontal()
        //{
        //    Vector2 sizeDelta = rectT.sizeDelta;
        //    sizeDelta.x = LayoutUtility.GetPreferredWidth(rectT);
        //    rectT.sizeDelta = sizeDelta;
        //    Debug.Log("hi");
        //}

        //public override void SetLayoutVertical()
        //{
        //    Vector2 sizeDelta = rectT.sizeDelta;
        //    sizeDelta.y = LayoutUtility.GetPreferredHeight(rectT);
        //    rectT.sizeDelta = sizeDelta;
        //}
    }
}
