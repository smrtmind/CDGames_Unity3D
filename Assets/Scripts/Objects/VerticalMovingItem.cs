using DG.Tweening;
using Scripts.Objects.Items;
using Scripts.Utils;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Objects
{
    public class VerticalMovingItem : MonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private Item _item;

        [Header("Parameters")]
        [SerializeField, Min(0.1f)] private float _movingDuration = 3;

        [Space]
        [SerializeField] private float _minDelayOnStart = 1f;
        [SerializeField] private float _maxDelayOnStart = 2f;

        [Space]
        [SerializeField] private float _minPositionY = 35f;
        [SerializeField] private float _maxPositionY = 60f;

        private Tween _moveTween;
        #endregion

        private void OnEnable()
        {
            this.WaitForSeconds(Random.Range(_minDelayOnStart, _maxDelayOnStart), () =>
            {
                Move(() => _item.Release());
            });
        }

        private void Move(Action onComplete)
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMoveY(Random.Range(_minPositionY, _maxPositionY), _movingDuration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .OnComplete(() => onComplete?.Invoke());
        }

        private void OnDisable()
        {
            _moveTween?.Kill();
        }
    }
}
