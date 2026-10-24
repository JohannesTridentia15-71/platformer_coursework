using UnityEngine;

public class OneShot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger has the CharacterBody component
        var player = other.GetComponent<CharacterBody>();
        if (player != null)
        {
            player.Die();
        }
    }
}
