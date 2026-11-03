using UnityEngine;

public class LavaOneShot : MonoBehaviour
{
    [SerializeField] private Vector2 _lavaStartPos = new Vector2(0, -40);
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
