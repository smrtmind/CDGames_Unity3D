using DG.Tweening;
using UnityEngine;

namespace Scripts.UI
{
    public class UiElement : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0.1f)] private float _fadeDuration = 0.5f;

        private Tween _fadeTween;

        public void Show() => Fade(1f);

        public void Hide() => Fade(0f);

        private void Fade(float endValue)
        {
            _fadeTween?.Kill();
            _fadeTween = _canvasGroup.DOFade(endValue, _fadeDuration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        private void OnDisable()
        {
            _fadeTween?.Kill();
        }
    }
}
