using UnityEngine;
using UnityEngine.UI;

public class MakeupTool : MonoBehaviour
{
    [SerializeField] private Image tool;
    [SerializeField] private Image toolMask;

    public void SetImages(Sprite _tool, Sprite _toolMask)
    {
        tool.sprite = _tool;

        toolMask.sprite = _toolMask;
    }

    public void AdjustSize(Sprite _tool, Sprite _toolMask)
    {
        tool.rectTransform.sizeDelta = _tool.rect.size;

        toolMask.rectTransform.sizeDelta = _toolMask.rect.size;
    }

    public void ActivateTool()
    {
        tool.color = new Color(1, 1, 1, 1);
        toolMask.color = new Color(0, 0, 0, 0);
    }

    public void DeactivateTool()
    {
        tool.color = new Color(0, 0, 0, 0);
        toolMask.color = new Color(1, 1, 1, 1);
    }
}
