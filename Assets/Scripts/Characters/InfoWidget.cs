using Scripts.Managers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.Characters
{
    public class InfoWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _boards;

        private MatchManager _matchManager;

        [Inject]
        private void Construct(MatchManager matchManager)
        {
            _matchManager = matchManager;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            MatchManager.OnBoardsAmountChanged += RefreshBoards;
            Player.OnPlayerLost += DisableCounter;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            MatchManager.OnBoardsAmountChanged -= RefreshBoards;
            Player.OnPlayerLost -= DisableCounter;
        }

        private void OnAfterStateChanged(GameState state)
        {
            if (state != GameState.Gameplay) return;

            RefreshBoards(_matchManager.Boards);
        }

        private void RefreshBoards(int value) => _boards.text = $"{value}";

        private void DisableCounter() => _boards.gameObject.SetActive(false);

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
