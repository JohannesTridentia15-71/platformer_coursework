using UnityEngine;

// Makes the tiled lava look like it's flowing: the lava tile is copied into a runtime texture
[RequireComponent(typeof(SpriteRenderer))]
public class LavaSurfaceScroll : MonoBehaviour
{
    [SerializeField] private Texture2D _lavaTexture;
    [SerializeField] private float _scrollInterval = 0.08f;
    [SerializeField] private int _verticalEvery = 3;

    private SpriteRenderer _renderer;
    private Texture2D _texture;
    private Color32[] _source;
    private Color32[] _pixels;
    private int _offsetX, _offsetY, _steps;
    private float _nextScroll;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_lavaTexture == null || !_lavaTexture.isReadable)
        {
            Debug.LogWarning("LavaSurfaceScroll: lava texture missing or not readable (enable Read/Write on it).", this);
            enabled = false;
            return;
        }

        int w = _lavaTexture.width, h = _lavaTexture.height;
        _source = _lavaTexture.GetPixels32();
        _pixels = new Color32[_source.Length];
        _texture = new Texture2D(w, h, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Repeat
        };
        Redraw();

        Sprite original = _renderer.sprite;
        float ppu = original != null ? original.pixelsPerUnit : 26f;
        Vector2 size = _renderer.size;
        _renderer.sprite = Sprite.Create(_texture, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), ppu,
                                         0, SpriteMeshType.FullRect);
        _renderer.size = size;
    }

    private void Update()
    {
        if (_scrollInterval <= 0f || Time.time < _nextScroll) return;
        _nextScroll = Time.time + _scrollInterval;

        _steps++;
        _offsetX++;
        if (_verticalEvery > 0 && _steps % _verticalEvery == 0) _offsetY++;
        Redraw();
    }

    private void Redraw()
    {
        int w = _lavaTexture.width, h = _lavaTexture.height;
        for (int y = 0; y < h; y++)
        {
            int sy = ((y + _offsetY) % h) * w;
            int dy = y * w;
            for (int x = 0; x < w; x++)
            {
                _pixels[dy + x] = _source[sy + (x + _offsetX) % w];
            }
        }
        _texture.SetPixels32(_pixels);
        _texture.Apply(false);
    }

    private void OnDestroy()
    {
        if (_renderer != null && _renderer.sprite != null && _renderer.sprite.texture == _texture)
            Destroy(_renderer.sprite);
        if (_texture != null) Destroy(_texture);
    }
}
