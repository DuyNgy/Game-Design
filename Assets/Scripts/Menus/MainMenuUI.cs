using Project.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    void Start()
    {
        // Falls der InventoryManager persistent ist, lösche ihn zu Beginn, damit er nicht doppelt da ist
        var persistentObjects = Object.FindObjectsByType<InventoryManager>(FindObjectsSortMode.None);

        foreach (var obj in persistentObjects)
        {
            if (obj.gameObject.name == "InventoryManager")
            {
                Destroy(obj.gameObject);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Aktuelle Szene speichern
            SceneTracker.LastSceneName = SceneManager.GetActiveScene().name;

            // Neue Szene laden
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Kein Szenenname angegeben!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Methode um die vorherige gespeicherte Szene zu laden
    public void LoadPreviousScene()
    {
        if (!string.IsNullOrEmpty(SceneTracker.LastSceneName))
        {
            SceneManager.LoadScene(SceneTracker.LastSceneName);
        }
        else
        {
            Debug.LogWarning("Keine vorherige Szene gefunden.");
            // Optional: Lade Main Menu oder andere Default-Szene
            // SceneManager.LoadScene("Main Menu");
        }
    }
}
