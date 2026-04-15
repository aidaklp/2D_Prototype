using UnityEngine;
using TMPro;
using System.Collections;

public class ResultsManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;

    [Header("Animation")]
    public SimpleFrameAnimation animationToPlay;

    [Header("Settings")]
    public int coinsPerSecond = 2;
    public float countDuration = 1.5f; //how long counting takes

    //button
    public void StartResultsSequence()
    {
        StartCoroutine(ResultsSequence());
    }

    IEnumerator ResultsSequence()
    {
        //start animation
        if (animationToPlay != null)
        {
            StartCoroutine(animationToPlay.PlayAnimationCoroutine());
        }

        //calculate scores
        int coins1 = CalculateCoins(GameResultsData.player1ClipLength);
        int coins2 = CalculateCoins(GameResultsData.player2ClipLength);

        //player 1 count
        yield return StartCoroutine(CountUp(player1Score, "Player 1: ", coins1));

        //player 2 count
        yield return StartCoroutine(CountUp(player2Score, "Player 2: ", coins2));
    }

    IEnumerator CountUp(TextMeshProUGUI textUI, string label, int targetValue)
    {
        int current = 0;
        float timer = 0f;

        textUI.text = label + "0 coins";

        while (timer < countDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / countDuration;

            current = Mathf.FloorToInt(Mathf.Lerp(0, targetValue, progress));

            textUI.text = label + current + " coins";

            yield return null;
        }

        //final value
        textUI.text = label + targetValue + " coins";
    }

    int CalculateCoins(float length)
    {
        return Mathf.FloorToInt(length * coinsPerSecond);
    }
}