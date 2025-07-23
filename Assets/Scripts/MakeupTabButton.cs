using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MakeupTabButton : MonoBehaviour
{
    public Image LockSprite;
    private const float Duration = 0.5f;
    [SerializeField] private Button button;
    private MakeupType makeupType;
    private Sprite activeTabImage;
    private Sprite inactiveTabImage;

    void Start() => button.onClick.AddListener(OnTabButtonClick);

    public void SetMakeupType(MakeupType _makeupType) => makeupType = _makeupType;

    public void SetTabSprites(Sprite _activeTabImage, Sprite _inactiveTabImage)
    {
        activeTabImage = _activeTabImage;
        inactiveTabImage = _inactiveTabImage;
    }

    public void Unlock()
    {
        button.interactable = true;
        LockSprite.DOColor(new Color(1, 1, 1, 0), Duration);
    }

    public void Lock()
    {
        button.interactable = false;
        LockSprite.DOColor(new Color(1, 1, 1, 1), Duration);
    }

    public void Deactivate() => GetComponent<Image>().sprite = inactiveTabImage;

    public void Activate() => GetComponent<Image>().sprite = activeTabImage;

    private void OnTabButtonClick() => Game.Instance.UIManager.SetActiveTab(makeupType, this);
}
