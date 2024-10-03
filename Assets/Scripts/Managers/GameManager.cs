using System;
using UnityEngine;

namespace Scripts.Managers
{
    [Serializable]
    public enum GameState
    {
        None,
        StartScreen,
        Gameplay,
        Victory,
        Defeat
    }

    public class GameManager : MonoBehaviour
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;

        public static GameManager Instance;

        public GameState State { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            ChangeState(GameState.StartScreen);
        }

        public void ChangeState(GameState newState)
        {
            OnBeforeStateChanged?.Invoke(newState);
            State = newState;
            OnAfterStateChanged?.Invoke(newState);

#if UNITY_EDITOR
            Debug.Log($"<color=green>New state: {newState}</color>", this);
#endif
        }
    }
}
