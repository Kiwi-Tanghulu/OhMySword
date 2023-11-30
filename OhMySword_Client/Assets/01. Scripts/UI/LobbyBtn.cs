using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using MyUI;
public class LobbyBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PanelUI panel;
    private Button button;

    [SerializeField] private float maxBtnSize;
    [SerializeField] private float normalBtnSize;
    [SerializeField] private float btnChangeDuration;

    private LobbySetting lobbySetting;
    private void Awake()
    {
        lobbySetting = FindObjectOfType<LobbySetting>();
        panel = transform.Find("PanelPivot/Panel")?.GetComponent<PanelUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => panel.Show());
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(maxBtnSize, btnChangeDuration).SetEase(Ease.OutCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(normalBtnSize, btnChangeDuration).SetEase(Ease.OutCubic);
    }
}
