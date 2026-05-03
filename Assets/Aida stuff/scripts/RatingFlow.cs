using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RatingFlow : MonoBehaviour
{
    [Header("Panels")]
    public GameObject player1Panel;
    public GameObject player2Panel;
    public GameObject resultsPanel;

    [Header("UI")]
    public Image fadeImage;
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;

    [Header("Ratings")]
    public StarRatingUI player1RatingUI;
    public StarRatingUI player2RatingUI;

    [Header("Extras")]
    public SimpleFrameAnimation resultsAnimation;

    // stored final values
    private float player1Rating;
    private float player2Rating;

    // PLAYER 1 CONFIRM
    public void ConfirmPlayer1()
    {
        player1Rating = player1RatingUI.GetRating();
        StartCoroutine(SwitchPanels(player1Panel, player2Panel));
    }

    // PLAYER 2 CONFIRM
    public void ConfirmPlayer2()
    {
        player2Rating = player2RatingUI.GetRating();
        StartCoroutine(SwitchPanels(player2Panel, resultsPanel));
    }

    // SHOW RESULTS
    public void ShowResults()
    {
        int p1Coins = ToCoins(player1Rating);
        int p2Coins = ToCoins(player2Rating);

        player1Text.gameObject.SetActive(true);
        player2Text.gameObject.SetActive(true);

        StartCoroutine(CountCoins(player1Text, "Player 2: " + player1Rating + " stars", p1Coins));
        StartCoroutine(CountCoins(player2Text, "Player 1: " + player2Rating + " stars", p2Coins));

        // background animation
        if (resultsAnimation != null)
            resultsAnimation.PlayAnimation();
    }

    // RATING 2 COINS
    int ToCoins(float rating)
    {
        return Mathf.RoundToInt(rating * 10f); // 5 stars = 50 coins
    }

    // COUNT UP 
    IEnumerator CountCoins(TextMeshProUGUI text, string prefix, int target)
    {
        float duration = 2f;
        float t = 0;
        int current = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            float p = t / duration; 
            current = Mathf.RoundToInt(Mathf.Lerp(0, target, p));

            text.text = $"{prefix} ({current} coins)";
            yield return null;
        }

        text.text = $"{prefix} ({target} coins)";
    }

    // PANEL SWITCHING
    IEnumerator SwitchPanels(GameObject from, GameObject to)
    {
        yield return Fade(1);

        from.SetActive(false);
        to.SetActive(true);

        yield return Fade(0);
    }

    // FADE
    IEnumerator Fade(float targetAlpha)
    {
        float start = fadeImage.color.a;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;

            float a = Mathf.Lerp(start, targetAlpha, t);
            fadeImage.color = new Color(0, 0, 0, a);

            yield return null;
        }
    }
}