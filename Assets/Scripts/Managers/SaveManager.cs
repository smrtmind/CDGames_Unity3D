using UnityEngine;

namespace Scripts.Managers
{
    public class SaveManager : MonoBehaviour
    {
        private const string BestScoreKey = "BestScore";

        public int BestScore { get; private set; }

        private void Awake()
        {
            Load();
        }

        public void Save(int score)
        {
            PlayerPrefs.SetInt(BestScoreKey, score);
            PlayerPrefs.Save();
        }

        private void Load()
        {
            BestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        }
    }
}
