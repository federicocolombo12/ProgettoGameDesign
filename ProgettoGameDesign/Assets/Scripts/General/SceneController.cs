using UnityEngine;
using System.Collections.Generic;
using Dev.Nicklaj.Butter;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    [SerializeField] GameEvent loadGameEvent;
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



    // Update is called once per frame
    public void LoadScene(string sceneName)
    {
        loadGameEvent.Raise();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        
    }
    public void LoadAdditiveScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
