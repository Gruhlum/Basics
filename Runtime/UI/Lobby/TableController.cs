using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class TableController : MonoBehaviour
    {
        [SerializeField] private HorizontalOrVerticalLayoutGroup parentLayout = default;
        [SerializeField] private HorizontalOrVerticalLayoutGroup header = default;
        [SerializeField] private List<LayoutGroup> contentItems = default;

        private Transform Parent
        {
            get
            {
                if (parentLayout == null)
                {
                    return null;
                }
                return parentLayout.transform;
            }
        }

        private Orientation IsVertical
        {
            get
            {
                if (parentLayout != null)
                {
                    if (parentLayout is VerticalLayoutGroup)
                    {
                        return Orientation.Vertical;
                    }
                }
                return Orientation.Horizontal;
            }
        }


        private void Reset()
        {
            parentLayout = GetComponent<HorizontalOrVerticalLayoutGroup>();
            header = FindHeader();
            contentItems = FindContentItems();
            Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            SetLayoutGroupValues();
            SetCellValues();
        }

        private List<LayoutGroup> FindContentItems()
        {
            if (Parent == null || Parent.childCount <= 1)
            {
                return null;
            }
            List<LayoutGroup> results = new List<LayoutGroup>();
            for (int i = 1; i < Parent.childCount; i++)
            {
                if (Parent.GetChild(i).TryGetComponent(out LayoutGroup layoutGroup))
                {
                    results.Add(layoutGroup);
                }
            }
            return results;
        }

        private void SetLayoutGroupValues()
        {
            if (header.TryGetComponent(out HorizontalOrVerticalLayoutGroup headerGroup))
            {
                foreach (var item in contentItems)
                {
                    if (item.TryGetComponent(out HorizontalOrVerticalLayoutGroup contentGroup))
                    {
                        contentGroup.CopyData(headerGroup);
                    }
                }
            }
        }
        private void SetCellValues()
        {
            if (contentItems == null)
            {
                return;
            }

            Orientation orientation = IsVertical;

            for (int i = 0; i < header.transform.childCount; i++)
            {
                Transform headerCell = header.transform.GetChild(i);
                if (headerCell.TryGetComponent(out LayoutElement headerLayout))
                {
                    foreach (var contentItem in contentItems)
                    {
                        Transform contentCell = contentItem.transform.GetChild(i);
                        if (contentCell.TryGetComponent(out LayoutElement contentLayout))
                        {
                            contentLayout.CopyData(headerLayout, orientation);
                        }
                    }
                }
            }
        }

        private HorizontalOrVerticalLayoutGroup FindHeader()
        {
            if (Parent == null || Parent.childCount <= 0)
            {
                return null;
            }
            if (Parent.GetChild(0).TryGetComponent(out HorizontalOrVerticalLayoutGroup layoutGroup))
            {
                return layoutGroup;
            }
            return null;
        }
    }
}