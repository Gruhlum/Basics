using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Events;

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

        public UnityEvent OnNextSlide;
        public UnityEvent OnStarted;
        public UnityEvent OnEnded;


        private void Awake()
        {
            if (playOnAwake)
            {
                StartSlide();
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

        public void StartSlide()
        {
            pageDots = pageDotSpawner.Spawn(pages.Count);
            currentPageIndex = 0;
            UpdatePageDots();
            ShowPage(pages[0]);
            gameObject.SetActive(true);
            OnStarted?.Invoke();
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
            OnEnded?.Invoke();
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
            OnNextSlide?.Invoke();
        }

        private void ShowPage(SlideData page)
        {
            img.sprite = page.screenshot;
            titleGUI.text = page.title;
            descriptionGUI.text = page.description;
        }
    }
}