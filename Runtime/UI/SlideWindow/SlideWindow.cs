using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace HexTecGames.Basics.UI
{
    public class SlideWindow : AdvancedBehaviour
    {
        [SerializeField] private Image img = default;
        [SerializeField] private TMP_Text titleGUI = default;
        [SerializeField] private TMP_Text descriptionGUI = default;
        [SerializeField] private List<KeyCode> advanceKeyCodes = default;

        [SerializeField] private List<SlideData> pages = default;
        [SerializeField] private Spawner<PageDot> pageDotSpawner = default;

        [Space]
        [SerializeField] private bool playOnAwake = true;

        private List<PageDot> pageDots;
        private int currentPageIndex = 0;


        private void Awake()
        {
            if (playOnAwake)
            {
                InitializeTutorial();
            }
        }

        private void Update()
        {
            foreach (var keyCode in advanceKeyCodes)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    ShowNextPage();
                    break;
                }
            }
        }

        private void InitializeTutorial()
        {
            pageDots = pageDotSpawner.Spawn(pages.Count);
            currentPageIndex = 0;
            UpdatePageDots();
            ShowPage(pages[0]);
            gameObject.SetActive(true);
        }

        private void UpdatePageDots()
        {
            for (int i = 0; i < pageDots.Count; i++)
            {
                pageDots[i].SetActive(i == currentPageIndex);
            }
        }

        public void FinishTutorial()
        {
            gameObject.SetActive(false);
        }

        public void ShowNextPage()
        {
            currentPageIndex++;
            if (currentPageIndex >= pages.Count)
            {
                FinishTutorial();
                return;
            }
            UpdatePageDots();
            ShowPage(pages[currentPageIndex]);
        }

        private void ShowPage(SlideData page)
        {
            img.sprite = page.screenshot;
            titleGUI.text = page.title;
            descriptionGUI.text = page.description;
        }
    }
}