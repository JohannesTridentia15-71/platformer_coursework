using System.Collections;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{

    public float _crumbleTimer = 2f;
    public float _destroyTimer = 0.1f;

    public float _shakeAmount = 0.06f;
    public float _fallGravity = 3f;
    public float _pixelsPerUnit = 26f;
    public float _shakeInterval = 0.03f;

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
        float pixel = 1f / _pixelsPerUnit;
        Vector3 start = transform.position;
        start.x = Mathf.Round(start.x * _pixelsPerUnit) * pixel;
        float t = 0f;
        float nextStep = 0f;
        int lastOffset = 0;
        while (t < _crumbleTimer)
        {
            t += Time.deltaTime;
            if (t >= nextStep)
            {
                nextStep = t + _shakeInterval;
                float strength = _shakeAmount * Mathf.Lerp(0.25f, 1f, t / _crumbleTimer);
                int maxPixels = Mathf.Max(1, Mathf.RoundToInt(strength * _pixelsPerUnit));
                // always move to a different pixel so the shake is visible from the start
                int offset;
                do { offset = Random.Range(-maxPixels, maxPixels + 1); } while (offset == lastOffset);
                lastOffset = offset;
                transform.position = start + new Vector3(offset * pixel, 0f, 0f);
            }
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
