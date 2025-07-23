using System;
using UnityEngine;
using UnityEngine.UI;

public class TabsFactory : MonoBehaviour
{
    [SerializeField] private MakeupType[] makeupTypes;
    [SerializeField] private Sprite[] tabSprites;
    [SerializeField] private Sprite[] activeTabSprites;
    [SerializeField] private MakeupTabButton tabButtonPrefab;
    [SerializeField] private Sprite lockSprite;

    public MakeupTabButton[] CreateTabs(Transform parent)
    {
        MakeupTabButton[] tabButtons = new MakeupTabButton[makeupTypes.Length];

        for (int i = 0; i < makeupTypes.Length; i++)
        {
            tabButtons[i] = Instantiate(tabButtonPrefab, parent);
            tabButtons[i].SetMakeupType(makeupTypes[i]);
            tabButtons[i].GetComponent<Image>().sprite = tabSprites[i];
            tabButtons[i].SetTabSprites(activeTabSprites[i], tabSprites[i]);
            tabButtons[i].LockSprite.sprite = lockSprite;
        }

        return tabButtons;
    }
}
