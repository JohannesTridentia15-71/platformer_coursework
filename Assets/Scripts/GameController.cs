using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _deathScreenPanel;
    [SerializeField] private GameObject _victoryScreenPanel;

    private void Start()
    {
        // Make sure UI panels are hidden when the game starts
        if (_deathScreenPanel != null) _deathScreenPanel.SetActive(false);
        if (_victoryScreenPanel != null) _victoryScreenPanel.SetActive(false);
        
        Time.timeScale = 1f; // Ensure time is running normally
    }

    // Called when the player dies
    public void ShowDeathScreen()
    {
        if (_deathScreenPanel != null)
        {
            _deathScreenPanel.SetActive(true);
        }
        //Time.timeScale = 0f; 
    }

    
    public void ShowVictoryScreen()
    {
        if (_victoryScreenPanel != null)
        {
            _victoryScreenPanel.SetActive(true);
        }
        Time.timeScale = 0f; // Freeze game on victory
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
