using UnityEngine;

public class MicVisualizer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private RectTransform[] bars;

    [SerializeField] private float heightMultiplier = 300f;
    [SerializeField] private float smoothSpeed = 10f;

    private float[] spectrum = new float[64];

    void Start()
    {
        bars = GetComponentsInChildren<RectTransform>();

        // remove parent
        bars = System.Array.FindAll(bars, bar => bar != transform);

        // SORT by number in name
        System.Array.Sort(bars, (a, b) =>
        {
            int numA = int.Parse(a.name);
            int numB = int.Parse(b.name);
            return numA.CompareTo(numB);
        });
    }

    void Update()
    {
        if (audioSource == null) return;

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);

        int count = Mathf.Min(bars.Length, spectrum.Length);

        for (int i = 0; i < count; i++)
        {
            float intensity = spectrum[i] * heightMultiplier * 1000f;

            float targetHeight = Mathf.Max(intensity, 5f);

            float newHeight = Mathf.Lerp(
                bars[i].sizeDelta.y,
                targetHeight,
                Time.deltaTime * smoothSpeed
            );

            bars[i].sizeDelta = new Vector2(
                bars[i].sizeDelta.x,
                newHeight
            );
        }
    }
}