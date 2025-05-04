using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public SceneFader sceneFader;
    private TransformationUi transformationUi;
    private void OnEnable()
    {
        PlayerTransform.OnTransform+=HandleTransform;
    }
    private void OnDisable()
    {
        PlayerTransform.OnTransform-=HandleTransform;
    }
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
        transformationUi = GetComponentInChildren<TransformationUi>();
    }
    private void HandleTransform()
    {
        transformationUi.SetNextImage(Player.Instance.playerTransformation.index);
    }
   
    
}
