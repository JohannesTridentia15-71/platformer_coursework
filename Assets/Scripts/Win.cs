using UnityEngine;

public class Win : MonoBehaviour
{
    public bool _won = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_won && other.CompareTag("Player"))
        {
            FindAnyObjectByType<GameController>()?.ShowVictoryScreen();
            _won = true; 
        }
    }

}
