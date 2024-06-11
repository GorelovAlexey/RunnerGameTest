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
        PlayerSlideController slideController;
        PlayerMoneyPanel panel;

        public SkinTransformDictionary skinTraformDictionary;

        public ReactiveProperty<PlayerMoneySkinKey> CurrentSkinKey = new ReactiveProperty<PlayerMoneySkinKey>(PlayerMoneySkinKey.Decent);
        public ReactiveProperty<int> MoneyCount = new ReactiveProperty<int>(60);

        public List<MoneySkinThreshold> moneySkinTrhesholds;

        public PlayerSlideController PlayerSlideController => slideController;

        private void Awake()
        {
            panel = GetComponentInParent<PlayerMoneyPanel>();
            panel?.SetupSlider(this);

            moneySkinTrhesholds = moneySkinTrhesholds.OrderBy(x => x.key).ToList();

            MoneyCount.Subscribe(UpdateMoney).AddTo(this);
            CurrentSkinKey.StartWith(CurrentSkinKey.Value)
                .Subscribe(x => UpdateSkin(x)).AddTo(this);

            UpdateMoney(MoneyCount.Value);
        }

        private void UpdateMoney(int money)
        {
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

        // Start is called before the first frame update
        void Start()
        {
            slideController = GetComponentInParent<PlayerSlideController>();
            panel = GetComponentInChildren<PlayerMoneyPanel>();
            panel.MoneyValue = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}