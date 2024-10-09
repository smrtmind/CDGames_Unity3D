using UnityEngine;

namespace Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private SoundData[] _soundDatas;

        public void PlaySfx(string name)
        {
            foreach (var data in _soundDatas)
            {
                if (data.Name == name)
                {
                    _sfxSource.PlayOneShot(data.Clip);
                    break;
                }
            }
        }

        public void SetMusicTrack(string name)
        {
            foreach (var data in _soundDatas)
            {
                if (data.Name == name)
                {
                    _musicSource.clip = data.Clip;
                    _musicSource.Play();

                    break;
                }
            }
        }

        public void StopMusic() => _musicSource.Stop();
    }
}
