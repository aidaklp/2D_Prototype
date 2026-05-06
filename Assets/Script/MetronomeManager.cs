using UnityEngine;

public class MetronomeManager : MonoBehaviour
{
   

    // setting up my variables
    public AudioSource audioSource;
    public AudioClip clickSound;
    public float BPM = 120f;// 120 beats per minute

    private float interval;
    private float timer;

    void OnEnable()
    {
        // recalculates the  interval and resets timer every time metronome is switched on
        interval = 60f / BPM; //calculates interval of the as the bpm is 120 and its devided by 60 as there are 60 seconds in a minute meaning that it will have to beep every 0.5 s
        
        timer = interval; // trigger immediately on first beat
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        //checks if enough time has passed so it can play the next beat
        if (timer >= interval)
        {
            audioSource.PlayOneShot(clickSound);
            timer = 0f;
        }
    }

}
