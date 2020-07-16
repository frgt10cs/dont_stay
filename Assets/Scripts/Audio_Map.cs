using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Map : MonoBehaviour {

    [SerializeField]
    public AudioClip chase;

    [SerializeField]
    public AudioClip attack;

    public AudioClip opening;



    public void Start()
    {
        
    }

    public void play(AudioClip music, float delay)
    {        
        gameObject.GetComponent<AudioSource>().clip = music;
        gameObject.GetComponent<AudioSource>().PlayDelayed(delay);
    }

}
