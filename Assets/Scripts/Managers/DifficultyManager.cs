using Scripts.Utils;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Managers
{
    public class DifficultyManager : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float _increaseDifficultyStepSec = 10f;

        private MatchManager _matchManager;
        private Coroutine _difficultyRoutine;
        private WaitForSeconds _waitForIncreaseDifficultyStepSec;

        [Inject]
        private void Construct(MatchManager matchManager)
        {
            _matchManager = matchManager;
        }

        private void Awake()
        {
            _waitForIncreaseDifficultyStepSec = new WaitForSeconds(_increaseDifficultyStepSec);
        }

        private void OnEnable()
        {
            MatchManager.OnMatchStarted += OnMatchStarted;
        }

        private void OnDisable()
        {
            MatchManager.OnMatchStarted -= OnMatchStarted;

            this.StopCoroutine(ref _difficultyRoutine);
        }

        private void OnMatchStarted()
        {
            if (_difficultyRoutine == null)
                _difficultyRoutine = StartCoroutine(ControlDifficulty());
        }

        private IEnumerator ControlDifficulty()
        {
            while (_matchManager.IsStarted)
            {
                yield return _waitForIncreaseDifficultyStepSec;
                _matchManager.IncreaseLevelSpeed();
            }

            _difficultyRoutine = null;
        }
    }
}
