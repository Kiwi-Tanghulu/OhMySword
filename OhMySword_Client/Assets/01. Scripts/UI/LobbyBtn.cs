using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
public class LobbyBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Transform panel;

    [SerializeField] private float panelChangeDuration;

    [SerializeField] private float maxBtnSize;
    [SerializeField] private float normalBtnSize;
    [SerializeField] private float btnChangeDuration;
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(maxBtnSize, btnChangeDuration).SetEase(Ease.OutCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(normalBtnSize, btnChangeDuration).SetEase(Ease.OutCubic);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        panel.DOScale(1f, panelChangeDuration).SetEase(Ease.InOutCubic);
    }

    public void OnExitBtnClick()
    {
        panel.DOScale(0f, panelChangeDuration).SetEase(Ease.InOutCubic);
    }
}
