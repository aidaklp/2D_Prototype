using UnityEngine;
using UnityEngine.InputSystem;

public class UIFollowMouseX : MonoBehaviour
{
    private RectTransform rectTransform;

    // original Y 
    private float fixedY;

    // movement limits
    public float minX = -800f;
    public float maxX = 800f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Store starting Y
        fixedY = rectTransform.anchoredPosition.y;
    }

    void Update()
    {
        // mouse position 
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Convert to canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            mousePos,
            null,
            out Vector2 localPoint
        );

        // Clamp horizontal movement
        float clampedX = Mathf.Clamp(localPoint.x, minX, maxX);

        // Move only horizontally
        rectTransform.anchoredPosition = new Vector2(clampedX, fixedY);
    }
}