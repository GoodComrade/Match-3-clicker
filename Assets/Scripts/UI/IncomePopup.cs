using TMPro;
using UnityEngine;
using DG.Tweening;


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

        if(_rect != null)
            _destinationSizeDelta = _rect.sizeDelta;
    }

    public void Init(float incomeValue, Vector3 startPoint)
    {
        _rect.sizeDelta = _startSizeDelta;
        _rect.position = startPoint;
        _text.text = $"+ ${incomeValue}";
        _canvasGroup.alpha = 1f;
    }

    public void ActivatePopup()
    {
        _rect.DOSizeDelta(_destinationSizeDelta, _animationDuration).OnComplete(MovePopup);
    }

    private void MovePopup()
    {
        _rect.DOMoveY(0.5f, _animationDuration);
        _canvasGroup.DOFade(0, _animationDuration);
    }
}
