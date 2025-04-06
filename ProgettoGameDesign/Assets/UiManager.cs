using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public SceneFader sceneFader;
    private void Start()
    {
        sceneFader = GetComponentInChildren<SceneFader>();
    }
}
