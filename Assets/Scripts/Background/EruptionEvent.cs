using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class EruptionEvent : MonoBehaviour
{
    // true while the eruption cutscene is playing
    public static bool InProgress { get; private set; }

    [Header("Background")]
    public SpriteRenderer background;
    public Sprite beforeSprite;
    public Sprite afterSprite;

    [Header("Timing")]
    public float delayBeforeEruption = 1.5f;
    public float rumbleDuration = 4.5f;

    [Header("Build-up")]
    public Color darkenColor = new Color(0.35f, 0.05f, 0f, 0.45f);
    public float[] foreshocks = { 0.35f, 0.7f };
    public float foreshockFlash = 0.35f;
    public float foreshockShake = 0.35f;

    [Header("Flash")]
    public Color flashColor = new Color(1f, 0.85f, 0.6f);
    public float flashFadeTime = 3.5f;
    // red glow left after the flash
    public Color afterglowColor = new Color(1f, 0.25f, 0.05f, 0.35f);
    public float afterglowFadeTime = 5f;

    [Header("Camera Shake")]
    public Camera cam;
    public float rumbleStrength = 0.12f;
    public float shakeStrength = 0.8f;
    public float shakeDuration = 3f;
    public float shakeSpeed = 25f;

    [Header("Camera Pan")]
    public bool panCamera = true;
    public float panAboveBackground = 4.5f;
    public float panUpTime = 2f;
    public float holdAfterEruption = 2f;
    public float panDownTime = 1.5f;
    public bool fadePillars = true;
    public string levelGeometryName = "Level Geometry";

    [Header("Player")]
    public bool lockPlayer = true;
    // only play once per session, restarts skip it
    public bool playOncePerSession = true;

    private static bool playedThisSession;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        playedThisSession = false;
        InProgress = false;
    }

    private backgroundManager bgManager;
    private Image tintImage;
    private Image flashImage;
    private float currentShake;
    private float panBlend;
    private System.Collections.Generic.List<SpriteRenderer> pillars = new System.Collections.Generic.List<SpriteRenderer>();
    private System.Collections.Generic.List<Color> pillarColors = new System.Collections.Generic.List<Color>();
    private float appliedFade = -1f;
    private Vector3 lastOffset;
    private Vector3 lastShakenPos;
    private bool shakeApplied;
    private CharacterController[] players;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        if (background == null) background = GetComponent<SpriteRenderer>();
        bgManager = background.GetComponent<backgroundManager>();

        if (playOncePerSession && playedThisSession)
        {
            // no cutscene
            background.sprite = afterSprite;
            if (bgManager != null) bgManager.SampleEdgeColors();
            enabled = false;
            return;
        }
        playedThisSession = true;

        if (beforeSprite != null) background.sprite = beforeSprite;

        CreateOverlays();

        if (lockPlayer)
        {
            players = FindObjectsByType<CharacterController>(FindObjectsSortMode.None);
            SetPlayersLocked(true);
        }

        InProgress = true;
        StartCoroutine(EruptionSequence());
        if (panCamera)
        {
            if (fadePillars) FindPillars();
            StartCoroutine(PanSequence());
        }
    }

    void FindPillars()
    {
        GameObject geometry = GameObject.Find(levelGeometryName);
        if (geometry != null)
        {
            foreach (var sr in geometry.GetComponentsInChildren<SpriteRenderer>()) AddPillar(sr);
        }
        foreach (var crumbling in FindObjectsByType<CrumblingPlatform>(FindObjectsSortMode.None))
        {
            AddPillar(crumbling.GetComponent<SpriteRenderer>());
        }
    }

    void AddPillar(SpriteRenderer sr)
    {
        if (sr == null) return;
        pillars.Add(sr);
        pillarColors.Add(sr.color);
    }

    // pillars fade out as the camera looks up and fade back in as it returns
    void ApplyPillarFade()
    {
        if (Mathf.Approximately(panBlend, appliedFade)) return;
        appliedFade = panBlend;

        for (int i = 0; i < pillars.Count; i++)
        {
            if (pillars[i] == null) continue;
            Color c = pillarColors[i];
            c.a *= 1f - panBlend;
            pillars[i].color = c;
        }
    }

    IEnumerator PanSequence()
    {
        yield return new WaitForSeconds(delayBeforeEruption);

        for (float t = 0f; t < panUpTime; t += Time.deltaTime)
        {
            panBlend = Mathf.SmoothStep(0f, 1f, t / panUpTime);
            yield return null;
        }
        panBlend = 1f;

        yield return new WaitForSeconds(Mathf.Max(0f, rumbleDuration - panUpTime) + holdAfterEruption);

        for (float t = 0f; t < panDownTime; t += Time.deltaTime)
        {
            panBlend = Mathf.SmoothStep(1f, 0f, t / panDownTime);
            yield return null;
        }
        panBlend = 0f;
    }

    void OnDisable()
    {
        InProgress = false;
        SetPlayersLocked(false);
    }

    void SetPlayersLocked(bool locked)
    {
        if (players == null) return;
        foreach (var p in players)
        {
            if (p != null) p.SetControlsLocked(locked);
        }
    }

    IEnumerator EruptionSequence()
    {
        yield return new WaitForSeconds(delayBeforeEruption);

        int nextShock = 0;
        float shockShake = 0f;
        float shockFlash = 0f;
        float t = 0f;
        while (t < rumbleDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / rumbleDuration);

            if (foreshocks != null && nextShock < foreshocks.Length && p >= foreshocks[nextShock])
            {
                shockShake = foreshockShake;
                shockFlash = foreshockFlash;
                nextShock++;
            }
            shockShake = Mathf.MoveTowards(shockShake, 0f, Time.deltaTime * foreshockShake * 2.5f);
            shockFlash = Mathf.MoveTowards(shockFlash, 0f, Time.deltaTime * 1.5f);

            currentShake = rumbleStrength * p * p + shockShake;

            Color tint = darkenColor;
            tint.a = darkenColor.a * p;
            tintImage.color = tint;

            Color f = flashColor;
            f.a = shockFlash;
            flashImage.color = f;

            yield return null;
        }

        Color c = flashColor;
        c.a = 1f;
        flashImage.color = c;

        background.sprite = afterSprite;
        if (bgManager != null) bgManager.SampleEdgeColors();

        t = 0f;
        float panBackDone = panCamera ? holdAfterEruption + panDownTime : 0f;
        float total = Mathf.Max(flashFadeTime, shakeDuration, afterglowFadeTime, panBackDone);
        bool released = false;
        while (t < total)
        {
            t += Time.deltaTime;

            float flashProgress = Mathf.Clamp01(t / flashFadeTime);
            c.a = (1f - flashProgress) * (1f - flashProgress);
            flashImage.color = c;

            float shakeProgress = Mathf.Clamp01(t / shakeDuration);
            currentShake = shakeStrength * (1f - shakeProgress) * (1f - shakeProgress);

            float glowProgress = Mathf.Clamp01(t / afterglowFadeTime);
            Color glow = afterglowColor;
            glow.a = afterglowColor.a * (1f - glowProgress);
            tintImage.color = glow;

            if (!released && t >= shakeDuration && t >= panBackDone)
            {
                released = true;
                SetPlayersLocked(false);
                InProgress = false;
            }

            yield return null;
        }

        flashImage.color = Color.clear;
        tintImage.color = Color.clear;
        currentShake = 0f;
        SetPlayersLocked(false);
        InProgress = false;
    }

    void Update()
    {
        if (shakeApplied && cam.transform.position == lastShakenPos)
            cam.transform.position -= lastOffset;
        shakeApplied = false;
    }

    void LateUpdate()
    {
        if (pillars.Count > 0) ApplyPillarFade();
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

        if (panBlend > 0f)
        {
            float targetY = background.transform.position.y + panAboveBackground;
            offset.y += (targetY - cam.transform.position.y) * panBlend;
        }

        cam.transform.position += offset;
        lastOffset = offset;
        lastShakenPos = cam.transform.position;
        shakeApplied = true;
    }

    void CreateOverlays()
    {
        GameObject canvasObj = new GameObject("EruptionFlash");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // on top of everything

        tintImage = CreateFullScreenImage("Tint", canvasObj.transform);
        flashImage = CreateFullScreenImage("Flash", canvasObj.transform);
    }

    Image CreateFullScreenImage(string name, Transform parent)
    {
        GameObject imageObj = new GameObject(name);
        imageObj.transform.SetParent(parent, false);
        Image img = imageObj.AddComponent<Image>();
        img.raycastTarget = false;
        img.color = Color.clear;

        RectTransform rt = img.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        return img;
    }
}
