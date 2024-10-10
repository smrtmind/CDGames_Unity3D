using DG.Tweening;
using Scripts.Objects.Items;
using Scripts.Utils;
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
        [SerializeField] private float _finalPositionY = 35f;

        [Space]
        [SerializeField, Min(0.1f)] private float _movingDurationMin = 1f;
        [SerializeField, Min(0.1f)] private float _movingDurationMax = 3f;

        [Space]
        [SerializeField, Min(1f)] private float _delayOnStartMin = 1f;
        [SerializeField, Min(1f)] private float _delayOnStartMax = 2f;

        private Tween _moveTween;
        #endregion

        private void OnEnable()
        {
            this.WaitForSeconds(Random.Range(_delayOnStartMin, _delayOnStartMax), Move);
        }

        private void Move()
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMoveY(_finalPositionY, Random.Range(_movingDurationMin, _movingDurationMax))
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .OnComplete(_item.Release);
        }

        private void OnDisable()
        {
            _moveTween?.Kill();
        }
    }
}
