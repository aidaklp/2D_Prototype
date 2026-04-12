using UnityEngine;

public class RecordController : MonoBehaviour
{
    public GameObject recordingIcon;

    public void StartRecording()
    {
        recordingIcon.SetActive(true);
    }

    public void StopRecording()
    {
        recordingIcon.SetActive(false);
    }
}