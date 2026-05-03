using UnityEngine;
using UnityEngine.UI;

public class StarRatingUI : MonoBehaviour
{
    public Image starsImage;
    public Sprite defaultSprite;
    public Sprite[] ratingSprites; // 0.5 to 5.0 

    public GameObject confirmButton;

    private bool hasRated = false;
    private float currentRating = 0f;

    void Start()
    {
        starsImage.sprite = defaultSprite;
        confirmButton.SetActive(false);
    }

    public void SetRating(float value)
    {
        currentRating = value;

        int index = Mathf.RoundToInt(value * 2) - 1;
        starsImage.sprite = ratingSprites[index];

        if (!hasRated)
        {
            hasRated = true;
            confirmButton.SetActive(true);
        }
    }

    public float GetRating()
    {
        return currentRating;
    }
}