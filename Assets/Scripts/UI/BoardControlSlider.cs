using Scripts.Characters;
using Scripts.Managers;
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

        private void OnEnable()
        {
            Subscribe();

            _slider.image.raycastTarget = false;
        }

        private void Subscribe()
        {
            MatchManager.OnMatchStarted += OnMatchStarted;
            Player.OnPlayerLost += OnPlayerLost;
        }

        private void Unsubscribe()
        {
            MatchManager.OnMatchStarted -= OnMatchStarted;
            Player.OnPlayerLost -= OnPlayerLost;
        }

        private void OnMatchStarted() => _slider.image.raycastTarget = true;

        private void OnPlayerLost()
        {
            _slider.image.raycastTarget = false;
            _slider.value = 0f;
        }

        public void OnPointerDown(PointerEventData eventData) => OnTouched?.Invoke();

        public void OnPointerUp(PointerEventData eventData)
        {
            _slider.value = 0f;
            OnReleased?.Invoke();
        }

        public float GetValue() => _slider.value;

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
