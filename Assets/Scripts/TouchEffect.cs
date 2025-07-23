using System;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    private const float EffectDuration = 2f;
    public GameObject starsEffectPrefab;
    public GameObject heartEffectPrefab;

    void Update()
    {
        Vector2 touchPosition = GetTouchPosition();

        if (touchPosition != Vector2.zero)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));

            GameObject heartEffect = Instantiate(heartEffectPrefab, worldPosition, Quaternion.identity);
            GameObject starsEffect = Instantiate(starsEffectPrefab, worldPosition, Quaternion.identity);

            Destroy(heartEffect, EffectDuration);
            Destroy(starsEffect, EffectDuration);
        }
    }

    private Vector2 GetTouchPosition()
    {
        Vector2 touchPosition = Vector2.zero;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                touchPosition = Input.GetTouch(0).position;
            }
        }

        return touchPosition;
    }
}
