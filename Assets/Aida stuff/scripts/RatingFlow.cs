using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RatingFlow : MonoBehaviour
{
    public GameObject player1Panel;
    public GameObject player2Panel;

    public StarRatingUI player1RatingUI;
    public StarRatingUI player2RatingUI;

    public Image fadeImage;

    public float player1Rating;
    public float player2Rating;

    public void ConfirmPlayer1()
    {
        // ALWAYS grab latest value at click time
        player1Rating = player1RatingUI.GetRating();

        StartCoroutine(SwitchToPlayer2());
    }

    public void ConfirmPlayer2()
    {
        player2Rating = player2RatingUI.GetRating();

        Debug.Log("P1: " + player1Rating + " | P2: " + player2Rating);

        // continue to next screen if needed
    }

    IEnumerator SwitchToPlayer2()
    {
        float t = 0;

        // fade out
        while (t < 1)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        player1Panel.SetActive(false);
        player2Panel.SetActive(true);

        // fade back in
        t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }
}