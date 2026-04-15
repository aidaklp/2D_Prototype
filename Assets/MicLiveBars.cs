using UnityEngine;

public class MicLiveBars : MonoBehaviour
{
    public AudioSource micSource;
    public RectTransform[] bars;

    public float maxHeight = 300f;
    public float smoothSpeed = 15f;

    private float[] samples = new float[64];

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone found");
            return;
        }

        string device = Microphone.devices[0];

        micSource.clip = Microphone.Start(device, true, 1, 44100);
        micSource.loop = true;

        // wait until mic starts
        while (Microphone.GetPosition(device) <= 0) { }

        micSource.Play();
    }

    void Update()
    {
        if (!micSource.isPlaying) return;

        micSource.GetOutputData(samples, 0);

        for (int i = 0; i < bars.Length; i++)
        {
            int index = i * samples.Length / bars.Length;

            float value = Mathf.Abs(samples[index]);

            float targetHeight = value * maxHeight;

            Vector2 size = bars[i].sizeDelta;

            size.y = Mathf.Lerp(size.y, targetHeight, Time.deltaTime * smoothSpeed);

            bars[i].sizeDelta = size;
        }
    }
}