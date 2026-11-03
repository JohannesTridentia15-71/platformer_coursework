using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private Button[] _levelButtons;

    private void Start()
    {
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            _levelButtons[i].interactable = LevelManager.Instance.IsUnlocked(i);
        }
    }
    public void OnClickBack()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnClickTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

        public void OnClickLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

        public void OnClickLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

        public void OnClickLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }
}
