using UnityEngine;
//lava behaviour
public class LavaMovement : MonoBehaviour
{
    [SerializeField] private float _risingSpeed = 0.3f;

    //how long the lava waits before it starts rising (in seconds)
    [Min(0)]
    [SerializeField] private float _startDelay = 4f;

    [SerializeField] private Vector2 _risingVector;

    private float _startTime;

    void Start()
    {
            _risingVector = new Vector2(0, _risingSpeed);
            _startTime = Time.time + _startDelay;
    }


    void Update()
    {
        //lava stays still until the delay has passed
        if (Time.time < _startTime)
        {
            return;
        }

        this.transform.Translate(_risingVector * Time.deltaTime);
    }
}
