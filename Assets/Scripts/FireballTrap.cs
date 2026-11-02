using UnityEngine;

public class FireballTrap : MonoBehaviour
{
[SerializeField] private GameObject _fireballPrefab; 
    
    // An offset relative to the trap's position 
    [SerializeField] private Vector2 _spawnOffset = new Vector2(-10f, 0f);

    private bool _hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_hasTriggered && other.CompareTag("Player"))
        {
            SpawnFireball();
            _hasTriggered = true; 
        }
    }

    private void SpawnFireball()
    {

            // Take the trap's current position and add the offset vector to it
            Vector2 spawnPosition = (Vector2)transform.position + _spawnOffset;

            // Instantiate the fireball at that calculated position
            Instantiate(_fireballPrefab, spawnPosition, Quaternion.identity);
    
    }
}
