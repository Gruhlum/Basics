using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public abstract class PreSeededDisplayController<D, T> : DisplayControllerBase<D, T> where D : Display<D, T>
    {
        [SerializeField] protected List<D> displays = default;

        protected virtual void Reset()
        {
            displays = GetComponentsInChildren<D>().ToList();
        }

        private void Awake()
        {
            foreach (var display in displays)
            {
                AddDisplayEvents(display);
            }
        }

        public void SelectFirstDisplay()
        {
            if (displays == null || displays.Count <= 0)
            {
                return;
            }
            displays[0].DisplayClicked();
        }
        
        public List<D> GetDisplays()
        {
            if (displays != null && displays.Count > 0)
            {
                return displays;
            }
            return null;
        }
    }
}