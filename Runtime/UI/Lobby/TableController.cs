using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.ContentSizeFitter;

namespace HexTecGames.Basics.UI
{
    public class TableController : LayoutGroup
    {
        [SerializeField] private HorizontalOrVerticalLayoutGroup parentLayout = default;
        [SerializeField] private List<HorizontalOrVerticalLayoutGroup> contentItems = default;
        [Space]
        [SerializeField] private bool useHeader = default;
        [SerializeField, DrawIf(nameof(useHeader), true)] private HorizontalOrVerticalLayoutGroup header = default;
        [Space]
        [SerializeField] private FitMode fitMode = default;

        private DrivenRectTransformTracker drivingTracker;

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

        private Orientation Orientation
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

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            parentLayout = GetComponent<HorizontalOrVerticalLayoutGroup>();
            header = FindHeader();
            contentItems = FindContentItems();
            Refresh();
        }
#endif

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            //SetDrivingState();
            SetLayoutGroupValues();
            SetCellValues();
        }
        private void SetDrivingState()
        {
            drivingTracker.Clear();
            DrivenTransformProperties drivenProperties = Orientation == Orientation.Vertical ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY;
            foreach (var item in contentItems)
            {
                for (int i = 0; i < item.transform.childCount; i++)
                {
                    drivingTracker.Add(this, item.transform.GetChild(i).GetComponent<RectTransform>(), drivenProperties);
                }
            }
        }
        private List<HorizontalOrVerticalLayoutGroup> FindContentItems()
        {
            if (Parent == null || Parent.childCount <= 1)
            {
                return null;
            }
            List<HorizontalOrVerticalLayoutGroup> results = new List<HorizontalOrVerticalLayoutGroup>();
            for (int i = 1; i < Parent.childCount; i++)
            {
                if (Parent.GetChild(i).TryGetComponent(out HorizontalOrVerticalLayoutGroup layoutGroup))
                {
                    results.Add(layoutGroup);
                }
            }
            return results;
        }

        private void SetLayoutGroupValues()
        {
            if (contentItems == null || contentItems.Count == 0)
            {
                return;
            }
            HorizontalOrVerticalLayoutGroup leader;

            if (useHeader && header != null)
            {
                leader = header;
            }
            else leader = contentItems[0];

            if (leader.TryGetComponent(out HorizontalOrVerticalLayoutGroup leaderGroup))
            {
                foreach (var item in contentItems)
                {
                    if (item != null && item.TryGetComponent(out HorizontalOrVerticalLayoutGroup contentGroup))
                    {
                        contentGroup.CopyData(leaderGroup);
                    }
                }
            }
        }
        private void SetCellValues()
        {
            if (contentItems == null || contentItems.Count == 0)
            {
                return;
            }
            if (fitMode == FitMode.Unconstrained)
            {
                return;
            }

            List<HorizontalOrVerticalLayoutGroup> allItems = new List<HorizontalOrVerticalLayoutGroup>(contentItems);
            if (useHeader && header != null)
            {
                allItems.Add(header);
            }

            int totalRows = allItems[0].transform.childCount;
            Orientation orientation = Orientation;

            for (int i = 0; i < totalRows; i++)
            {
                float minSize = 0;
                foreach (var item in allItems)
                {
                    float result = 0;
                    if (orientation == Orientation.Vertical)
                    {
                        if (fitMode == FitMode.MinSize)
                        {
                            result = LayoutUtility.GetMinWidth(item.transform.GetChild(i).GetComponent<RectTransform>());
                        }
                        else if (fitMode == FitMode.PreferredSize)
                        {
                            result = LayoutUtility.GetPreferredWidth(item.transform.GetChild(i).GetComponent<RectTransform>());
                        }
                    }
                    else
                    {
                        if (fitMode == FitMode.MinSize)
                        {
                            result = LayoutUtility.GetMinHeight(item.transform.GetChild(i).GetComponent<RectTransform>());
                        }
                        else if (fitMode == FitMode.PreferredSize)
                        {
                            result = LayoutUtility.GetPreferredHeight(item.transform.GetChild(i).GetComponent<RectTransform>());
                        }
                    }
                    if (minSize < result)
                    {
                        minSize = result;
                    }
                }
                foreach (var item in allItems)
                {
                    RectTransform result = item.transform.GetChild(i).GetComponent<RectTransform>();
                    if (orientation == Orientation.Vertical)
                    {
                        result.sizeDelta = new Vector2(minSize, result.sizeDelta.y);
                    }
                    else
                    {
                        result.sizeDelta = new Vector2(result.sizeDelta.x, minSize);
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

        public override void CalculateLayoutInputVertical()
        {
            Refresh();
        }

        public override void SetLayoutHorizontal()
        {
            Refresh();
        }

        public override void SetLayoutVertical()
        {
            Refresh();
        }
    }
}