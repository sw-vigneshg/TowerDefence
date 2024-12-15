using UnityEngine;
using UnityEngine.UI; 

public class GunSelectionHandler : MonoBehaviour
{
    [SerializeField] private Button _BuyButton;
    [SerializeField] private int _GunValue;
    [SerializeField] private int _GunIndex;
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private CanvasGroup _CanvasGroup;

    private void Awake()
    {
        _BuyButton.onClick.AddListener(OnClickBuy);
        _CanvasGroup.alpha = 1f;
    }

    private void OnClickBuy()
    {
        if (!_GameManager.CanBuy) return;
        _GameManager.CanBuy = false;
        _BuyButton.interactable = false;
        _CanvasGroup.alpha = 0.5f;
        _GameManager.OnGoldValueChanges(_GunValue, false);
        _GameManager.SpawnGun(_GunIndex);
        CancelInvoke(nameof(EnableButton));
        Invoke(nameof(EnableButton), 5);
    }

    private void EnableButton()
    {
        _BuyButton.interactable = true;
        _CanvasGroup.alpha = 1f;
    }
}
