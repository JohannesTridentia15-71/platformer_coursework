using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnClickPlay()
    {
        SceneManager.LoadScene("Level Select");
    }

    public void OnClickQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void OnClickSound()
    {
        SceneManager.LoadScene("Sound Options");
    }


}
