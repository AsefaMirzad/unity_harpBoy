using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectManager : MonoBehaviour
{
    public static HitEffectManager Instance;

    public GameObject PerfectEffectPrefab;
    public GameObject GoodEffectPrefab;
    public GameObject MissEffectPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowPerfect(Vector3 position)
    {
        Instantiate(PerfectEffectPrefab, position, Quaternion.identity);
    }

    public void ShowGood(Vector3 position)
    {
        Instantiate(GoodEffectPrefab, position, Quaternion.identity);
    }

    public void ShowMiss(Vector3 position)
    {
        Instantiate(MissEffectPrefab, position, Quaternion.identity);
    }
}
