using Scripts.Managers;
using Scripts.Utils;
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
            MatchManager.OnCoinsAmountChanged += RefreshCoins;
            MatchManager.OnBoardsAmountChanged += RefreshBoards;
        }

        private void OnDisable()
        {
            MatchManager.OnCoinsAmountChanged -= RefreshCoins;
            MatchManager.OnBoardsAmountChanged -= RefreshBoards;
        }

        private void RefreshCoins(int value) => _coins.text = $"{value}";

        private void RefreshBoards(int value) => _boards.text = $"{value}";
    }
}
