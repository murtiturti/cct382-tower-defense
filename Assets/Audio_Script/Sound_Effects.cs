using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Effects : MonoBehaviour
{
    public static Sound_Effects Instance;

    [SerializeField] private AudioSource Sound_Effect_Object;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }


    public void Play_Sound_Effects(AudioClip audioclip, Transform spawntransform, float volume)
    {
        //spawn in game object
        AudioSource audioSource = Instantiate(Sound_Effect_Object, spawntransform.position, Quaternion.identity);
        //assign the audio clip
        audioSource.clip = audioclip;

        //volume adjustment

        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //length 
        float clipLength = audioSource.clip.length;

        //destroy after playing

        Destroy(audioSource.gameObject, clipLength);
    }

}

