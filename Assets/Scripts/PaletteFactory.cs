using UnityEngine;
using UnityEngine.UI;

public class PaletteFactory : MonoBehaviour, IPaletteFactory
{
    [SerializeField] private MakeupType makeupType;
    [SerializeField] private Sprite[] palette;
    [SerializeField] private Sprite[] makeupSprite;
    [SerializeField] private Sprite toolSprite;
    [SerializeField] private Sprite toolMaskSprite;
    [SerializeField] private PaletteButton paletteButtonPrefab;
    [SerializeField] private MakeupTool toolPrefab;
    [SerializeField] private Vector3 toolOffset;
    [SerializeField] private Vector3 toolRotation;
    [SerializeField] private Vector3 faceOffset;

    private MakeupTool paletteTool;

    public PaletteButton[] CreatePalette(GameObject parent)
    {
        PaletteButton[] paletteButtons = new PaletteButton[palette.Length];

        for (int i = 0; i < palette.Length; i++)
        {
            paletteButtons[i] = Instantiate(paletteButtonPrefab, parent.transform);
            paletteButtons[i].SetMakeupData(makeupType, makeupSprite[i], paletteTool, toolOffset, toolRotation, faceOffset);
            paletteButtons[i].GetComponent<Image>().sprite = palette[i];
            paletteButtons[i].transform.SetParent(parent.transform);
        }

        return paletteButtons;
    }

    public MakeupTool CreateTool(GameObject parent)
    {
        paletteTool = Instantiate(toolPrefab, parent.transform);
        paletteTool.SetImages(toolSprite, toolMaskSprite);
        paletteTool.ActivateTool();

        return paletteTool;
    }
}
