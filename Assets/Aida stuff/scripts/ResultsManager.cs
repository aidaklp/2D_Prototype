using UnityEngine;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject resultsPanel;

    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;

    [Header("Settings")]
    public int coinsPerSecond = 2;

    void Start()
    {
        ShowResults();
    }

    void ShowResults()
    {
        // [NOTE 6] No need for dialogue panel anymore (removed system dependency)
        resultsPanel.SetActive(true);

        // [NOTE 7] Read stored values from static class
        int coins1 = CalculateCoins(GameResultsData.player1ClipLength);
        int coins2 = CalculateCoins(GameResultsData.player2ClipLength);

        player1Score.text = "Player 1: " + coins1 + " coins";
        player2Score.text = "Player 2: " + coins2 + " coins";
    }

    int CalculateCoins(float clipLength)
    {
        // [NOTE 8] Core formula: time × rate
        return Mathf.FloorToInt(clipLength * coinsPerSecond);
    }
}