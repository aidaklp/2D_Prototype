using UnityEngine;

public class PlayerVoiceVisualizer : MonoBehaviour
{
    [Header("Assign THIS player's AudioSource")]
    public AudioSource audioSource;

    [Header("Assign THIS player's bars")]
    public RectTransform[] bars;

    public float maxHeight = 250f;
    public float smoothSpeed = 12f;
    public float sensitivity = 5f;

    private float[] samples = new float[64];

    void Update()
    {
        if (audioSource == null || !audioSource.isPlaying) return;

        audioSource.GetOutputData(samples, 0);

        for (int i = 0; i < bars.Length; i++)
        {
            int index = i * samples.Length / bars.Length;

            float value = Mathf.Abs(samples[index]);

            float targetHeight = value * maxHeight * sensitivity;

            Vector2 size = bars[i].sizeDelta;

            size.y = Mathf.Lerp(size.y, targetHeight, Time.deltaTime * smoothSpeed);

            bars[i].sizeDelta = size;
        }
    }
}