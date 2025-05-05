using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.ContentSizeFitter;

namespace HexTecGames.Basics.UI
{
    public class TableController : HorizontalOrVerticalLayoutGroup
    {
        [SerializeField] private List<HorizontalOrVerticalLayoutGroup> contentItems = default;
        [Space]
        [SerializeField] private bool useHeader = default;
        [SerializeField, DrawIf(nameof(useHeader), true)] private HorizontalOrVerticalLayoutGroup header = default;
        [Space]
        [SerializeField] private FitMode fitMode = default;

        private DrivenRectTransformTracker drivingTracker;

        private Orientation Orientation
        {
            get
            {
                return orientation;
            }
        }
        [SerializeField] private Orientation orientation = default;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            if (useHeader)
            {
                header = FindHeader();
            }
            contentItems = FindContentItems();
            Refresh();
        }
        protected override void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            int lastCount = 0;
            if (contentItems != null)
            {
                lastCount = contentItems.Count;
            }
            
            contentItems = FindContentItems();
            if (contentItems != null && lastCount != contentItems.Count)
            {
                LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
            }
            base.Update();
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
            if (transform.childCount <= 0)
            {
                return null;
            }
            List<HorizontalOrVerticalLayoutGroup> results = new List<HorizontalOrVerticalLayoutGroup>();

            int startIndex = useHeader ? 1 : 0;

            for (int i = startIndex; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out HorizontalOrVerticalLayoutGroup layoutGroup))
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

            List<HorizontalOrVerticalLayoutGroup> allItems = new List<HorizontalOrVerticalLayoutGroup>(contentItems);
            if (useHeader && header != null)
            {
                allItems.Add(header);
            }

            HorizontalOrVerticalLayoutGroup leader = allItems[0];
            int totalRows = leader.transform.childCount;
            Orientation orientation = Orientation;

            for (int i = 0; i < totalRows; i++)
            {
                float minSize = CalculateTargetSize(allItems, i);
                ApplyTargetSize(allItems, orientation, i, minSize);
            }
        }

        private void ApplyTargetSize(List<HorizontalOrVerticalLayoutGroup> allItems, Orientation orientation, int index, float minSize)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (fitMode == FitMode.Unconstrained && i == 0)
                {
                    continue;
                }
                HorizontalOrVerticalLayoutGroup item = allItems[i];
                RectTransform result = item.transform.GetChild(index).GetComponent<RectTransform>();
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

        private float CalculateTargetSize(List<HorizontalOrVerticalLayoutGroup> allItems, int index)
        {
            if (fitMode == FitMode.Unconstrained)
            {
                if (Orientation == Orientation.Vertical)
                {
                    return allItems[0].transform.GetChild(index).GetComponent<RectTransform>().sizeDelta.x;
                }
                else return allItems[0].transform.GetChild(index).GetComponent<RectTransform>().sizeDelta.y;
            }
            else
            {
                float minSize = 0;

                foreach (var item in allItems)
                {
                    float result = 0;
                    if (orientation == Orientation.Vertical)
                    {
                        if (fitMode == FitMode.MinSize)
                        {
                            result = LayoutUtility.GetMinWidth(item.transform.GetChild(index).GetComponent<RectTransform>());
                        }
                        else if (fitMode == FitMode.PreferredSize)
                        {
                            result = LayoutUtility.GetPreferredWidth(item.transform.GetChild(index).GetComponent<RectTransform>());
                        }
                    }
                    else
                    {
                        if (fitMode == FitMode.MinSize)
                        {
                            result = LayoutUtility.GetMinHeight(item.transform.GetChild(index).GetComponent<RectTransform>());
                        }
                        else if (fitMode == FitMode.PreferredSize)
                        {
                            result = LayoutUtility.GetPreferredHeight(item.transform.GetChild(index).GetComponent<RectTransform>());
                        }
                    }
                    if (minSize < result)
                    {
                        minSize = result;
                    }
                }
                return minSize;
            }
        }

        private HorizontalOrVerticalLayoutGroup FindHeader()
        {
            if (transform.childCount <= 0)
            {
                return null;
            }
            if (transform.GetChild(0).TryGetComponent(out HorizontalOrVerticalLayoutGroup layoutGroup))
            {
                return layoutGroup;
            }
            return null;
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            Refresh();
            CalcAlongAxis(0, Orientation == Orientation.Vertical);
        }
        public override void CalculateLayoutInputVertical()
        {
            Refresh();
            CalcAlongAxis(1, Orientation == Orientation.Vertical);
        }
        public override void SetLayoutHorizontal()
        {
            Refresh();
            SetChildrenAlongAxis(0, Orientation == Orientation.Vertical);
        }
        public override void SetLayoutVertical()
        {
            Refresh();
            SetChildrenAlongAxis(1, Orientation == Orientation.Vertical);
        }
    }
}