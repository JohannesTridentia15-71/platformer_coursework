using UnityEngine;
//lava behaviour
public class LavaMovement : MonoBehaviour
{
    [SerializeField] private float _risingSpeed = 0.3f;

    [SerializeField] private Vector2 _risingVector;

    void Start()
    {
            _risingVector = new Vector2(0, _risingSpeed);
    }
    
    
    void Update()
    {

        this.transform.Translate(_risingVector * Time.deltaTime);
    }
}
