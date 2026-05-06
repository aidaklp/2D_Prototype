using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class RecordMultipleAudios : MonoBehaviour
{
    // used to store my audiclip
    private AudioClip recordedClip;

    // refrences the audio source where the recorder clip will be played
    [SerializeField] AudioSource audioSource;


    // ─────────────────────────────────────────────
    // Two-Player Recording System
    // ─────────────────────────────────────────────

    public enum GamePhase
    {
        Dialogue1,// added by siennaS
        Player1Recording,
        Player2Recording,
        FinalPlayback
    }

    [Header("Two-Player State")]
    [SerializeField] public GamePhase currentPhase = GamePhase.Player1Recording;
    private AudioClip player1Clip;
    private AudioClip player2Clip;
    private bool isRecording = false;

    // ── Audio Sources ──────────────────────────────
    [Header("Playback Audio Sources")]
    [Tooltip("AudioSource dedicated to playing back Player 1's recording")]
    [SerializeField] private AudioSource player1AudioSource;

    [Tooltip("AudioSource dedicated to playing back Player 2's recording")]
    [SerializeField] private AudioSource player2AudioSource;

    // ── Optional UI references (assign in Inspector or leave blank) ──
    [Header("UI – Player 1 Recording Screen")]
    [SerializeField] private GameObject player1RecordingPanel;
    [SerializeField] private Button player1StartRecordingButton;
    [SerializeField] private Button player1StopRecordingButton;
    [SerializeField] private Button player1ConfirmButton;         // advances to Player 2
    [SerializeField] private TextMeshProUGUI player1StatusText;

    [Header("UI – Player 2 Recording Screen")]
    [SerializeField] private GameObject player2RecordingPanel;
    [SerializeField] private Button player2StartRecordingButton;
    [SerializeField] private Button player2StopRecordingButton;
    [SerializeField] private Button player2ConfirmButton;         // advances to Final screen
    [SerializeField] private TextMeshProUGUI player2StatusText;

    [Header("UI – Final Playback Screen")]
    [SerializeField] private GameObject finalPlaybackPanel;
    [SerializeField] private Button playBothButton;
    [SerializeField] private Button stopBothButton;
    [SerializeField] private TextMeshProUGUI finalStatusText;



    //refrence to the metronome manager script
    [SerializeField] private MetronomeManager metronomeManager; 

    // ─────────────────────────────────────────────
    // Unity Lifecycle
    // ─────────────────────────────────────────────

    private void Start()
    {
        //restes both recording to null (so deletes the recording) when scene  starts (so can reset when new round start)
        player1Clip = null;
        player2Clip = null;

        isRecording = false;
        // makes sure it starts on the right phase
        currentPhase = GamePhase.Dialogue1;
        ShowPhaseUI(GamePhase.Dialogue1); //edited by sienna

        // resets metronome
        if (metronomeManager != null)
        {
            metronomeManager.enabled = false;
        }


        // Wire up buttons if they have been assigned in the Inspector
        player1StartRecordingButton?.onClick.AddListener(StartPlayer1Recording);
        player1StopRecordingButton?.onClick.AddListener(StopPlayer1Recording);
        player1ConfirmButton?.onClick.AddListener(ConfirmPlayer1AndAdvance);

        player2StartRecordingButton?.onClick.AddListener(StartPlayer2Recording);
        player2StopRecordingButton?.onClick.AddListener(StopPlayer2Recording);
        player2ConfirmButton?.onClick.AddListener(ConfirmPlayer2AndAdvance);

        playBothButton?.onClick.AddListener(PlayBothRecordings);
        stopBothButton?.onClick.AddListener(StopBothRecordings);

        
    }

    // ─────────────────────────────────────────────
    // Original Methods (unchanged)
    // ─────────────────────────────────────────────

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
        recordedClip = Microphone.Start(device, false, lengthSec, sampleRate);
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

    // ─────────────────────────────────────────────
    // Player 1 Recording
    // ─────────────────────────────────────────────

    /// <summary>Starts a fresh recording for Player 1.</summary>
    public void StartPlayer1Recording()
    {
        if (currentPhase != GamePhase.Player1Recording) return;

        string device = Microphone.devices[0];

        // Start the actual recording that gets saved
        player1Clip = Microphone.Start(device, false, 35, 44100);
        isRecording = true;

        // Route the live mic through the AudioSource so the visualizer
        // can read spectrum data from it in real time via GetSpectrumData()
        player1AudioSource.clip = player1Clip;
        player1AudioSource.loop = true;
        StartCoroutine(StartAudioSourceWhenMicReady(player1AudioSource, device));

        SetText(player1StatusText, "Recording… Player 1, play your instrument!");
        player1StartRecordingButton?.gameObject.SetActive(false);
        player1StopRecordingButton?.gameObject.SetActive(true);
        player1ConfirmButton?.gameObject.SetActive(false);

        //checks whether the players have purchased a metronome and sets it to true if that have which will indicate the metronome script to start
        if (GameData.Instance.hasMetronome)
        {
            metronomeManager.enabled = true;
        }
    }

    /// <summary>Stops the Player 1 microphone and stores the clip.</summary>
    public void StopPlayer1Recording()
    {
        if (!isRecording) return;

        Microphone.End(null);
        isRecording = false;

        // Stop the live monitoring so the AudioSource is free for playback later
        player1AudioSource.Stop();
        player1AudioSource.loop = false;

        SetText(player1StatusText, "Recording saved! Press Confirm when you're ready.");
        player1StopRecordingButton?.gameObject.SetActive(false);
        player1ConfirmButton?.gameObject.SetActive(true);

        //stops metronome when player stops recording 
        if (metronomeManager != null)
        {
            metronomeManager.enabled = false;
        } 
    }

    /// <summary>
    /// Trims the Player 1 clip to the actual recorded length and
    /// advances the game to the Player 2 recording phase.
    /// </summary>
    public void ConfirmPlayer1AndAdvance()
    {
        if (player1Clip == null)
        {
            SetText(player1StatusText, "No recording found – please record first.");
            return;
        }

        player1Clip = TrimSilence(player1Clip);
        currentPhase = GamePhase.Player2Recording;
        ShowPhaseUI(GamePhase.Player2Recording);
    }

    // ─────────────────────────────────────────────
    // Player 2 Recording
    // ─────────────────────────────────────────────

    /// <summary>Starts a fresh recording for Player 2.</summary>
    public void StartPlayer2Recording()
    {
        if (currentPhase != GamePhase.Player2Recording) return;

        string device = Microphone.devices[0];

        // Start the actual recording that gets saved
        player2Clip = Microphone.Start(device, false, 35, 44100);
        isRecording = true;

        // Route the live mic through the AudioSource so the visualizer
        // can read spectrum data from it in real time via GetSpectrumData()
        player2AudioSource.clip = player2Clip;
        player2AudioSource.loop = true;
        StartCoroutine(StartAudioSourceWhenMicReady(player2AudioSource, device));

        SetText(player2StatusText, "Recording… Player 2, play your instrument!");
        player2StartRecordingButton?.gameObject.SetActive(false);
        player2StopRecordingButton?.gameObject.SetActive(true);
        player2ConfirmButton?.gameObject.SetActive(false);

        //checks whether the players have purchased a metronome and sets it to true if that have which will indicate the metronome script to start
        if (GameData.Instance.hasMetronome)
        {
            metronomeManager.enabled = true;
        }
    }

    /// <summary>Stops the Player 2 microphone and stores the clip.</summary>
    public void StopPlayer2Recording()
    {
        if (!isRecording) return;

        Microphone.End(null);
        isRecording = false;

        // Stop the live monitoring so the AudioSource is free for playback later
        player2AudioSource.Stop();
        player2AudioSource.loop = false;

        SetText(player2StatusText, "Recording saved! Press Confirm when you're ready.");
        player2StopRecordingButton?.gameObject.SetActive(false);
        player2ConfirmButton?.gameObject.SetActive(true);

        //stops metronome when player stops recording 
        if (metronomeManager != null)
        {
            metronomeManager.enabled = false;
        }
    }

    /// <summary>
    /// Trims the Player 2 clip and advances the game to the final playback phase.
    /// </summary>
    public void ConfirmPlayer2AndAdvance()
    {
        if (player2Clip == null)
        {
            SetText(player2StatusText, "No recording found – please record first.");
            return;
        }

        player2Clip = TrimSilence(player2Clip);
        currentPhase = GamePhase.FinalPlayback;
        ShowPhaseUI(GamePhase.FinalPlayback);
    }

    // ─────────────────────────────────────────────
    // Final Playback
    // ─────────────────────────────────────────────

    /// <summary>
    /// Plays both saved recordings simultaneously.
    /// Requires two separate AudioSource components assigned in the Inspector.
    /// </summary>
    public void PlayBothRecordings()
    {
        if (player1Clip == null || player2Clip == null)
        {
            SetText(finalStatusText, "One or both recordings are missing!");
            return;
        }

        if (player1AudioSource == null || player2AudioSource == null)
        {
            Debug.LogError("RecordMultipleAudios: Assign Player1AudioSource and Player2AudioSource in the Inspector.");
            return;
        }

        player1AudioSource.clip = player1Clip;
        player2AudioSource.clip = player2Clip;

        // PlayScheduled keeps both clips perfectly in sync
        double startTime = AudioSettings.dspTime + 0.1;
        player1AudioSource.PlayScheduled(startTime);
        player2AudioSource.PlayScheduled(startTime);

        SetText(finalStatusText, "Playing both recordings – how do they sound together?");
    }

    /// <summary>Stops simultaneous playback of both recordings.</summary>
    public void StopBothRecordings()
    {
        player1AudioSource?.Stop();
        player2AudioSource?.Stop();
        SetText(finalStatusText, "Playback stopped.");
        SceneManager.LoadScene("Moneygeneration");
    }

    // ─────────────────────────────────────────────
    // Live Microphone Monitoring
    // ─────────────────────────────────────────────

    /// <summary>
    /// Waits until the microphone has actually started capturing samples,
    /// then plays the AudioSource so the visualizer can read from it.
    /// Without this wait the AudioSource starts at position 0 before any
    /// mic data has arrived, causing the visualizer to stay flat.
    /// </summary>
    private IEnumerator StartAudioSourceWhenMicReady(AudioSource source, string device)
    {
        while (Microphone.GetPosition(device) <= 0)
            yield return null;

        source.Play();
    }

    // ─────────────────────────────────────────────
    // UI Helpers
    // ─────────────────────────────────────────────

    /// <summary>
    /// Shows the correct panel for the given phase and hides the others.
    /// Panels are optional – nothing breaks if they are not assigned.
    /// </summary>
    private void ShowPhaseUI(GamePhase phase)
    {
        player1RecordingPanel?.SetActive(phase == GamePhase.Player1Recording);
        player2RecordingPanel?.SetActive(phase == GamePhase.Player2Recording);
        finalPlaybackPanel?.SetActive(phase == GamePhase.FinalPlayback);
    }

    private void SetText(TextMeshProUGUI label, string message)
    {
        if (label != null) label.text = message;
    }

    // ─────────────────────────────────────────────
    // Utility
    // ─────────────────────────────────────────────

    /// <summary>
    /// Trims an AudioClip recorded with Microphone.Start() to remove
    /// the silent tail that Unity pre-allocates up to the max length.
    /// </summary>
    private AudioClip TrimSilence(AudioClip clip, float threshold = 0.001f)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        int lastNonSilent = samples.Length - 1;
        while (lastNonSilent > 0 && Mathf.Abs(samples[lastNonSilent]) < threshold)
            lastNonSilent--;

        int trimmedSampleCount = lastNonSilent + 1;

        // Align to whole frames
        trimmedSampleCount -= trimmedSampleCount % clip.channels;
        if (trimmedSampleCount <= 0) return clip; // completely silent – return as-is

        float[] trimmed = new float[trimmedSampleCount];
        System.Array.Copy(samples, trimmed, trimmedSampleCount);

        AudioClip trimmedClip = AudioClip.Create(
            clip.name + "_trimmed",
            trimmedSampleCount / clip.channels,
            clip.channels,
            clip.frequency,
            false
        );
        trimmedClip.SetData(trimmed, 0);
        return trimmedClip;
    }


    //Code to change f=phase after dialogue

    public void DialogueFinishedAndAdvance()
    {
        currentPhase = GamePhase.Player1Recording;
        ShowPhaseUI(GamePhase.Player1Recording);
    }
}


 