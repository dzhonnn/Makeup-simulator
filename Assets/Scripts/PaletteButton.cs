using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PaletteButton : MonoBehaviour
{
    public MakeupData makeupData;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnPaletteButtonClick);
    }

    public void SetMakeupData(MakeupType makeupType,
                              Sprite sprite,
                              MakeupTool tool,
                              Vector3 toolOffset,
                              Vector3 toolRotation,
                              Vector3 faceOffset)
    {
        makeupData = new MakeupData(makeupType,
                                    sprite,
                                    tool,
                                    toolOffset,
                                    toolRotation,
                                    faceOffset);
    }

    private void OnPaletteButtonClick() =>
        Game.Instance.Hand.GrabToolAnimation(transform, makeupData);
}
