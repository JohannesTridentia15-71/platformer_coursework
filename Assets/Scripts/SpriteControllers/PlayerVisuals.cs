using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private CharacterBody body;
    private SpriteRenderer sr;
    private Animator anim;

    void Awake()
    {
        body = GetComponent<CharacterBody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float xSpeed = body.Velocity.x;

        // Flip
        if (xSpeed > 0.01f) sr.flipX = false;
        else if (xSpeed < -0.01f) sr.flipX = true;

        // Animation
        anim.SetFloat("Speed", Mathf.Abs(xSpeed));
        anim.SetBool("IsGrounded", body.State == CharacterBody.CharacterStateEnum.Grounded);
    }
}