using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class BoardControlSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Slider _slider;

        public static Action OnTouched;
        public static Action OnReleased;

        public void OnPointerDown(PointerEventData eventData) => OnTouched?.Invoke();

        public void OnPointerUp(PointerEventData eventData) => OnReleased?.Invoke();

        public float GetValue() => _slider.value;
    }
}
