using UnityEditor;
using UnityEngine;

public static class ProgressMenu
{
    [MenuItem("Tools/Reset Level Progress")]
    private static void ResetProgress()
    {
        if (Application.isPlaying)
        {
            LevelManager.Instance.ResetProgress();
            Debug.Log("Level progress reset for this session.");
        }
        else
        {
            Debug.Log("Progress is not saved between sessions, so it already resets every time Play starts.");
        }
    }
}
