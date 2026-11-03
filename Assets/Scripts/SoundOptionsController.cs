using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundOptionsController : MonoBehaviour
{
    public void OnClickBack()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
