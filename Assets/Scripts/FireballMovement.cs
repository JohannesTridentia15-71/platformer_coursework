using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float _movementSpeed = 3f;

    [SerializeField] private Vector2 _movementVector;

    void Start()
    {
        _movementVector = new Vector2(_movementSpeed, 0);
    }

    void Update()
    {

        this.transform.Translate(_movementVector * Time.deltaTime);
    }
}
