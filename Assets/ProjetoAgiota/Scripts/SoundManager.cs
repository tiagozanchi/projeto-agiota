using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    [Range(0,1)]
    public float volume = 1f;
    [Range(-3,3)]
    public float pitch = 1f;
    public bool loop;
}

public class SoundManager : MonoBehaviour
{
    public enum Sounds
    {
        BGM_Gameplay,
        BGM_Menu,
        Largada,
        CarEngine,
        DerraparOleo,
        Explosion,
        Crash,
        Nitro
    }

    [SerializeField]
    private Sound[] soundsArray = null;
    private Dictionary<Sounds, Sound> _soundsDictionary = new Dictionary<Sounds, Sound>();
    private List<AudioSource> _audioSourcesToDisable = new List<AudioSource>();
    private Coroutine _audioSourcesDisablerRoutine = null;

    public void Awake()
    {
        Array soundTypes = Enum.GetValues(typeof(Sounds));
        foreach (Sounds sound in soundTypes)
        {
            _soundsDictionary.Add(sound, soundsArray[(int)sound]);
        }
        
        if (_soundsDictionary.Count < soundTypes.Length)
        {
            Debug.LogError("Some sound type is not set in the Sound Manager.");
            throw new Exception("Some sound type is not set in the Sound Manager.");
        }
    }
    
    public void Play(Sounds clip, AudioSource audioSource, Vector3? position = null)
    {
        if (audioSource == null) return;

        Sound tempSound = _soundsDictionary[clip];

        if (position != null) audioSource.transform.position = position.Value;

        audioSource.enabled = true;

        audioSource.volume = tempSound.volume;
        audioSource.pitch = tempSound.pitch;
        audioSource.loop = tempSound.loop;
        audioSource.clip = tempSound.clip;

        audioSource.Play();
        if (!tempSound.loop)_audioSourcesToDisable.Add(audioSource);

        if (_audioSourcesDisablerRoutine == null) _audioSourcesDisablerRoutine = StartCoroutine(AudioSourcesDisablerRoutine());
    }

    public bool IsAvailableToPlay(AudioSource audioSource, Sounds clip)
    {
        return audioSource == null || audioSource.clip == null || !audioSource.clip.Equals(_soundsDictionary[clip].clip);
    }

    public IEnumerator PlayInSequency(AudioSource audioSource, Vector3? position = null, params Sounds[] clips)
    {
        if (audioSource == null) yield break;

        foreach (Sounds clip in clips)
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }

            Play(clip, audioSource, position);
        }
    }

    private IEnumerator AudioSourcesDisablerRoutine()
    {
        while (_audioSourcesToDisable.Count > 0)
        {
            List<AudioSource> shouldDisableList = new List<AudioSource>();
            foreach (AudioSource audioSource in _audioSourcesToDisable)
            {
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.enabled = false;
                    shouldDisableList.Add(audioSource);
                }
            }

            foreach (AudioSource audioSource in shouldDisableList)
            {
                _audioSourcesToDisable.Remove(audioSource);
            }

            yield return null;
        }
        
        _audioSourcesDisablerRoutine = null;
    }
}
