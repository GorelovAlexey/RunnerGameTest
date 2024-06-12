using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OtherObjects
{
    public class DegreeRotator : BasicPlayerIntractible
    {
        [SerializeField] private float rotationDegrees = 90f;
        [SerializeField] private float rotationSpeedSeconds = 90f;

        [SerializeField] private bool rotateClockwise = true;

        public void CapturePlayer(PlayerObject player)
        {
            // чтобы не влиять на анимацию
            player.PlayerSlideController.SetCantMoveSilent(false);
        }

        public void ReleasePlayer(PlayerObject player)
        {
            player.PlayerSlideController.CanMove = true;
        }

        public override void OnPlayerInteraction(PlayerObject player)
        {
            StartCoroutine(RotateCoroutine(player));
        }

        IEnumerator RotateCoroutine(PlayerObject player)
        {
            CapturePlayer(player);

            var rotatingObject = player.PlayerSlideController.transform;
            var centerPosition = transform.position;
            centerPosition.y = rotatingObject.transform.position.y;

            var leftDegrees = rotationDegrees;

            while (leftDegrees > 0)
            {
                yield return new WaitForFixedUpdate();

                var rotateAngle = rotationSpeedSeconds * Time.fixedDeltaTime;
                if (rotateAngle > leftDegrees)
                {
                    rotateAngle = leftDegrees;
                    leftDegrees = 0;
                }
                else
                    leftDegrees -= rotateAngle;

                if (!rotateClockwise)
                    rotateAngle = -rotateAngle;

                rotatingObject.RotateAround(transform.position, Vector3.up, rotateAngle);
            }

            ReleasePlayer(player);
        }
    }
}