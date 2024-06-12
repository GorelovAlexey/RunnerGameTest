using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerObject : MonoBehaviour
    {

        public SkinTransformDictionary skinTraformDictionary;

        public ReactiveProperty<PlayerMoneySkinKey> CurrentSkinKey = new ReactiveProperty<PlayerMoneySkinKey>(PlayerMoneySkinKey.Decent);
        public ReactiveProperty<int> MoneyCount = new ReactiveProperty<int>(60);
        public int MaxMoney { get; private set; }

        public List<MoneySkinThreshold> moneySkinTrhesholds;


        private PlayerMoneyPanel _moneyPanel;
        public PlayerMoneyPanel MoneyPanel => _moneyPanel ??= GetComponentInChildren<PlayerMoneyPanel>();


        private PlayerSlideController _slideController;
        public PlayerSlideController PlayerSlideController => _slideController ??= GetComponentInParent<PlayerSlideController>();


        private PlayerAnimationController _playerAnimationController;
        private PlayerAnimationController AnimationController => _playerAnimationController ??= GetComponentInChildren<PlayerAnimationController>();

        private void Awake()
        {
            moneySkinTrhesholds = moneySkinTrhesholds.OrderBy(x => x.key).ToList();
            MaxMoney = moneySkinTrhesholds.Max(x => x.moneyMin);

            MoneyCount.Subscribe(UpdateMoney).AddTo(this);
            CurrentSkinKey.StartWith(CurrentSkinKey.Value)
                .Subscribe(x => UpdateSkin(x)).AddTo(this);

            UpdateMoney(MoneyCount.Value);

            MoneyPanel?.SetupSlider(this);

            MoneyCount.Where(x => x <= 0).Subscribe(x =>
            {
                GameManager.Instance?.LevelFail();
            });
        }

        private void UpdateMoney(int money)
        {
            if (money > MaxMoney)
            {
                MoneyCount.Value = MaxMoney;
                return;
            }
            if (money < 0)
            {
                MoneyCount.Value = 0;
                return;
            }

            var skin = CalculateCurrentMoneyKey(money);
            CurrentSkinKey.Value = skin;
        }

        private PlayerMoneySkinKey CalculateCurrentMoneyKey(int money)
        {
            var key = moneySkinTrhesholds[0];
            for (var i = 1; i < moneySkinTrhesholds.Count; i++)
            {
                var t = moneySkinTrhesholds[i];
                if (money < t.moneyMin)
                    break;

                key = t;
            }

            return key.key;
        }

        private void UpdateSkin(PlayerMoneySkinKey skin, bool force = false)
        {
            if (force)
                CurrentSkinKey.SetValueAndForceNotify(skin);
            else
                CurrentSkinKey.Value = skin;

            foreach (var (k, t) in skinTraformDictionary)
            {
                t.gameObject.SetActive(skin == k);
            }
        }

        public void Lose()
        {
            if (PlayerSlideController)
                PlayerSlideController.CanMove = false;
        }

        public void Win()
        {
            _moneyPanel?.gameObject.SetActive(false);

            if (AnimationController)
                AnimationController.Dance = true;

            if (PlayerSlideController)
                PlayerSlideController.CanMove = false;
        }

        public void FactoryReset()
        {
            _moneyPanel?.gameObject.SetActive(true);
            MoneyCount.Value = 60;
            if (AnimationController)
                AnimationController.Dance = false;

            if (PlayerSlideController)
                PlayerSlideController.CanMove = false;
        }

        public void StartGame()
        {
            if (PlayerSlideController)
                PlayerSlideController.CanMove = true;
        }
    }
}