using Scripts.Managers;
using Scripts.Utils;
using System;
using System.Collections;
using UnityEngine;
using Zenject;
using IPoolable = Scripts.Pooling.IPoolable;

namespace Scripts.Objects
{
    public class Board : MonoBehaviour, IPoolable
    {
        [SerializeField, Min(1f)] float speed = 20f;
        [SerializeField, Min(1f)] private float _lifespan = 1f;

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;

        private Coroutine _lifespanRoutine;
        private WaitForSeconds _waitForLifespan;
        private GameManager _gameManager;

        [Inject]
        private void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Awake()
        {
            _waitForLifespan = new WaitForSeconds(_lifespan);
        }

        private void OnEnable()
        {
            if (_lifespanRoutine == null)
                _lifespanRoutine = StartCoroutine(ControlLifespan());
        }

        private void Update()
        {
            if (_gameManager.State != GameState.Gameplay) return;
            
            Move();
        }

        private void Move() => transform.position += Vector3.back * Time.deltaTime * speed;

        private IEnumerator ControlLifespan()
        {
            yield return _waitForLifespan;
            Release();
        }

        public void Release() => Destroyed?.Invoke(this);

        private void OnDisable()
        {
            this.StopCoroutine(ref _lifespanRoutine);
        }
    }
}
