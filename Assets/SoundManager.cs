using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _inst;
    
    public Sound[] sounds;

    public AudioSource windMusic;
    private float windBaseVolume = .1f, windBasePitch = .85f;
    public float windPitchChangeSpeed = 5f, windVolumeChangeSpeed = 5f;
    float targetWindPitch = 1f, targetWindVolume = 1f;

    public int maxInitPerSound;
    private void Awake()
    {
        _inst = this;
        foreach (var s in this.sounds)
        {
            initSound(s);
        }
    }

    private void Start()
    {
        
        windBasePitch = windMusic.pitch;
        windBaseVolume = windMusic.volume;
        targetWindPitch = windBasePitch;
        targetWindVolume = windBaseVolume;
    }
    private void Update()
    {
        windMusic.pitch = Mathf.MoveTowards(windMusic.pitch, targetWindPitch, Time.deltaTime * windPitchChangeSpeed);
        windMusic.volume = Mathf.MoveTowards(windMusic.volume, targetWindVolume, Time.deltaTime * windVolumeChangeSpeed);

    }
    public void playSoundOnce(SoundEnum soundEnum)
    {

        foreach (var s in this.sounds)
        {
            if (s.sound == soundEnum)
            {
                s.audioSource.Play();
                return;
            }
        }
    }

    public void stopSound(SoundEnum soundEnum)
    {
        foreach (var s in this.sounds)
        {
            if (s.sound == soundEnum)
            {
                s.audioSource.Stop();
                return;
            }
        }
    }
    public void playSoundOnceAt(SoundEnum soundEnum, Vector3 playPosition)
    {
        playPosition.z = 0f;
        foreach (var s in this.sounds)
        {
            if (s.sound == soundEnum)
            {
                if (s.audioSource.isPlaying)
                {
                    AudioSource.PlayClipAtPoint(s.clip, playPosition);
                }
                else
                {
                    s.audioSource.transform.position = playPosition;
                    s.audioSource.Play();
                }

                return;
            }
        }
    }


    public void playMusic(SoundEnum soundEnum)
    {
        foreach (var s in this.sounds)
        {
            if (s.sound == soundEnum && s.isMusic)
            {
                s.audioSource.Play();
                return;
            }
            else if (s.isMusic)
            {
                s.audioSource.Stop();
            }
        }
    }

    public void stopAllMusic()
    {
        foreach (var s in this.sounds)
        {
            if (s.isMusic)
            {
                s.audioSource.Stop();
            }
        }
    }

    public void initSound(Sound s)
    {
        var gmObj = new GameObject();
        gmObj.transform.SetParent(transform);
        gmObj.transform.position = Vector3.zero;
        s.audioSource = gmObj.AddComponent<AudioSource>();
        s.audioSource.clip = s.clip;
        s.audioSource.volume = s.volume;
        s.audioSource.pitch = s.pitch;
        s.audioSource.loop = s.isLooping;
        s.audioSource.playOnAwake = s.playOnAwake;
        s.audioSource.minDistance = 10f;
        s.audioSource.maxDistance = s.maxDistanceToHear;
        s.audioSource.spatialBlend = (s.is3D) ? 1 : 0;
    }


    public void setWindProps(float pitch = 0, float volume = 0)
    {
      
        targetWindPitch = windBasePitch + pitch;
        targetWindVolume = windBaseVolume + volume;
    }

}
