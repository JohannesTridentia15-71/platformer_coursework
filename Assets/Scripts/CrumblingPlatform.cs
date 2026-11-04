using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{

    public float _crumbleTimer = 2f;
    public float _destroyTimer = 0.1f;

    public float _shakeAmount = 0.06f;
    public float _fallGravity = 3f;

    bool _isFalling = false;

    Rigidbody2D rb;
    Collider2D col;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
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

        // shake strongewr
        Vector3 start = transform.position;
        float t = 0f;
        while (t < _crumbleTimer)
        {
            t += Time.deltaTime;
            float strength = _shakeAmount * Mathf.Lerp(0.25f, 1f, t / _crumbleTimer);
            transform.position = start + new Vector3(Random.Range(-1f, 1f) * strength, 0f, 0f);
            yield return null;
        }
        transform.position = start;

        // drop
        if (col != null) col.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = _fallGravity;
        Destroy(this.gameObject, _destroyTimer);
    }


}
