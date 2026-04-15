using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleFrameAnimation : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 0.1f;

    private Image image;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        for (int i = 0; i < frames.Length; i++)
        {
            if (image != null)
                image.sprite = frames[i];
            else
                spriteRenderer.sprite = frames[i];

            yield return new WaitForSeconds(frameRate);
        }
    }
}