using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    public class UiElement : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0.1f)] private float _fadeDuration = 0.5f;

        private void Show()
        {

        }

        private void Hide()
        {

        }
    }
}
