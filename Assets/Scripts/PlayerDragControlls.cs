using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player
{
    public class PlayerDragControlls : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        RectTransform rectTransform;
        Vector2 lastPosition;
        PlayerSlideController slideController;

        public float sensetivity = 1f;
        public void OnBeginDrag(PointerEventData eventData)
        {
            lastPosition = eventData.position;
            rectTransform = GetComponent<RectTransform>();
            slideController = FindAnyObjectByType<PlayerSlideController>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            var dragDistance = (eventData.position - lastPosition).x;
            lastPosition = eventData.position;

            // TODO physical screen size???
            var width = rectTransform.rect.width;
            var relativePath = dragDistance * sensetivity / width;

            slideController.slidePosition += relativePath; 
            Debug.Log($"{dragDistance} {eventData.position - eventData.pressPosition}");
        }
    }
}