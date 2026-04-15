using UnityEngine;
using TMPro; // if using TextMeshPro

public class ResultsManager : MonoBehaviour
{
    [Header("Audio Sources (from your players)")]
    public AudioSource player1Source;
    public AudioSource player2Source;

    [Header("UI")]
    public GameObject dialoguePanel;
    public GameObject resultsPanel;

    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;

    [Header("Settings")]
    public int coinsPerSecond = 2;

    // Called when button is pressed
    public void ShowResults()
    {
        dialoguePanel.SetActive(false);
        resultsPanel.SetActive(true);

        int coins1 = CalculateCoins(player1Source);
        int coins2 = CalculateCoins(player2Source);

        player1Score.text = "Player 1: " + coins1 + " coins";
        player2Score.text = "Player 2: " + coins2 + " coins";
    }

    int CalculateCoins(AudioSource source)
    {
        if (source.clip == null) return 0;

        float length = source.clip.length;

        int coins = Mathf.FloorToInt(length * coinsPerSecond);

        return coins;
    }
}