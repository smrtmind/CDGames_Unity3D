using Scripts.Managers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.Characters
{
    public class InfoWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coins;
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
            MatchManager.OnCoinsAmountChanged += RefreshCoins;
            MatchManager.OnBoardsAmountChanged += RefreshBoards;
            Player.OnPlayerLost += DisableCounters;
            MatchManager.OnMatchEnded += DisableCounters;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            MatchManager.OnCoinsAmountChanged -= RefreshCoins;
            MatchManager.OnBoardsAmountChanged -= RefreshBoards;
            Player.OnPlayerLost -= DisableCounters;
            MatchManager.OnMatchEnded -= DisableCounters;
        }

        private void OnAfterStateChanged(GameState state)
        {
            if (state != GameState.Gameplay) return;

            RefreshBoards(_matchManager.Boards);
        }

        private void RefreshCoins(int value) => _coins.text = $"{value}";

        private void RefreshBoards(int value) => _boards.text = $"{value}";

        private void DisableCounters()
        {
            _coins.gameObject.SetActive(false);
            _boards.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
