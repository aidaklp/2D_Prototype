using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class RecordAudio : MonoBehaviour
{
    // used to store my audiclip
    private AudioClip recordedClip;

    // refrences the audio source where the recorder clip will be played
   [SerializeField] AudioSource audioSource;


    //Method for starting recording
    public void StartRecording()
    {
        // this is to pick the default microphone
        string device = Microphone.devices[0];

        // quality of the recording
        int sampleRate = 44100;

        // how long we want the recording to be 
        int lengthSec = 35;

        // defines our recorded clip
        recordedClip = Microphone.Start(device, false , lengthSec, sampleRate);

    }

    //Method for paying recording 
    public void PlayRecording()
    {
        //giving the audio source the recorded clip
        audioSource.clip = recordedClip;

        //calling the play method
        audioSource.Play(); 
    }

    //Method to stop recording
    public void StopRecording()
    {
        //stops microphone  
        Microphone.End(null); 
    }
    

}
