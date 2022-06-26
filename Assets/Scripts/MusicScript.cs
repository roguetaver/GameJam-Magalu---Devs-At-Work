using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public AudioClip[] allMusics;
    private AudioSource audioSource;
    private int actualMusic;

    void Start()
    {
        int numMusic = FindObjectsOfType<MusicScript>().Length;
        if (numMusic != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        audioSource = this.GetComponent<AudioSource>();
        actualMusic = 0;
        audioSource.clip = allMusics[actualMusic];
        audioSource.Play();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.time = 7.619f;
            audioSource.Play();
        }
    }

    public void NextMusic(){
        if(actualMusic < allMusics.Length - 1){
            actualMusic++;
            float musicTime = audioSource.time;
            audioSource.Stop();
            audioSource.clip = allMusics[actualMusic];
            audioSource.Play();
            audioSource.time = musicTime;
        }
    }
}
