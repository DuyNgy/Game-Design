using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject tooltipObject;
    private CanvasGroup canvasGroup;
    private bool showTooltip = false;

    private void Start()
    {
        canvasGroup = tooltipObject.GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }

        // Activate tooltip object, but start hidden
        tooltipObject.SetActive(true);
        SetShow(false);  // Instead of manually disabling stuff
    }

    private void Update()
    {
        if (canvasGroup&& showTooltip)
        {
            Vector2 cursorPosition = Input.mousePosition;
            RectTransform tooltipRect = tooltipObject.GetComponent<RectTransform>();
            
            // Set pivot to bottom left
            tooltipRect.pivot = new Vector2(0, 0);

            // Apply a small offset to avoid overlapping directly under mouse
            cursorPosition += new Vector2(10f, 10f);

            tooltipObject.transform.position = cursorPosition;
        }
    }

    public void ChangeTooltip(string name, string description)
    {
        nameText.text = name;
        descriptionText.text = description;
        // Debug.Log("CHANGE TOOLTIP");
    }

    public void SetShow(bool show)
    {
        showTooltip = show;

        if (tooltipObject != null)
        {
            tooltipObject.GetComponent<Image>().enabled = show;

            foreach (Transform child in tooltipObject.transform)
            {
                child.gameObject.SetActive(show);
            }
        }
    }
}