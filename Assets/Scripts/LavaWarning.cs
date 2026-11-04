using TMPro;
using UnityEngine;

// Shows an on-screen warning the moment the lava starts rising
public class LavaWarning : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset _font;
    [SerializeField] private string _message = "THE LAVA IS RISING!";
    [SerializeField] private string _subMessage = "CLIMB!";
    [SerializeField] private Color _color = new Color(0.9f, 0.08f, 0.05f, 1f);
    [SerializeField] private float _showTime = 2.5f;
    [SerializeField] private float _fadeTime = 1f;

    private LavaMovement _lava;
    private RectTransform _group;
    private TextMeshProUGUI[] _texts;
    private float _shownAt = -1f;

    private void Start()
    {
        _lava = FindAnyObjectByType<LavaMovement>();
        CreateText();
    }

    private void Update()
    {
        if (_lava == null || _group == null) return;

        if (_shownAt < 0f)
        {
            if (_lava.IsRising)
            {
                _shownAt = Time.time;
                _group.gameObject.SetActive(true);
            }
            return;
        }

        float t = Time.time - _shownAt;

        // pulse while shown, then fade out
        _group.localScale = Vector3.one * (1f + 0.06f * Mathf.Sin(t * 10f));

        Color c = _color;
        c.a = t < _showTime ? 1f : 1f - Mathf.Clamp01((t - _showTime) / _fadeTime);
        foreach (var text in _texts) text.color = c;

        if (t > _showTime + _fadeTime)
        {
            _group.gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void CreateText()
    {
        GameObject canvasObj = new GameObject("LavaWarningCanvas");
        canvasObj.transform.SetParent(transform, false);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 900;

        GameObject groupObj = new GameObject("LavaWarning", typeof(RectTransform));
        groupObj.transform.SetParent(canvasObj.transform, false);
        _group = (RectTransform)groupObj.transform;
        _group.anchorMin = new Vector2(0f, 0.6f);
        _group.anchorMax = new Vector2(1f, 0.9f);
        _group.offsetMin = Vector2.zero;
        _group.offsetMax = Vector2.zero;

        _texts = new[]
        {
            CreateLine(_message, 72, 0.5f, 1f),
            CreateLine(_subMessage, 50, 0f, 0.45f),
        };

        groupObj.SetActive(false);
    }

    private TextMeshProUGUI CreateLine(string message, float size, float bottom, float top)
    {
        GameObject textObj = new GameObject("Line");
        textObj.transform.SetParent(_group, false);
        var text = textObj.AddComponent<TextMeshProUGUI>();
        if (_font != null) text.font = _font;
        text.text = message;
        text.fontSize = size;
        text.alignment = TextAlignmentOptions.Center;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.faceColor = Color.white;
        text.color = _color;
        text.outlineColor = Color.black;
        text.outlineWidth = 0.2f;
        text.raycastTarget = false;

        RectTransform rt = text.rectTransform;
        rt.anchorMin = new Vector2(0f, bottom);
        rt.anchorMax = new Vector2(1f, top);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        return text;
    }
}
