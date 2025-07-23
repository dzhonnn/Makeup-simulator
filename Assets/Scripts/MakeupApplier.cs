using System;
using DG.Tweening;
using UnityEngine;

public class MakeupApplier : MonoBehaviour
{
    public Character character;
    public MakeupEffect makeupEffect;
    public Action AcneRemoved;

    [SerializeField] private float tweenDuration = 0.5f;

    Action onMakeupApplied;

    void Start()
    {
        onMakeupApplied += makeupEffect.PlayEffect;
    }
    void OnDisable()
    {
        onMakeupApplied -= makeupEffect.PlayEffect;
    }

    public void RemoveAcne()
    {
        if (character.IsAcneVisible)
        {
            character.Acne.DOColor(new Color(1, 1, 1, 0), tweenDuration);
            onMakeupApplied?.Invoke();
            AcneRemoved?.Invoke();
        }
    }

    public void ApplyLipstick(Sprite lipstickSprite)
    {
        character.Lipstick.sprite = lipstickSprite;
        character.Lipstick.DOColor(new Color(1, 1, 1, 1), tweenDuration);
        onMakeupApplied?.Invoke();
    }

    public void ApplyBlush(Sprite blushSprite)
    {
        character.Blush.sprite = blushSprite;
        character.Blush.DOColor(new Color(1, 1, 1, 1), tweenDuration);
        onMakeupApplied?.Invoke();
    }

    public void ApplyEyeshadows(Sprite eyeshadowsSprite)
    {
        character.Eyeshadows.sprite = eyeshadowsSprite;
        character.Eyeshadows.DOColor(new Color(1, 1, 1, 1), tweenDuration);
        onMakeupApplied?.Invoke();
    }

    public void RemoveMakeup()
    {
        character.Blush.DOColor(new Color(1, 1, 1, 0), tweenDuration);
        character.Lipstick.DOColor(new Color(1, 1, 1, 0), tweenDuration);
        character.Eyeshadows.DOColor(new Color(1, 1, 1, 0), tweenDuration);
    }
}
