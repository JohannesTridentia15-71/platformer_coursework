using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Runs after other scripts so the shake is applied on top of any camera follow
[DefaultExecutionOrder(1000)]
public class EruptionEvent : MonoBehaviour
{
    [Header("Background")]
    public SpriteRenderer background;
    public Sprite beforeSprite;
    public Sprite afterSprite;

    [Header("Timing")]
    public float delayBeforeEruption = 3f;
    public float rumbleDuration = 1f;

    [Header("Flash")]
    public Color flashColor = new Color(1f, 0.85f, 0.6f); // warm, fiery white
    public float flashFadeTime = 1.5f;

    [Header("Camera Shake")]
    public Camera cam;
    public float rumbleStrength = 0.05f;
    public float shakeStrength = 0.4f;
    public float shakeDuration = 1.2f;
    public float shakeSpeed = 25f;

    private backgroundManager bgManager;
    private Image flashImage;
    private float currentShake;
    private Vector3 lastOffset;
    private Vector3 lastShakenPos;
    private bool shakeApplied;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (background == null) background = GetComponent<SpriteRenderer>();
        bgManager = background.GetComponent<backgroundManager>();

        if (beforeSprite != null) background.sprite = beforeSprite;

        CreateFlashOverlay();
        StartCoroutine(EruptionSequence());
    }

    IEnumerator EruptionSequence()
    {
        yield return new WaitForSeconds(delayBeforeEruption);

        // Build-up: a rumble that grows stronger
        float t = 0f;
        while (t < rumbleDuration)
        {
            t += Time.deltaTime;
            currentShake = Mathf.Lerp(0f, rumbleStrength, t / rumbleDuration);
            yield return null;
        }

        // The eruption: full flash, swap the background while the screen is bright
        Color c = flashColor;
        c.a = 1f;
        flashImage.color = c;

        background.sprite = afterSprite;
        if (bgManager != null) bgManager.SampleEdgeColors();

        // Fade the flash out and let the big shake die down
        t = 0f;
        float total = Mathf.Max(flashFadeTime, shakeDuration);
        while (t < total)
        {
            t += Time.deltaTime;

            float flashProgress = Mathf.Clamp01(t / flashFadeTime);
            c.a = (1f - flashProgress) * (1f - flashProgress); // quick drop, slow tail
            flashImage.color = c;

            float shakeProgress = Mathf.Clamp01(t / shakeDuration);
            currentShake = shakeStrength * (1f - shakeProgress) * (1f - shakeProgress);

            yield return null;
        }

        flashImage.color = Color.clear;
        currentShake = 0f;
    }

    void Update()
    {
        // Remove last frame's shake so other scripts see the camera's real position
        if (shakeApplied && cam.transform.position == lastShakenPos)
            cam.transform.position -= lastOffset;
        shakeApplied = false;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 offset = Vector3.zero;
        if (currentShake > 0f)
        {
            float time = Time.time * shakeSpeed;
            offset = new Vector3(
                (Mathf.PerlinNoise(time, 0f) - 0.5f) * 2f,
                (Mathf.PerlinNoise(0f, time) - 0.5f) * 2f,
                0f) * currentShake;
        }

        cam.transform.position += offset;
        lastOffset = offset;
        lastShakenPos = cam.transform.position;
        shakeApplied = true;
    }

    void CreateFlashOverlay()
    {
        GameObject canvasObj = new GameObject("EruptionFlash");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // on top of everything

        GameObject imageObj = new GameObject("Flash");
        imageObj.transform.SetParent(canvasObj.transform, false);
        flashImage = imageObj.AddComponent<Image>();
        flashImage.raycastTarget = false;
        flashImage.color = Color.clear;

        RectTransform rt = flashImage.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}