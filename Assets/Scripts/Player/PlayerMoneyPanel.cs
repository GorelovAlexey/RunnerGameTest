using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerMoneyPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Slider slider;
        [SerializeField] private Image sliderFill;
        [SerializeField] private float animTimeSec;

        [SerializeField] private PlayerMoneySkinDictionary skinsDictionary;
        [SerializeField] private int maxMoney;

        public float SliderDisplayValue
        {
            get => slider.value;
            private set
            {
                slider.value = value;
            }
        }

        int _moneyValue;
        public int MoneyValue
        {
            get => _moneyValue;
            set
            {
                _moneyValue = value;

                var startSlow = animTween == null || animTween.active == false || animTween.position <= .15f;

                animTween?.Kill();
                animTween = GetSliderTween(value).SetEase(startSlow ? Ease.InOutCubic : Ease.OutCubic);
            }
        }

        Tween animTween;
        Tween GetSliderTween(float value) =>
            DOTween.To(() => SliderDisplayValue, x => SliderDisplayValue = x, value, animTimeSec)
            .SetLink(gameObject);

        public void SetupSlider(PlayerObject o)
        {
            SliderDisplayValue = o.MoneyCount.Value;

            o.MoneyCount.Subscribe(x => MoneyValue = x).AddTo(this);
            o.CurrentSkinKey.Subscribe(x => UpdateSkin(x)).AddTo(this);
        }

        public void UpdateSkin(PlayerMoneySkinKey needSkin)
        {
            var applySkin = skinsDictionary[needSkin];
            sliderFill.sprite = applySkin.sprite;
            statusText.color = applySkin.textColor;
            // TODO LOCALIZE KEY;
            statusText.text = applySkin.textText;
        }
    }
}