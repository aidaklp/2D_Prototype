using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AudioVisualizer : MonoBehaviour
{
    // ─────────────────────────────────────────────
    // Inspector Settings
    // ─────────────────────────────────────────────

    [Header("Audio")]
    [Tooltip("The AudioSource to visualize. Swap this at runtime to switch between players.")]
    [SerializeField] private AudioSource targetAudioSource;

    [Header("Waveform Shape")]
    [Tooltip("Number of sample points along the waveform line. Higher = more detail.")]
    [SerializeField][Range(64, 1024)] private int sampleCount = 256;

    [Tooltip("How wide the waveform stretches across the screen (world units).")]
    [SerializeField] private float waveformWidth = 10f;

    [Tooltip("Maximum height the waveform can reach (world units).")]
    [SerializeField] private float waveformHeight = 3f;

    [Header("Smoothing")]
    [Tooltip("How quickly the waveform follows the audio. Lower = smoother, Higher = snappier.")]
    [SerializeField][Range(1f, 30f)] private float smoothSpeed = 10f;

    [Header("Appearance")]
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private Color waveformColor = Color.cyan;

    [Tooltip("Optional gradient that runs left-to-right along the line.")]
    [SerializeField] private bool useGradient = false;
    [SerializeField] private Color gradientEndColor = Color.magenta;

    // ─────────────────────────────────────────────
    // Private State
    // ─────────────────────────────────────────────

    private LineRenderer lineRenderer;
    private float[] spectrumData;
    private float[] smoothedData;

    // ─────────────────────────────────────────────
    // Unity Lifecycle
    // ─────────────────────────────────────────────

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spectrumData = new float[sampleCount];
        smoothedData = new float[sampleCount];

        SetupLineRenderer();
    }

    private void Update()
    {
        if (targetAudioSource == null) return;

        if (targetAudioSource.isPlaying)
        {
            // Grab spectrum data from the playing audio
            targetAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);
            UpdateWaveform();
        }
        else
        {
            // Smoothly return to a flat line when audio stops
            DecayToFlat();
        }
    }

    // ─────────────────────────────────────────────
    // Public API
    // ─────────────────────────────────────────────

    /// <summary>
    /// Swap the visualized AudioSource at runtime.
    /// Call this when switching from Player 1 to Player 2 playback,
    /// or pass both sources to two separate AudioVisualizer instances.
    /// </summary>
    public void SetAudioSource(AudioSource source)
    {
        targetAudioSource = source;
    }

    /// <summary>Re-apply appearance settings at runtime if you change them via code.</summary>
    public void RefreshAppearance()
    {
        SetupLineRenderer();
    }

    // ─────────────────────────────────────────────
    // Internal
    // ─────────────────────────────────────────────

    private void SetupLineRenderer()
    {
        lineRenderer.positionCount = sampleCount;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = false; // positions are local to this GameObject

        if (useGradient)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(waveformColor, 0f),
                    new GradientColorKey(gradientEndColor, 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );
            lineRenderer.colorGradient = gradient;
        }
        else
        {
            lineRenderer.startColor = waveformColor;
            lineRenderer.endColor = waveformColor;
        }
    }

    private void UpdateWaveform()
    {
        float xStart = -waveformWidth / 2f;
        float xStep = waveformWidth / (sampleCount - 1);

        for (int i = 0; i < sampleCount; i++)
        {
            // Smooth the raw spectrum value over time
            smoothedData[i] = Mathf.Lerp(
                smoothedData[i],
                spectrumData[i],
                Time.deltaTime * smoothSpeed
            );

            float x = xStart + xStep * i;
            float y = smoothedData[i] * waveformHeight;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    private void DecayToFlat()
    {
        float xStart = -waveformWidth / 2f;
        float xStep = waveformWidth / (sampleCount - 1);

        for (int i = 0; i < sampleCount; i++)
        {
            smoothedData[i] = Mathf.Lerp(smoothedData[i], 0f, Time.deltaTime * smoothSpeed);

            float x = xStart + xStep * i;
            lineRenderer.SetPosition(i, new Vector3(x, smoothedData[i] * waveformHeight, 0f));
        }
    }
}