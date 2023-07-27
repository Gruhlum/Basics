using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    public enum Constrain { Column, Row };
    public enum Alignment { TopLeft, TopRight, BottomLeft, BottomRight };
    public enum ControlMode { Calculations, Buttons }
    [ExecuteAlways]
    public class DragAndDropGrid : MonoBehaviour
    {
        public Constrain Constrain;

        [Min(1)]
        public int MaxLength = 3;
        //TODO: public bool AllowRearranging;

        [Header("Other Grids")]
        public GridController gridController = default;
        public bool AllowSending = true;
        public bool AllowRecieving = true;

        [Header("Size")]
        public Alignment Alignment;

        public int SpacingX;
        public int SpacingY;

        public int BorderX;
        public int BorderY;

        [Header("Items")]
        public ControlMode ControlMode;

        public int OverwriteWidth;
        public int OverwriteHeight;

        public int MinItems;
        public int MaxItems;

        public bool AddEmptySpace;

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
                return AllowSending && (items.Count < MaxItems || MaxItems <= 0);
            }
        }
        [SerializeField] private RectTransform rectT;
        private DrivenRectTransformTracker tracker;

        public event Action<RectTransform, DragAndDropGrid> ReceivedItem;
        public event Action<RectTransform, DragAndDropGrid> SentItem;
        public event Action<RectTransform> SelectedItemChanged;
        private void Reset()
        {
            rectT = GetComponent<RectTransform>();
            tracker.Add(this, rectT, DrivenTransformProperties.SizeDelta);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                //TODO: check why this fucks it up
                //Refresh();
                UpdateItemSize();
                ResizeTransform();
                UpdateGridItems();
            }
        }
        private void Awake()
        {
            if (Application.isPlaying)
            {
                tracker.Add(this, rectT, DrivenTransformProperties.SizeDelta);
            }
            Refresh();
        }

        private void Update()
        {
            if (Application.isPlaying == false)
            {
                ValidateChildren();
                Refresh();
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
                    UpdateGridItems();
                }
            }
        }
        public void RemoveItem(RectTransform rectT)
        {           
            if (SelectedItem == rectT)
            {
                SelectedItem = null;
            }
            items.Remove(rectT);
            Refresh();
        }
        public void AddItems(List<RectTransform> items)
        {
            foreach (var rectT in items)
            {
                items.Add(rectT);
                rectT.SetParent(transform);
                rectT.SetAsLastSibling();
            }
            Refresh();
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
            Refresh();
        }
        public void InsertItem(RectTransform rectT, bool check = false)
        {
            items.Insert(GetIndexOfMouse(), rectT);
            if (!check)
            {
                ResizeTransform();               
            }
            UpdateGridItems();
        }
        public bool ContainsItem(RectTransform rectT)
        {
            return items.Contains(rectT);
        }
        public void ReceiveItem(RectTransform item, DragAndDropGrid other)
        {
            item.SetParent(transform);
            ResizeTransform();
            UpdateGridItems();
            ReceivedItem?.Invoke(item, other);
        }
        public void SendItem(RectTransform item, DragAndDropGrid other)
        {
            RemoveItem(item);
            ResizeTransform();
            UpdateGridItems();
            SentItem?.Invoke(item, other);
        }
        public void ItemClicked(RectTransform item)
        {
            SelectedItem = item;
        }
        private int GetColumnCount(int min = 0)
        {
            int extra = AddEmptySpace ? 1 : 0;
            switch (Constrain)
            {
                case Constrain.Column:
                    if (GetRowCount() == 1)
                    {
                        return Mathf.Min(items.Count + extra, MaxLength);
                    }
                    else return Mathf.Max((int)((items.Count + extra) / (float)GetRowCount() + 0.99f), MaxLength);
                case Constrain.Row:
                    return (MaxLength - 1 + Mathf.Max(min, items.Count) + extra) / MaxLength;
                default:
                    return 0;
            }
        }
        private int GetRowCount(int min = 0)
        {
            int extra = AddEmptySpace ? 1 : 0;
            switch (Constrain)
            {
                case Constrain.Column:
                    return (MaxLength - 1 + Mathf.Max(min, items.Count) + extra) / MaxLength;
                case Constrain.Row:
                    if (GetColumnCount() == 1)
                    {
                        return Mathf.Min(items.Count + extra, MaxLength);
                    }
                    else return Mathf.Max((int)((items.Count + extra) / (float)GetColumnCount() + 0.99f), MaxLength);
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
        private void Refresh()
        {
            UpdateItemSize();
            ResizeTransform();
            UpdateGridItems();
        }
        private void UpdateItemSize()
        {
            if (OverwriteWidth > 0)
            {
                itemWidth = OverwriteWidth;
            }
            else if (items.Count > 0)
            {
                itemWidth = items.Max(x => x.sizeDelta.x);
            }

            if (OverwriteHeight > 0)
            {
                itemHeight = OverwriteHeight;
            }
            else if (items.Count > 0)
            {
                itemHeight = items.Max(x => x.sizeDelta.y);
            }
        }
        public void ResizeTransform()
        {
            if (rectT == null)
            {
                return;
            }
            if (items.Count == 0)
            {
                rectT.sizeDelta = Vector2.zero;
            }
            else rectT.sizeDelta = new Vector2(GetColumnCount(MinItems) * TotalSpacingX + BorderX, GetRowCount(MinItems) * TotalSpacingY + BorderY);
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

        private void ValidateChildren()
        {
            items.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform item = transform.GetChild(i).GetComponent<RectTransform>();
                items.Add(item);
            }
        }
        public void UpdateGridItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                //if (i == 6)
                //{
                //    Debug.Log(CalculateItemPosition(i) + " - " + (Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f))));
                //    Debug.Log(Screen.width + " - " + Screen.height);
                //}
                items[i].anchoredPosition = (CalculateItemPosition(i) / items[i].transform.lossyScale);
            }
        }

        private Vector2 GetAlignmentPosition(int index)
        {
            switch (Alignment)
            {
                case Alignment.TopLeft:
                    return new Vector2(TotalSpacingX * (index % GetColumnCount()), -TotalSpacingY * (index / GetColumnCount()))
                        + new Vector2(-rectT.sizeDelta.x + TotalSpacingX + BorderX, rectT.sizeDelta.y - TotalSpacingY - BorderY) / 2f;
                case Alignment.TopRight:
                    return new Vector2(-TotalSpacingX * (index % GetColumnCount()), -TotalSpacingY * (index / GetColumnCount()))
                        + new Vector2(rectT.sizeDelta.x - TotalSpacingX - BorderX, rectT.sizeDelta.y - TotalSpacingY - BorderY) / 2f;
                case Alignment.BottomLeft:
                    return new Vector2(TotalSpacingX * (index % GetColumnCount()), TotalSpacingY * (index / GetColumnCount()))
                        + new Vector2(-rectT.sizeDelta.x + TotalSpacingX + BorderX, -rectT.sizeDelta.y + TotalSpacingY + BorderY) / 2f;
                case Alignment.BottomRight:
                    return new Vector2(-TotalSpacingX * (index % GetColumnCount()), TotalSpacingY * (index / GetColumnCount()))
                        + new Vector2(rectT.sizeDelta.x - TotalSpacingX - BorderX, -rectT.sizeDelta.y + TotalSpacingY + BorderY) / 2f;
                default:
                    return Vector2.zero;
            }
        }

        private Vector2 CalculateItemPosition(int index)
        {
            Vector2 pos = GetAlignmentPosition(index);

            return Camera.main.ScreenToWorldPoint(pos + new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f));
        }
    }
}
