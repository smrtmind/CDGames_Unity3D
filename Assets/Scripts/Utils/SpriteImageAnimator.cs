using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Utils
{
    public class SpriteImageAnimator : MonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private Image _image;
        [SerializeField] private Sprite[] _sprites;

        [Header("Parameters")]
        [SerializeField] private bool _loop;
        [SerializeField][Range(1, 60)] private int _frameRate = 10;

        private float _secondsPerFrame;
        private float _nextFrameTime;
        private int _currentFrame;
        #endregion

        private void OnEnable()
        {
            _nextFrameTime = Time.unscaledTime;
            _currentFrame = 0;
        }

        private void Start()
        {
            _secondsPerFrame = 1f / _frameRate;
        }

        private void Update()
        {
            if (_nextFrameTime > Time.unscaledTime) return;

            if (_currentFrame >= _sprites.Length)
            {
                if (_loop) _currentFrame = 0;
            }

            SetSprite(_sprites[_currentFrame]);

            _nextFrameTime += _secondsPerFrame;
            _currentFrame++;
        }

        private void SetSprite(Sprite sprite) => _image.sprite = sprite;
    }
}
