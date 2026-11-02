using UnityEngine;

public class backgroundManager : MonoBehaviour
{
    public Camera cam; // optional: drag your camera here

    [Tooltip("0 = background stays fixed vertically in the world, 1 = locked to the camera vertically")]
    [Range(0f, 1f)] public float verticalFactor = 0.5f;

    [Tooltip("How many pixel rows to average at the top and bottom edges when picking the colours")]
    public int edgeRowsToSample = 2;

    private SpriteRenderer sr;
    private float startY;
    private float camStartY;
    private Color topColor;
    private Color bottomColor;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("backgroundManager: no camera found.", this);
            enabled = false;
            return;
        }

        sr = GetComponent<SpriteRenderer>();
        startY = transform.position.y;
        camStartY = cam.transform.position.y;

        SampleEdgeColors();
        cam.clearFlags = CameraClearFlags.SolidColor;
    }

    void LateUpdate()
    {
        Vector3 c = cam.transform.position;

        // Follow the camera fully on X, partly on Y
        float y = startY + (c.y - camStartY) * verticalFactor;
        transform.position = new Vector3(c.x, y, transform.position.z);

        // Above the middle of the image, use the top colour; below it, the bottom colour
        cam.backgroundColor = c.y >= sr.bounds.center.y ? topColor : bottomColor;
    }

    public void SampleEdgeColors()
    {
        Texture2D tex = sr.sprite.texture;
        Rect r = sr.sprite.textureRect;

        int x = Mathf.RoundToInt(r.x);
        int width = Mathf.RoundToInt(r.width);
        int rows = Mathf.Clamp(edgeRowsToSample, 1, Mathf.RoundToInt(r.height));

        // Texture rows start at the bottom
        bottomColor = AverageRows(tex, x, Mathf.RoundToInt(r.y), width, rows);
        topColor = AverageRows(tex, x, Mathf.RoundToInt(r.y + r.height) - rows, width, rows);
    }

    Color AverageRows(Texture2D tex, int x, int y, int width, int rows)
    {
        Color[] pixels = tex.GetPixels(x, y, width, rows);
        Color sum = Color.clear;
        foreach (Color p in pixels) sum += p;
        Color avg = sum / pixels.Length;
        avg.a = 1f;
        return avg;
    }
}