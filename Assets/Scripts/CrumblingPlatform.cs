using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{

    public float _crumbleTimer = 2f;
    public float _destroyTimer = 0.1f;

    bool _isFalling = false;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!_isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Crumble());
        }
    }

    private IEnumerator Crumble()
    {
        _isFalling = true;
        yield return new WaitForSeconds(_crumbleTimer);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(this.gameObject, _destroyTimer);
    }


}
