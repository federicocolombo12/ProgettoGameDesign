using System;
using UnityEngine;

public class StartBossfight : MonoBehaviour
{
    public static Action OnBossfightStart;
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnBossfightStart?.Invoke();
        Destroy(gameObject);
    }
}
