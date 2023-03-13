using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudView : MonoBehaviour
{
    public RectTransform Rect => _rect;
    [SerializeField]
    private float _moveDuration;

    private float _moveEndValue = 5;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        transform.DOKill();
        DisableCloud();
    }

    private void OnEnable()
    {
        Move();
    }

    private void Move()
    {
        transform.DOMoveX(_moveEndValue, _moveDuration).SetEase(Ease.Linear).OnComplete(DisableCloud);
    }

    private void DisableCloud()
    {
        _rect.anchoredPosition3D = Vector3.zero;
        gameObject.SetActive(false);
    }
}
