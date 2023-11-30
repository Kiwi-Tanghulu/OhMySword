using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUI;
using DG.Tweening;

public class GameInfoPanel : PanelUI
{
    [SerializeField] private float panelChangeDuration;
    private Transform panelPivot;

    protected override void Awake()
    {
        base.Awake();
        panelPivot = transform.parent;
    }
    public override void Show(Transform parent = null, bool isAniamtion = false)
    {
        base.Show(parent, true);
        panelPivot.DOScale(1f, panelChangeDuration).SetEase(Ease.InOutCubic);
    }

    public override void Hide(bool isAnimation = false)
    {
        base.Hide(true);
        panelPivot.DOScale(0f, panelChangeDuration).SetEase(Ease.InOutCubic);
    }
}
