using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backgroundImage;
    public Image imageToScale;

    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    public Color hoverBackgroundColor = Color.gray;

    private Vector3 originalScale;
    private Color originalBackgroundColor;


    private void Start()
    {
        // Store the original values

        if (backgroundImage != null)    
            originalBackgroundColor = backgroundImage.color;

        if (imageToScale != null)
            originalScale = imageToScale.rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Apply hover effect

        if (imageToScale != null)
            imageToScale.rectTransform.localScale = hoverScale;
        if (backgroundImage != null)
            backgroundImage.color = hoverBackgroundColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert to original

        if (imageToScale != null)
            imageToScale.rectTransform.localScale = originalScale;
        if (backgroundImage != null)
            backgroundImage.color = originalBackgroundColor;
    }
}
