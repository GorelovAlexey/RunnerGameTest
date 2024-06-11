using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [ExecuteInEditMode]
    public class PlayerSlideController : MonoBehaviour
    {
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
            }
        }

        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!CanMove)
                return;

            rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * walkSpeedPerSec);
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