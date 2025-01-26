using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip[] clips;
    public Dictionary<string, AudioClip> Clips = new Dictionary<string, AudioClip>();
    public AudioSource source, windupSource, keySource, music;
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < clips.Length; i++)
        {
            Clips.Add(clips[i].name, clips[i]);
        }
    }
    public void PlaySound(string name, float pitchMin, float pitchMax)
    {
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.PlayOneShot(Clips[name]);
    }
    public void PlayKeySound(float pitch)
    {
        keySource.pitch = pitch;
        keySource.PlayOneShot(Clips["key"]);
    }
    public void PlayWindupSound()
    {
        windupSource.Play();
    }
    public void StopWindupSound()
    {
        windupSource.Stop();
    }
    public void StopMusic()
    {
        music.Stop();
    }
}
