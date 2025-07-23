using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Static")]
    [SerializeField] private Button cream;
    [SerializeField] private Button loofah;
    [SerializeField] private Button arrowLeft;
    [SerializeField] private Button arrowRight;
    [SerializeField] private Sprite activePageSprite;
    [SerializeField] private GameObject pageLeft;
    [SerializeField] private GameObject pageRight;
    [SerializeField] private GameObject paletteGrid;
    [SerializeField] private GameObject lipstickGrid;
    [SerializeField] private GameObject tabsPanel;

    [Header("Factories")]
    [SerializeField] private PaletteFactory blushFactory;
    [SerializeField] private PaletteFactory eyeshadowsFactory;
    [SerializeField] private LipstickFactory lipstickFactory;
    [SerializeField] private TabsFactory tabsFactory;

    private PaletteButton[] blushPalette;
    private PaletteButton[] eyeshadowsPalette;
    private PaletteButton[] lipstickPalette;
    private PaletteButton[] activePaletteButtons;
    private MakeupTabButton[] tabButtons;
    private MakeupTool blushTool;
    private MakeupTool eyeshadowsTool;
    private MakeupTool activeTool;
    private MakeupTabButton activeTabButton;

    private MakeupType[] tabsOrder = { MakeupType.Blush, MakeupType.Eyeshadows, MakeupType.Lipstick };
    private int currentTab = 0;

    void OnEnable()
    {
        Game.Instance.MakeupApplier.AcneRemoved += ActivatePalette;
    }

    void OnDisable()
    {
        Game.Instance.MakeupApplier.AcneRemoved -= ActivatePalette;
    }

    private void ActivatePalette()
    {
        cream.interactable = false;

        pageLeft.GetComponent<Image>().sprite = activePageSprite;
        pageRight.GetComponent<Image>().sprite = activePageSprite;

        arrowLeft.gameObject.SetActive(true);
        arrowRight.gameObject.SetActive(true);

        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].Unlock();
        }

        SetActiveTab(MakeupType.Blush, tabButtons[0]);
    }

    void Start()
    {
        SetupButtonsListeners();

        arrowLeft.gameObject.SetActive(false);
        arrowRight.gameObject.SetActive(false);

        tabButtons = tabsFactory.CreateTabs(tabsPanel.transform);
        activeTabButton = tabButtons[0];

        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].Lock();
        }

        blushTool = blushFactory.CreateTool(pageRight);
        blushPalette = blushFactory.CreatePalette(paletteGrid);

        for (int i = 0; i < blushPalette.Length; i++)
        {
            blushTool.gameObject.SetActive(false);
            blushPalette[i].gameObject.SetActive(false);
        }

        activeTool = blushTool;
        activePaletteButtons = blushPalette.ToArray();

        eyeshadowsTool = eyeshadowsFactory.CreateTool(pageRight);
        eyeshadowsPalette = eyeshadowsFactory.CreatePalette(paletteGrid);

        for (int i = 0; i < eyeshadowsPalette.Length; i++)
        {
            eyeshadowsTool.gameObject.SetActive(false);
            eyeshadowsPalette[i].gameObject.SetActive(false);
        }

        lipstickPalette = lipstickFactory.CreateLipsticks(lipstickGrid);

        for (int i = 0; i < lipstickPalette.Length; i++)
        {
            lipstickPalette[i].gameObject.SetActive(false);
        }
    }

    private void SetupButtonsListeners()
    {
        cream.onClick.AddListener(() => Game.Instance.Hand.CreamAnimation(cream.transform));
        loofah.onClick.AddListener(() => Game.Instance.MakeupApplier.RemoveMakeup());
        arrowLeft.onClick.AddListener(() => SwitchTab(-1));
        arrowRight.onClick.AddListener(() => SwitchTab(1));
    }

    private void SwitchTab(int direction)
    {
        currentTab += direction;

        if (currentTab < 0)
        {
            currentTab = tabsOrder.Length - 1;
        }
        else if (currentTab >= tabsOrder.Length)
        {
            currentTab = 0;
        }

        SetActiveTab(tabsOrder[currentTab], tabButtons[currentTab]);
    }

    public void SetActiveTab(MakeupType makeupType, MakeupTabButton activeTab = null)
    {
        activeTabButton.Deactivate();
        activeTabButton = activeTab;
        activeTab.Activate();

        lipstickGrid.GetComponent<Image>().raycastTarget = false;
        activeTool.gameObject.SetActive(false);
        for (int i = 0; i < activePaletteButtons.Length; i++)
        {
            activePaletteButtons[i].gameObject.SetActive(false);
        }

        switch (makeupType)
        {
            case MakeupType.Blush:
                for (int i = 0; i < blushPalette.Length; i++)
                {
                    activeTool = blushTool;
                    activeTool.gameObject.SetActive(true);
                    blushPalette[i].gameObject.SetActive(true);

                    activePaletteButtons = blushPalette.ToArray();
                }
                break;
            case MakeupType.Lipstick:
                lipstickGrid.GetComponent<Image>().raycastTarget = true;
                for (int i = 0; i < lipstickPalette.Length; i++)
                {
                    lipstickPalette[i].gameObject.SetActive(true);

                    activePaletteButtons = lipstickPalette.ToArray();
                }
                break;
            case MakeupType.Eyeshadows:
                for (int i = 0; i < eyeshadowsPalette.Length; i++)
                {
                    activeTool = eyeshadowsTool;
                    activeTool.gameObject.SetActive(true);
                    eyeshadowsPalette[i].gameObject.SetActive(true);

                    activePaletteButtons = eyeshadowsPalette.ToArray();
                }
                break;
        }
    }
}
