using System;
using UnityEngine;

namespace Scripts.Managers
{
    [Serializable]
    public class SoundData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public AudioClip Clip { get; private set; }
    }
}
