using Assets.Scripts.Player;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.OtherObjects
{

    public class PaparazziObject : BasicPlayerIntractible 
    {
        public Animator anim;

        public float speedRotateDeg = 180;
        public float speedWalk = 2;

        public Transform walkMarkerA;
        public Transform walkMarkerB;

        private Vector3 posA;
        private Vector3 posB;

        public int money = -10;
        public override void OnPlayerInteraction(PlayerObject player)
        {
            player.MoneyCount.Value += money;
            DestroySelf();
        }

        private void Start()
        {
            posA = walkMarkerA.transform.position;
            posB = walkMarkerB.transform.position;

            RotateTo(posA, posB);
        }

        private void DestroySelf()
        {
            moveTween?.Kill();
            anim.SetBool("dance", true);
            Destroy(gameObject, 5);
        }

        Tween moveTween;
        private void WalkToPos(Vector3 target, Vector3 futureTarget)
        {
            var posEnd = new Vector3(target.x, transform.position.y, target.z);

            var dist = (posEnd - transform.position).magnitude;
            var time = dist / speedWalk;

            moveTween = transform.DOMove(target, time).SetLink(gameObject)
                .OnComplete(() => RotateTo(futureTarget, target)).SetEase(Ease.InOutSine);
        }

        private void RotateTo(Vector3 target, Vector3 futureTarget)
        {
            var posEnd = new Vector3(target.x, transform.position.y, target.z);

            var lookRotation = Quaternion.LookRotation(posEnd - transform.position);
            var currentRotation = transform.rotation;

            var angle = Mathf.Abs(Quaternion.Angle(lookRotation, currentRotation));
            var time = angle / speedRotateDeg;

            moveTween = transform.DORotateQuaternion(lookRotation, time).SetLink(gameObject)
                .OnComplete(() => WalkToPos(target, futureTarget)).SetEase(Ease.InOutSine);
        }
    }

}