using UnityEngine;

public class AudioVisualize : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Transform[] bars;

    [SerializeField] float heightMultiplier = 50f;
    [SerializeField] float smoothSpeed = 10f;

    float[] spectrum = new float[64];

    void Update()
    {
        if (audioSource == null || !audioSource.isPlaying) return;

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);

        for (int i = 0; i < bars.Length; i++)
        {
            float intensity = Mathf.Log(spectrum[i] + 1) * heightMultiplier;

            if (intensity < 0.01f) intensity = 0;

            Vector3 scale = bars[i].localScale;
            scale.y = Mathf.Lerp(scale.y, intensity, Time.deltaTime * smoothSpeed);
            bars[i].localScale = scale;
        }
    }
}