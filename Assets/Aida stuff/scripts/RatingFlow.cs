using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RatingFlow : MonoBehaviour
{
    [Header("Panels")]
    public GameObject player1Panel;
    public GameObject player2Panel;
    public GameObject DialoguePanel3;
    public GameObject resultsPanel;
    public GameObject shopPanel;

    [Header("Shop")]
    public Button goToShopButton;
    public ShopManager shopManager;

    [Header("UI")]
    public Image fadeImage;
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;

    [Header("Ratings")]
    public StarRatingUI player1RatingUI;
    public StarRatingUI player2RatingUI;

    [Header("Extras")]
    public SimpleFrameAnimation resultsAnimation;

    private float player1Rating;
    private float player2Rating;

    // what controls shop button unlock
    private int countsCompleted = 0;

    public void Start()
    {
        //  reset state when scene loads
        countsCompleted = 0;

        if (goToShopButton != null)
            goToShopButton.gameObject.SetActive(false);
    }

    // SHOP BUTTON 

    public void GoToShop()
    {
        StartCoroutine(GoToShopRoutine());
    }

    IEnumerator GoToShopRoutine()
    {
        player1Panel.SetActive(false);
        player2Panel.SetActive(false);
        DialoguePanel3.SetActive(false);

        yield return StartCoroutine(SwitchPanels(resultsPanel, shopPanel));

        if (shopManager != null)
            shopManager.UpdateCoinsDisplay();
    }

    // RATING CONFIRM 

    public void ConfirmPlayer1()
    {
        player1Rating = player1RatingUI.GetRating();
        StartCoroutine(SwitchPanels(player1Panel, player2Panel));
    }

    public void ConfirmPlayer2()
    {
        player2Rating = player2RatingUI.GetRating();
        StartCoroutine(SwitchPanels(player2Panel, DialoguePanel3));
    }

    public void ShowResults()
    {
        StartCoroutine(SwitchPanels(DialoguePanel3, resultsPanel));
    }

    //coins + shop unlock logic happens
    public void DisplayResults()
    {
        Debug.Log("DisplayResults called");

        int p1Coins = ToCoins(player1Rating);
        int p2Coins = ToCoins(player2Rating);

        player1Text.gameObject.SetActive(true);
        player2Text.gameObject.SetActive(true);

        StartCoroutine(CountCoins(player1Text, "Player 2: " + player1Rating + " stars", p1Coins));
        StartCoroutine(CountCoins(player2Text, "Player 1: " + player2Rating + " stars", p2Coins));

        if (resultsAnimation != null)
            resultsAnimation.PlayAnimation();

        // show shop button
        if (goToShopButton != null)
            goToShopButton.gameObject.SetActive(true);
    }


    int ToCoins(float rating)
    {
        return Mathf.RoundToInt(rating * 10f);
    }

    //COIN COUNT ANIMATION 

    IEnumerator CountCoins(TextMeshProUGUI text, string prefix, int target)
    {
        // prevents full system crash if UI is missing
        if (text == null)
        {
            Debug.LogError("CountCoins: missing text reference");

            countsCompleted++;
            TryShowShopButton();
            yield break;
        }

        float duration = 2f;
        float t = 0f;
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

        // adds coins to global system
        GameData.Instance.coins += target;

        // each completed coroutine contributes to unlock
        countsCompleted++;

        TryShowShopButton();
    }

    //SHOP BUTTON CHECK 

    void TryShowShopButton()
    {
        // unlock after both players finished counting
        if (countsCompleted >= 2)
        {
         
                goToShopButton.gameObject.SetActive(true);
            
        }
    }

    //panel switch

    IEnumerator SwitchPanels(GameObject from, GameObject to)
    {
        yield return Fade(1);

        // null checks prevent UI crash bugs
        if (from != null) from.SetActive(false);
        if (to != null) to.SetActive(true);

        yield return Fade(0);
    }

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