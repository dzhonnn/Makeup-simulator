using UnityEngine;

public class MakeupEffect : MonoBehaviour
{
    private const float EffectDuration = 2f;
    [SerializeField] private GameObject makeupEffectPrefab;
    [SerializeField] private Vector3 spawnPoint;

    public void PlayEffect()
    {
        GameObject effect = Instantiate(makeupEffectPrefab, spawnPoint, Quaternion.identity);
        Destroy(effect, EffectDuration);
    }
}
