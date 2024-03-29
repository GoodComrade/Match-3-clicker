using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class IncomePopup : MonoBehaviour
{
    private TMP_Text _text;
    private RectTransform _rect;
    private CanvasGroup _canvasGroup;

    private Vector2 _destinationSizeDelta;
    private Vector2 _startSizeDelta = new Vector2(0, 0);

    private float _animationDuration = 0.5f;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _text = GetComponentInChildren<TMP_Text>();

        if (_rect != null)
            _destinationSizeDelta = _rect.sizeDelta;
    }

    public void Init(float incomeValue)
    {
        _rect.sizeDelta = _startSizeDelta;
        _text.text = $"+ ${incomeValue}";
        _canvasGroup.alpha = 1f;
    }

    public void ActivatePopup()
    {
        _rect.DOSizeDelta(_destinationSizeDelta, _animationDuration).OnComplete(MovePopup);
    }

    private void MovePopup()
    {
        _canvasGroup.DOFade(0, _animationDuration);
    }
}
