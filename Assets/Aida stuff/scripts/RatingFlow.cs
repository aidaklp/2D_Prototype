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

    // stored final values
    private float player1Rating;
    private float player2Rating;

    //stores the counts as this script run one count per player so this will be used to make sure both counts happen before the shop button appears
    private int countsCompleted = 0;

    
    public void Start()
    {
        goToShopButton.gameObject.SetActive(false);


    }


    // method for going to shop
    public void GoToShop()
    {
        player1Panel.SetActive(false);
        player2Panel.SetActive(false);
        DialoguePanel3.SetActive(false);

        shopManager.UpdateCoinsDisplay();
        StartCoroutine(SwitchPanels(resultsPanel, shopPanel));
    }




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
        StartCoroutine(SwitchPanels(player2Panel, DialoguePanel3));
    }

    // SHOW RESULTS
    public void ShowResults()
    {
       
        StartCoroutine(SwitchPanels(DialoguePanel3, resultsPanel));

      
    }

    public void DisplayResults()
    {
        Debug.Log("DisplayResults called");
        //resets counts
        countsCompleted = 0;

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

        //adds the coins from previous round with the coins already collected 
        GameData.Instance.coins += target;


        //adds a count once this code has run (used to count the counts)
        countsCompleted++;

        Debug.Log("Count completed, countsCompleted = " + countsCompleted);
        //only shows button once both couns have been completed
        if (countsCompleted >= 2)
        {
            goToShopButton.gameObject.SetActive(true);
        }
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