using Assets.Scripts.Player;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OtherObjects
{
    public class MoneyObject : BasicPlayerIntractible
    {
        public int money;
        public Transform animateObject;
        public float animationTopY;
        public float animationBotY;
        public float animCycleTime = .15f;

        public void Start()
        {
            AnimationCycle();
        }

        public override void OnPlayerInteraction(PlayerObject player)
        {
            player.MoneyCount.Value += money;
            DestroySelf();

        }

        private Tween tweeen;
        private void AnimationCycle()
        {
            tweeen?.Kill();
          
            var seq = DOTween.Sequence();
            tweeen = seq;

            seq.Append(animateObject.DOLocalMoveY(animationTopY, animCycleTime).SetEase(Ease.InOutSine));
            seq.Append(animateObject.DOLocalMoveY(animationBotY, animCycleTime).SetEase(Ease.InOutSine));

            seq.SetLink(gameObject);
            seq.OnComplete(() => AnimationCycle());

            seq.Play();
        }

        private void DestroySelf()
        {
            tweeen?.Kill();

            animateObject.DOShakeScale(.15f, 2).SetLink(animateObject.gameObject);
            animateObject.DOScale(0, .3f).SetLink(animateObject.gameObject).SetEase(Ease.InQuad);

            Destroy(gameObject, 5);
        }
    }
}