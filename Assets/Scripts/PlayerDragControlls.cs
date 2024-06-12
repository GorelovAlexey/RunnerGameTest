using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class PlayerDragControlls : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        RectTransform rectTransform;
        Vector2 lastPosition;
        PlayerSlideController slideController;

        public float sensetivity = 1f;

        public void Awake()
        {
            slideController = FindAnyObjectByType<PlayerSlideController>();
        }

        public void SetupPlayer(PlayerObject p) 
        {
            slideController = p.PlayerSlideController;
        }

        public ReactiveProperty<bool> DragStart = new ReactiveProperty<bool>();
           
        public void OnBeginDrag(PointerEventData eventData)
        {
            DragStart.Value = true;

            lastPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragStart.Value = false;

            var dragDistance = (eventData.position - lastPosition).x;
            lastPosition = eventData.position;

            // TODO physical screen size???
            var width = Screen.width;
            var relativePath = dragDistance * sensetivity / width;

            slideController.slidePosition += relativePath; 
            //Debug.Log($"{dragDistance} {eventData.position - eventData.pressPosition}");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragStart.Value = false;
        }
    }
}