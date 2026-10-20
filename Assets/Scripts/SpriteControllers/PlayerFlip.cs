using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    private CharacterBody body;
    private SpriteRenderer sr;

    void Awake()
    {
        body = GetComponent<CharacterBody>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float xSpeed = body.Velocity.x;

        if (xSpeed > 0.01f)
        {
            sr.flipX = false; // moving right
        }
        else if (xSpeed < -0.01f)
        {
            sr.flipX = true;  // moving left
        }
    }
}