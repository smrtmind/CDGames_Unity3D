using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Utils
{
    public static class DelayedCall
    {
        private class Engine : MonoBehaviour { }

        private static readonly Engine _engine;

        static DelayedCall()
        {
            _engine = new GameObject("delayed_calls").AddComponent<Engine>();
            Object.DontDestroyOnLoad(_engine.gameObject);
        }

        public static void WaitForFrame(Action action) => _engine.WaitForFrame(action);
        public static void WaitForFrames(int frames, Action action) => _engine.WaitForFrames(frames, action);
        public static void WaitEndOfFrame(Action action) => _engine.WaitEndOfFrame(action);
        public static void WaitForSeconds(float seconds, Action action) => _engine.WaitForSeconds(seconds, action);
        public static void WaitForSecondsUnclamped(float seconds, Action action) => _engine.WaitForSeconds(seconds, action);

        public static Coroutine WaitForFrame(this MonoBehaviour engine, Action action)
            => engine.CustomInstruction(action, null);

        public static Coroutine WaitForFrames(this MonoBehaviour engine, int frames, Action action)
        {
            IEnumerator ExecuteInstruction()
            {
                for (var i = 0; i < frames; i++) yield return null;
                action();
            }

            return engine.StartCoroutine(ExecuteInstruction());
        }

        public static Coroutine WaitEndOfFrame(this MonoBehaviour engine, Action action)
            => engine.CustomInstruction(action, new WaitForEndOfFrame());

        public static Coroutine WaitForSeconds(this MonoBehaviour engine, float seconds, Action action)
            => engine.CustomInstruction(action, new WaitForSeconds(seconds));

        public static Coroutine WaitForSecondsUnclamped(this MonoBehaviour engine, float seconds, Action action)
            => engine.CustomInstruction(action, new WaitForSecondsRealtime(seconds));

        private static Coroutine CustomInstruction(this MonoBehaviour engine, Action action, object instruction)
        {
            IEnumerator ExecuteInstruction()
            {
                yield return instruction;
                action();
            }

            return engine.StartCoroutine(ExecuteInstruction());
        }
    }
}
