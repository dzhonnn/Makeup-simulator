using TMPro;
using UnityEngine;

public class SceneTextLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private string[] sceneVariants = new string[]
    {
        "Переезд",
        "Выпускной",
        "Свидание",
        "Фотосессия",
        "Концерт",
        "Свадьба",
        "Путешествие",
    };

    void Start()
    {
        _text.text = sceneVariants[Random.Range(0, sceneVariants.Length)];
    }
}
