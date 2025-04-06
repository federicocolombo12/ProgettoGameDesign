using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public SceneFader sceneFader;
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
        sceneFader = GetComponentInChildren<SceneFader>();
    }
   
    
}
