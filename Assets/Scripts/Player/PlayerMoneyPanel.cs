using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Slider slider;
    [SerializeField] private Image sliderFill;
    [SerializeField] private float animTimeSec;

    [SerializeField] private List<PlayerMoneySkin> panelSkins;
    [SerializeField] private int maxMoney;

    public float SliderDisplayValue { 
        get => slider.value;
        private set
        {
            slider.value = value;
            UpdateSkin();
        } 
    }

    int _moneyValue;
    public int MoneyValue { 
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
        DOTween.To(() => SliderDisplayValue, x =>  SliderDisplayValue = x, value, animTimeSec)
        .SetLink(gameObject);

    public void Awake()
    {
        slider.minValue = 0;
        slider.maxValue = maxMoney;

        panelSkins = panelSkins.OrderBy(x => x.minMoney).ToList();
        MoneyValue = 0;
    }


    private int lastSkin = -1;
    public void UpdateSkin(bool force = false)
    {
        if (panelSkins == null || panelSkins.Count == 0)
            return;

        var needSkin = 0;

        for (var i = 1; i < panelSkins.Count; i++)
        {
            var skin = panelSkins[i];
            if (MoneyValue < skin.minMoney)
                break;

            needSkin = i;
        }

        if (!force && needSkin == lastSkin)
            return;

        var applySkin = panelSkins[needSkin];
        sliderFill.sprite = applySkin.sprite;
        statusText.color = applySkin.textColor;
        // TODO LOCALIZE KEY;
        statusText.text = applySkin.textText;
    }
}
