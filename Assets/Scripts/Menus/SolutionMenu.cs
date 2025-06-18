using UnityEngine;

public class SolutionMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private bool isVisible = false;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ToggleMenu()
    {
        isVisible = !isVisible;

        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;

        Time.timeScale = isVisible ? 0 : 1;
    }

    public void ResumeGame()
    {
        isVisible = false;

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Time.timeScale = 1;
    }
}
