using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintAnimation : MonoBehaviour
{
    public float animTime;
    public RectTransform hand;
    public RectTransform left;
    public RectTransform right;


    Tween animation;
    private void OnEnable()
    {
        if (animation == null)
        {
            var seq = DOTween.Sequence();
            var initial = hand.position;
            seq.Append(hand.DOMove(left.position, animTime).ChangeStartValue(right.position));
            seq.Append(hand.DOMove(right.position, animTime));
            seq.SetAutoKill(false);
            seq.SetLink(gameObject);
            seq.OnComplete(() => seq.Restart());
            seq.Play();

            animation = seq;
        }

        animation?.Restart();
    }

    private void OnDisable()
    {
        animation?.Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
