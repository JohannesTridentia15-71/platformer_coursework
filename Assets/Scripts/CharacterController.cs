using UnityEngine;
using UnityEngine.InputSystem;

//code taken from hackernoon

public class CharacterController : MonoBehaviour
{
    // reference to the character body
    [SerializeField] private CharacterBody _characterBody;

    // character movement speed
    [Min(0)]
    [SerializeField] private float _speed = 5;

    //character jump height
    [Min(0)]
    [SerializeField] private float _jumpHeight = 2.5f;

    //by how much the upward velocity decreases when releasing the jump button
    [Min(1)]
    [SerializeField] private float _stopJumpFactor = 2.5f;

    //stores the current movement velocity along the surface, or horizontally when the character is
    //airborne
    private float _locomotionVelocity; 

    //Speed of the jump
    private float _jumpSpeed;

    //In order to check if the character is jumping, limiting the jump speed whenever appropriate
    private bool _isJumping;

    //defines the jump speed by applying the formula, altered by the gravity factor
    private void Awake()
    {
        _jumpSpeed = Mathf.Sqrt(2 * Physics2D.gravity.magnitude * _characterBody.GravityFactor * _jumpHeight);
    }

    //move define the movement by multiplying the direction by the speed
    //this is defined as a Vector2 so the context will provide a Vector2
    //itself. For example, pressing D and W will give (1,1)
    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _locomotionVelocity = value.x * _speed;
    }

    //The jump action is bound to a button, so the context starts when it is pressed and ends when it is released.
    //The tuturial separates W and Space from the jump button. This may need to be ammended in the future, as well as
    //adding the button S as a fall.
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump();
        }
        else if (context.canceled)
        {
            StopJumping();
        }
    }

    //jump method here that changed the state and calls the jump function from the other class
    private void Jump()
    {
        if (_characterBody.State == CharacterBody.CharacterState.Grounded)
        {
            _isJumping = true;
        }
        _characterBody.Jump(_jumpSpeed);
    }

    //The jump is terminated iff the the flag tells us so. It also checks if the character is moving upwards.
    //this prevents the fall from being slowed down if the jump button is being pressed. If the button is released,
    //the flag is set back to false and the vertical speed reduced by the factor.
    private void StopJumping()
    {
        var velocity = _characterBody.Velocity;

        if (_isJumping && velocity.y > 0)
        {
            _isJumping = false;
        }
        _characterBody.Velocity = new Vector2(velocity.x, velocity.y / _stopJumpFactor);
    }

 

    //at every frame, update the velocity of the character and check if character is jumping
    private void Update()
    {
        if (_characterBody.State == CharacterBody.CharacterState.Grounded)
        {
            _isJumping = false;
        }
        _characterBody.SetLocomotionVelocity(_locomotionVelocity);
    }

}
