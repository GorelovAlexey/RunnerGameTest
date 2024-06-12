using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts.Player
{
    [Serializable]
    public class SpeedDictionary : SerializableDictionaryBase<PlayerMoneySkinKey, float> { };


    public class PlayerSlideController : MonoBehaviour
    {
        [SerializeField] private SpeedDictionary walkSpeeds;
        [SerializeField] private Transform leftBorder;
        [SerializeField] private Transform rightBorder;
        [SerializeField] private Transform playerSlideObject;

        [Range(0, 1f)]
        [SerializeField] public float slidePosition = .5f;

        [Range(0, 10f)]
        [SerializeField] private float walkSpeedPerSec = 2f;



        private bool _canMove = true;
        public bool CanMove
        {
            get => _canMove; set
            {
                _canMove = value;
                CanMoveReactive.Value = value;
            }
        }

        public ReactiveProperty<bool> CanMoveReactive = new ReactiveProperty<bool>();

        private Rigidbody rb;
        private PlayerObject playerModel;

        public void SetCantMoveSilent(bool canMove)
        {
            _canMove = canMove;
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            playerModel = GetComponentInChildren<PlayerObject>();
            var animator = GetComponentInChildren<PlayerAnimationController>();
            var minSpeed = walkSpeeds.Min(x => x.Value);
            var maxSpeed = walkSpeeds.Max(x => x.Value);

            playerModel.CurrentSkinKey.StartWith(playerModel.CurrentSkinKey.Value)
                .Subscribe(_ => SetWalkSpeedToSkinValue()).AddTo(gameObject);

            CanMoveReactive.Subscribe(x =>
            {
                CanMove = x;
                animator.CanWalk = x;

                if (x)
                    SetWalkSpeedToSkinValue();

            }).AddTo(gameObject);


            void SetWalkSpeedToSkinValue()
            {
                var skin = playerModel.CurrentSkinKey.Value;
                walkSpeedPerSec = walkSpeeds[skin];
                animator.WalkSpeed = Mathf.InverseLerp(minSpeed, maxSpeed, walkSpeedPerSec);
            }

        }

        private void FixedUpdate()
        {
            if (!CanMove)
                return;

            rb.MovePosition(transform.position + Time.fixedDeltaTime * walkSpeedPerSec * transform.forward);
        }

        // Update is called once per frame
        void Update()
        {
            if (!leftBorder || !rightBorder || !playerSlideObject)
                return;

            slidePosition = Mathf.Clamp(slidePosition, 0, 1f);
            var playerLocalPos = Vector3.Lerp(leftBorder.localPosition, rightBorder.localPosition, slidePosition);
            playerSlideObject.localPosition = playerLocalPos;
        }


    }
}