using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsPreloader : MonoBehaviour
{
    public AudioSource player1Source;
    public AudioSource player2Source;

    public void GoToResults()
    {
        // extract data from existing scene objects

        GameResultsData.player1ClipLength = GetLength(player1Source);
        GameResultsData.player2ClipLength = GetLength(player2Source);

        //switch scene AFTER storing data
        SceneManager.LoadScene("ResultsScene");
    }

    float GetLength(AudioSource source)
    {
        if (source == null || source.clip == null)
            return 0f;

        return source.clip.length;
    }
}