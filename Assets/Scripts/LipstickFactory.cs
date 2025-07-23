using UnityEngine;
using UnityEngine.UI;

public class LipstickFactory : MonoBehaviour
{
    [SerializeField] private Sprite[] lipsticks;
    [SerializeField] private Sprite[] makeupSprite;
    [SerializeField] private Sprite lipstickMask;
    [SerializeField] private PaletteButton paletteButtonPrefab;
    [SerializeField] private MakeupTool makeupToolPrefab;
    [SerializeField] private Vector3 toolOffset;
    [SerializeField] private Vector3 toolRotation;
    [SerializeField] private Vector3 faceOffset;
    private MakeupType makeupType = MakeupType.Lipstick;

    public PaletteButton[] CreateLipsticks(GameObject parent)
    {
        PaletteButton[] paletteButtons = new PaletteButton[lipsticks.Length];

        for (int i = 0; i < lipsticks.Length; i++)
        {
            paletteButtons[i] = Instantiate(paletteButtonPrefab, parent.transform);

            MakeupTool tool = Instantiate(makeupToolPrefab, paletteButtons[i].transform);
            tool.SetImages(lipsticks[i], lipstickMask);
            tool.AdjustSize(lipsticks[i], lipstickMask);
            tool.ActivateTool();

            paletteButtons[i].SetMakeupData(makeupType,
                                            makeupSprite[i],
                                            tool,
                                            toolOffset,
                                            toolRotation,
                                            faceOffset);

            paletteButtons[i].GetComponent<Image>().sprite = lipsticks[i];
            paletteButtons[i].transform.SetParent(parent.transform);
        }

        return paletteButtons;
    }
}
