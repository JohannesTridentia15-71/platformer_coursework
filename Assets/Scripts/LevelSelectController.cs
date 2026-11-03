using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private Button[] _levelButtons;

    // colour used for locked buttons and their numbers
    [SerializeField] private Color _lockedColor = new Color(0.45f, 0.45f, 0.45f, 0.6f);

    private void Start()
    {
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            bool unlocked = LevelManager.Instance.IsUnlocked(i);
            Button button = _levelButtons[i];
            button.interactable = unlocked;

            if (!unlocked)
            {
                ColorBlock colors = button.colors;
                colors.disabledColor = _lockedColor;
                colors.colorMultiplier = 1f;
                button.colors = colors;
                button.transition = Selectable.Transition.ColorTint;

                foreach (Graphic label in button.GetComponentsInChildren<TMPro.TMP_Text>())
                {
                    Color c = label.color;
                    c.a = 0.4f;
                    label.color = c;
                }
            }
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

        public void OnClickLevel4()
    {
        SceneManager.LoadScene("Level 4");
    }

        public void OnClickLevel5()
    {
        SceneManager.LoadScene("Level 5");
    }
}
