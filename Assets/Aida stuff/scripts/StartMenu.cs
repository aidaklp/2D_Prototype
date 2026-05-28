using System.Collections;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public RectTransform leftCurtain;
    public RectTransform rightCurtain;

    public float slideDistance = 800f; // adjust based on screen size
    public float duration = 0.5f;

    private Vector2 leftStartPos;
    private Vector2 rightStartPos;

    private bool isStarting = false;

    void Start()
    {
        leftStartPos = leftCurtain.anchoredPosition;
        rightStartPos = rightCurtain.anchoredPosition;
    }

    public void StartGame()
    {
        if (isStarting) return;
        StartCoroutine(PlayCurtainAndStart());
    }

    private IEnumerator PlayCurtainAndStart()
    {
        isStarting = true;

        Vector2 leftTarget = leftStartPos + Vector2.left * slideDistance;
        Vector2 rightTarget = rightStartPos + Vector2.right * slideDistance;

        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float progress = t / duration;

            leftCurtain.anchoredPosition = Vector2.Lerp(leftStartPos, leftTarget, progress);
            rightCurtain.anchoredPosition = Vector2.Lerp(rightStartPos, rightTarget, progress);

            yield return null;
        }

        // ensure final position
        leftCurtain.anchoredPosition = leftTarget;
        rightCurtain.anchoredPosition = rightTarget;

        gameObject.SetActive(false);
    }
}