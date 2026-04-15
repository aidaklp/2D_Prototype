using UnityEngine;

public class PlaybackBars : MonoBehaviour
{
    public AudioSource audioSource;
    public RectTransform[] bars;

    public float maxHeight = 250f;
    public float sensitivity = 8f;
    public float smoothSpeed = 10f;

    private float[] samples = new float[64];

    void Update()
    {
        if (audioSource == null || !audioSource.isPlaying) return;

        audioSource.GetOutputData(samples, 0);

        for (int i = 0; i < bars.Length; i++)
        {
            int index = i * samples.Length / bars.Length;

            float value = Mathf.Abs(samples[index]) * sensitivity;

            float targetHeight = value * maxHeight;

            Vector2 size = bars[i].sizeDelta;

            size.y = Mathf.Lerp(size.y, targetHeight, Time.deltaTime * smoothSpeed);

            bars[i].sizeDelta = size;
        }
    }
}
