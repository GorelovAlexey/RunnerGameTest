using Assets.Scripts.Player;
using System;
using System.Collections;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMoneyCounter;
        [SerializeField] private TextMeshProUGUI textLevelTitle;

        [SerializeField] private GameObject RestartPanel;
        [SerializeField] private Button restartButton;

        [SerializeField] private GameObject WinPanel;
        [SerializeField] private Button advanceButton;

        [SerializeField] public PlayerDragControlls DragControlls;

        [SerializeField] private GameObject HintConrolls;

        private void Start()
        {
        }


        IDisposable moneyUpdateSub;
        public void PreLevelStart(int level, PlayerObject player)
        {
            SetHint(true);

            textLevelTitle.text = $"Level {level}";
            moneyUpdateSub?.Dispose();

            moneyUpdateSub = player.MoneyCount.Subscribe(x => textLevelTitle.text = x.ToString())
                .AddTo(player.gameObject).AddTo(gameObject);
        }

        public void SetHint(bool active) => HintConrolls.SetActive(active);

        IDisposable hintHideSub;
        public void SetInGameState(bool ingame)
        {
            hintHideSub?.Dispose();

            textMoneyCounter.gameObject.SetActive(ingame == true);
            textLevelTitle.gameObject.SetActive(ingame == true);
            DragControlls.gameObject.SetActive(ingame == true);

            hintHideSub = DragControlls.DragStart.Where(x => x != false).Subscribe(x =>
            {
                HintConrolls?.SetActive(false);
                Debug.Log("CanvasDone " + x);

            }).AddTo(gameObject);


            if (ingame == true) 
            {
                RestartPanel.SetActive(ingame == false);
                WinPanel.SetActive(ingame == false);
            };
        }

        public void SetupMoneyCounter(PlayerObject player) {

            player.MoneyCount.Subscribe(x => textMoneyCounter.text = x.ToString())
                .AddTo(gameObject).AddTo(player.gameObject);
        }

        public void ShowFail(Action restart)
        {
            moneyUpdateSub?.Dispose();
            RestartPanel.SetActive(true);
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() => restart.Invoke());
        }

        public void ShowWin(Action advance)
        {
            moneyUpdateSub?.Dispose();
            WinPanel.SetActive(true);
            advanceButton.onClick.RemoveAllListeners();
            advanceButton.onClick.AddListener(() => advance.Invoke());
        }
    }
}