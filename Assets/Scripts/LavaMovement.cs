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

    //true once the lava has started moving up
    public bool IsRising { get; private set; }

    void Start()
    {
            _risingVector = new Vector2(0, _risingSpeed);
            _startTime = Time.time + _startDelay;
    }


    void Update()
    {
        //during the volcano eruption the lava waits, then starts its delay once the eruption is over
        if (EruptionEvent.InProgress)
        {
            _startTime = Time.time + _startDelay;
            return;
        }

        //lava stays still until the delay has passed
        if (Time.time < _startTime)
        {
            return;
        }

        IsRising = true;
        this.transform.Translate(_risingVector * Time.deltaTime);
    }
}
