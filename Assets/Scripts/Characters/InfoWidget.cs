using Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Scripts.Characters
{
    public class InfoWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coins;
        [SerializeField] private TMP_Text _boards;

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            MatchManager.OnCoinsAmountChanged += RefreshCoins;
            MatchManager.OnBoardsAmountChanged += RefreshBoards;
            Player.OnPlayerLost += DisableCounters;
            MatchManager.OnMatchEnded += DisableCounters;
        }

        private void Unsubscribe()
        {
            MatchManager.OnCoinsAmountChanged -= RefreshCoins;
            MatchManager.OnBoardsAmountChanged -= RefreshBoards;
            Player.OnPlayerLost -= DisableCounters;
            MatchManager.OnMatchEnded -= DisableCounters;
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
