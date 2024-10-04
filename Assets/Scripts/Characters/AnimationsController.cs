using UnityEngine;

namespace Scripts.Characters
{
    public class AnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private static readonly int FallKey = Animator.StringToHash("fall");
        private static readonly int RunningKey = Animator.StringToHash("run");
        private static readonly int LoseKey = Animator.StringToHash("lose");
        private static readonly int WinKey = Animator.StringToHash("win");

        public void Run() => _animator.SetTrigger(RunningKey);

        public void Win() => _animator.SetTrigger(WinKey);

        public void Lose() => _animator.SetTrigger(LoseKey);

        public void Fall() => _animator.SetTrigger(FallKey);
    }
}
