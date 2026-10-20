using UnityEngine;
using System;
// code obtained from hackernoon tutorial
public class CharacterBody : MonoBehaviour
{
    //SerializeField basically allows unity to represent private vars in the editor
    [SerializeField] private Rigidbody2D _rigidbody;
    //makes gravity act stronger than usual due to some dynamics of character movement
    //this factor cannot be less than 0
    [Min(0)]
    [field: SerializeField] public float GravityFactor { get; private set; } = 1f;
    //defines which objects are impassable surfaces
    [SerializeField] private LayerMask _solidLayers;
    //set max speed so that it isn't affected by things such as gravity; min speed is 0
    [Min(0)]
    [SerializeField] private float _maxSpeed = 30;
    //character clings to a surface if the distance between them is significantly small
    [Min(0)]
    [SerializeField] private float _surfaceAnchor = 0.01f;
    //Defines maximum slope of surface the character can stand in without falling.
    //redundant since there are only horizontal surfaces, but good to have
    [Range(0,90)]
    [SerializeField] private float _maxSlope = 45f;
    [SerializeField] private Vector2 _velocity;

    //used in the velocity getters and setter in order to reduce the computational impact
    //of calculating the square root
    private float _sqrMaxSpeed;
    private float _minGroundVertical;

    //using Unity's Slide() method that simplifies implementing the physics for the character
    private Rigidbody2D.SlideMovement _slideMovement;

    //An event that responds instantly to landing, [0] is the previous event and [1] is the current state
    public event Action<CharacterStateEnum, CharacterStateEnum> StateChanged;

    [field: SerializeField] private CharacterStateEnum _state;

    private void Awake()
    {
        _minGroundVertical = Mathf.Cos(_maxSlope * Mathf.PI / 180f);
        _sqrMaxSpeed = _maxSpeed * _maxSpeed;
        _slideMovement = CreateSlideMovement();
    }

    public enum CharacterStateEnum
    {
        Grounded,
        //Grounded: Character stays on the ground and is able to jump and move across it

        Airborne
        //Airborne: Character is airborne or free falling

    }

    //stablishes the states the character can be in
    public CharacterStateEnum State
    {
        get => _state;

        private set
        {
            if (_state != value)
            {
                var previousState = _state;
                _state = value;
                StateChanged?.Invoke(previousState, value);
            }
        }
    }

    // getter and setter for velocity, limited by the character's max velocity. Using the squared magnitude 
    // to avoid calculation costs from using the square root on the normal magnitude calculation
    public Vector2 Velocity
    {
        get => _velocity;

        //must check if velocity isn't more than the max speed allowed.
        set => _velocity = value.sqrMagnitude > _sqrMaxSpeed
            ? value.normalized * _maxSpeed
            : value;

    }

    private Rigidbody2D.SlideMovement CreateSlideMovement()
    {
        return new Rigidbody2D.SlideMovement
        {
            //determines how many times an object can change direction as a result of a collision
            maxIterations = 3,
            surfaceSlideAngle = 90,
            gravitySlipAngle = 90,
            surfaceUp = Vector2.up,
            surfaceAnchor = Vector2.down * _surfaceAnchor,
            gravity = Vector2.zero,
            layerMask = _solidLayers,
            useLayerMask = true,
        };
    }

    //calculates the speed limitation by subtracting the projection of the original velocity vector on the collision 
    //normal from the velocity normal itself
    private static Vector2 ClipVector(Vector2 vector, Vector2 hitNormal)
    {
        return vector - Vector2.Dot(vector, hitNormal) * hitNormal; 
    }

    //Sets the locomotion velocity referenced in CharacterController.cs
    //since we have no slopes, we simply preserve the vertical component of the velocity while changing only
    //the horizontal x coordinate
    public void SetLocomotionVelocity(float locomotionVelocity)
    {
        Velocity = new Vector2(locomotionVelocity, _velocity.y);
    }


    //Create a new vector of the vertical velocity in the direction of the jump and set the character state to
    //airborne
       public void Jump(float jumpSpeed)
    {
        Velocity = new Vector2(_velocity.x, jumpSpeed);
        State = CharacterStateEnum.Airborne;
        
    }


    //honestly, not sure what exactly is going on here. I understand this is used to make sure the acceleration
    //isn't weird and that his movement is smooth
    private void FixedUpdate()
    {
        //this gets the vector of gravity acting on the character at time intervals
        Velocity += Time.fixedDeltaTime * GravityFactor * Physics2D.gravity;

        var slideResults = _rigidbody.Slide(
            _velocity,
            Time.fixedDeltaTime,
            _slideMovement);
        
        if (slideResults.slideHit)
        {
            _velocity = ClipVector(_velocity, slideResults.slideHit.normal);
        }

        if (_velocity.y <= 0 && slideResults.surfaceHit)
        {
            var surfaceHit = slideResults.surfaceHit;
            Velocity = ClipVector(_velocity, surfaceHit.normal);

            if (surfaceHit.normal.y >= _minGroundVertical)
            {
                State = CharacterStateEnum.Grounded;
                return;
            }


        }
        
        State = CharacterStateEnum.Airborne;    
        
    }

    




}
