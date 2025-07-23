using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
    public Transform Face;
    public SpriteRenderer Acne;
    public SpriteRenderer Blush;
    public SpriteRenderer Lipstick;
    public SpriteRenderer Eyeshadows;

    [Header("Tweening")]
    [SerializeField] private float tweenDuration;
    [SerializeField] private Vector3 targetPosition;

    public bool IsAcneVisible => Acne.color.a > 0f;

    void Start()
    {
        PlayStartAnimation();
    }

    private void PlayStartAnimation()
    {
        transform.DOMove(targetPosition, tweenDuration)
            .SetEase(Ease.InOutCubic);
    }

}
