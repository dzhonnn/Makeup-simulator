using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    public MakeupApplier MakeupApplier { get; private set; }
    public Hand Hand { get; private set; }
    public Character Character { get; private set; }
    public UIManager UIManager { get; private set; }

    [Header("Character")]
    [SerializeField] private Character characterPrefab;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 characterScale;

    [Header("Makeup")]
    [SerializeField] private MakeupApplier makeupApplierPrefab;
    [SerializeField] private MakeupEffect makeupEffectPrefab;

    [Header("Touch Effect")]
    [SerializeField] private GameObject touchEffectsPrefab;

    [Header("Hand")]
    [SerializeField] private Hand handPrefab;

    [Header("UI")]
    [SerializeField] private UIFactory uiFactoryPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        Character = SpawnCharacter();
        MakeupEffect makeupEffect = SpawnMakeupEffect();
        MakeupApplier = SpawnMakeupApplier(Character, makeupEffect);
        Hand = SpawnHand();
        SpawnUIFactory();

        SpawnTouchEffect();
    }

    private void SpawnUIFactory()
    {
        Instantiate(uiFactoryPrefab, Vector3.zero, Quaternion.identity);
    }

    private Hand SpawnHand()
    {
        return Instantiate(handPrefab, new Vector3(0, -10f, 0), Quaternion.identity);
    }

    private MakeupEffect SpawnMakeupEffect()
    {
        return Instantiate(makeupEffectPrefab, Vector2.zero, Quaternion.identity);
    }

    private void SpawnTouchEffect()
    {
        Instantiate(touchEffectsPrefab, Vector3.zero, Quaternion.identity);
    }

    private MakeupApplier SpawnMakeupApplier(Character character, MakeupEffect makeupEffect)
    {
        MakeupApplier makeupApplier = Instantiate(makeupApplierPrefab);
        makeupApplier.character = character;
        makeupApplier.makeupEffect = makeupEffect;

        return makeupApplier;
    }

    private Character SpawnCharacter()
    {
        Character character = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
        character.transform.localScale = characterScale;

        return character;
    }

    public void SetUIManager(UIManager uiManager) => UIManager = uiManager;
}
