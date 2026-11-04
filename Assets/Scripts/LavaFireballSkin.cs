using UnityEngine;

// Gives a fireball the lava texture: cuts a round sprite out of the lava tile at runtime
[RequireComponent(typeof(SpriteRenderer))]
public class LavaFireballSkin : MonoBehaviour
{
    [SerializeField] private Texture2D _lavaTexture;
    [SerializeField] private int _diameterPixels = 13;
    [SerializeField] private float _rimDarkness = 0.55f;
    [SerializeField] private float _scrollInterval = 0.08f;

    private SpriteRenderer _renderer;
    private Texture2D _texture;
    private Color32[] _source;
    private Color32[] _pixels;
    private int _offsetX, _offsetY;
    private float _nextScroll;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_lavaTexture == null || !_lavaTexture.isReadable)
        {
            Debug.LogWarning("LavaFireballSkin: lava texture missing or not readable (enable Read/Write on it).", this);
            enabled = false;
            return;
        }

        _source = _lavaTexture.GetPixels32();
        int d = _diameterPixels;
        _texture = new Texture2D(d, d, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
        _pixels = new Color32[d * d];

        Redraw();

        _renderer.sprite = Sprite.Create(_texture, new Rect(0, 0, d, d), new Vector2(0.5f, 0.5f), d);
        _renderer.color = Color.white;
    }

    private void Update()
    {
        if (_scrollInterval <= 0f || Time.time < _nextScroll) return;
        _nextScroll = Time.time + _scrollInterval;
        _offsetX++;
        _offsetY += (_offsetX % 3 == 0) ? 1 : 0;
        Redraw();
    }

    private void Redraw()
    {
        int d = _diameterPixels;
        int w = _lavaTexture.width, h = _lavaTexture.height;
        float r = d / 2f;

        for (int y = 0; y < d; y++)
        {
            for (int x = 0; x < d; x++)
            {
                float dx = x + 0.5f - r, dy = y + 0.5f - r;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);
                int i = y * d + x;

                if (dist > r)
                {
                    _pixels[i] = new Color32(0, 0, 0, 0);
                    continue;
                }

                Color32 c = _source[((y + _offsetY) % h) * w + ((x + _offsetX) % w)];
                if (dist > r - 1.2f)
                {
                    c.r = (byte)(c.r * _rimDarkness);
                    c.g = (byte)(c.g * _rimDarkness);
                    c.b = (byte)(c.b * _rimDarkness);
                }
                c.a = 255;
                _pixels[i] = c;
            }
        }

        _texture.SetPixels32(_pixels);
        _texture.Apply(false);
    }

    private void OnDestroy()
    {
        if (_texture != null) Destroy(_texture);
        if (_renderer != null && _renderer.sprite != null) Destroy(_renderer.sprite);
    }
}
