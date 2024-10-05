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

        private RagdollPart[] _ragdollParts;

        private void Start()
        {
            _ragdollParts = GetComponentsInChildren<RagdollPart>();
            DisableRagdoll();
        }

        public void Run() => _animator.SetTrigger(RunningKey);

        public void Win() => _animator.SetTrigger(WinKey);

        public void Lose() => _animator.SetTrigger(LoseKey);

        public void Fall() => _animator.SetTrigger(FallKey);

        public void EnableRagdoll() => SetRagdollState(true);

        public void DisableRagdoll() => SetRagdollState(false);

        private void SetRagdollState(bool state)
        {
            _animator.enabled = !state;

            if (_ragdollParts.Length > 0)
            {
                foreach (var part in _ragdollParts)
                {
                    part.Rigidbody.isKinematic = !state;
                    part.Rigidbody.detectCollisions = state;
                    part.Collider.enabled = state;
                }
            }
        }
    }
}
