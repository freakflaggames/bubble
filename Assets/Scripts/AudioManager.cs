using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public int voxSoundChance;
    public AudioClip[] clips;
    public AudioClip[] hitClips;
    public AudioClip[] happyClips;
    public Dictionary<string, AudioClip> Clips = new Dictionary<string, AudioClip>();
    public AudioSource source, windupSource, keySource, music, voxsource;
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
    public void PlayHitSound()
    {
        int rand = Random.Range(0, voxSoundChance);
        if (rand == voxSoundChance - 1)
        {
            voxsource.PlayOneShot(hitClips[Random.Range(0, hitClips.Length)]);
        }
    }
    public void PlayHappySound()
    {
        int rand = Random.Range(0, voxSoundChance);
        if (rand == voxSoundChance - 1)
        {
            voxsource.PlayOneShot(happyClips[Random.Range(0, happyClips.Length)]);
        }
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
    public void PlayMusic()
    {
        music.Play();
    }
    public void StopMusic()
    {
        music.Stop();
    }
}
