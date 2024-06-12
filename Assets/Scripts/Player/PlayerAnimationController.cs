using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private bool _dance;
        public bool Dance
        {
            get => _dance;
            set
            {
                _dance = value;
                animator.SetBool("dancing", value);
            }
        }


        float _walkSpeed = 0;
        public float WalkSpeed { 
            get => _walkSpeed; 
            set {
                _walkSpeed = value;
                animator.SetFloat("walkSpeed", value);
            }
        }

        private bool _canWalk;
        public bool CanWalk
        {
            get => _canWalk;
            set
            {
                _canWalk = value;
                animator.SetBool("canWalk", value);
            }
        }
    }
}