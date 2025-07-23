using UnityEngine;

public interface IPaletteFactory
{
    PaletteButton[] CreatePalette(GameObject parent);
    MakeupTool CreateTool(GameObject parent);
}
