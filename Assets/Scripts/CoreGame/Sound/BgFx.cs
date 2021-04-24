using System.Collections;
using UnityEngine;
namespace CoreGame.Sound
{
    public class BgFx : MonoBehaviour
    {
        [SerializeField] AudioClip[] backgroundSound;
        [SerializeField] int minPauseBetweenSounds = 10;
        [SerializeField] int maxPauseBetweenSounds = 30;
        [SerializeField] float bgFxVolume;

        private bool _makeSounds = true;

        private AudioSource _myAudioSource;

        private void Awake()
        {
            _myAudioSource = gameObject.GetComponent<AudioSource>();
            _myAudioSource.volume = bgFxVolume;
        }

        public void PlayBgfx()
        {
            _makeSounds = true;
            StartCoroutine(RepeatPlayBgfx());
        }

        public void StopBgfx()
        {
            _makeSounds = false;
        }

        private IEnumerator RepeatPlayBgfx()
        {
            while (_makeSounds == true)
            {
                yield return new WaitForSeconds(Random.Range(minPauseBetweenSounds, maxPauseBetweenSounds));
                PlayOneSound();
                //spawn = false;
            }
        }

        public void SetVolume(float volumeIn)
        {
            if (_myAudioSource)
                _myAudioSource.volume = volumeIn;
            bgFxVolume = volumeIn;
        }

        public float GetVolume()
        {
            return bgFxVolume;
        }

        private void PlayOneSound()
        {
            _myAudioSource.clip = backgroundSound[Random.Range(1, backgroundSound.Length)];
            _myAudioSource.Play();

        }

    }
}
