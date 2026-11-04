using System;
using TMPro;
using UnityEngine;

public class TutorialSigns : MonoBehaviour
{
    [Serializable]
    public struct Sign
    {
        public Vector2 position;
        public string text;
        public float size;
    }

    [SerializeField] private TMP_FontAsset _font;
    [SerializeField] private Color _textColor = new Color(0.9f, 0.08f, 0.05f, 1f);
    [SerializeField] private Color _outlineColor = Color.black;
    [SerializeField] private int _sortingOrder = 50;
    [SerializeField] private Sign[] _signs;

    private void Start()
    {
        if (_signs == null) return;

        foreach (Sign sign in _signs)
        {
            GameObject obj = new GameObject("Sign");
            obj.transform.SetParent(transform, false);
            obj.transform.position = new Vector3(sign.position.x, sign.position.y, 0f);

            TextMeshPro tmp = obj.AddComponent<TextMeshPro>();
            if (_font != null) tmp.font = _font;
            tmp.text = sign.text;
            tmp.fontSize = sign.size > 0 ? sign.size : 4f;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.textWrappingMode = TextWrappingModes.NoWrap;
            tmp.faceColor = Color.red;
            tmp.color = _textColor;
            tmp.outlineColor = _outlineColor;
            tmp.outlineWidth = 0.2f;
            tmp.lineSpacing = 25f;
            tmp.rectTransform.sizeDelta = new Vector2(10f, 3f);
            tmp.sortingOrder = _sortingOrder;
        }
    }
}
