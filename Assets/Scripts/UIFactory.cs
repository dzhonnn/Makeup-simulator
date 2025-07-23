using UnityEngine;

public class UIFactory : MonoBehaviour
{
    public UIManager UIManager { get; private set; }
    [SerializeField] private Canvas canvasPrefab;

    void Start()
    {
        Canvas canvas = Instantiate(canvasPrefab, Vector3.zero, Quaternion.identity);
        canvas.worldCamera = Camera.main;

        UIManager = canvas.GetComponent<UIManager>();

        Game.Instance.SetUIManager(UIManager);
    }
}
