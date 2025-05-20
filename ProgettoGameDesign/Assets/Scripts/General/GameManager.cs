using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;    
    public static GameManager Instance { get; private set; }
    public Vector2 platformRespawnPoint;
    public Vector2 respawnPoint;
    public string currentGameplayScene;
    [SerializeField] Bench bench;
    
    private void Awake()
    {
        if (Instance!=null && Instance != this)
        {
            Destroy(gameObject);
           
        }
        else
            {
            Instance = this;
           
        }
        DontDestroyOnLoad(gameObject);
        

    }
   
    public void SetRespawnPoint(Vector2 newRespawnPoint)
    {
        platformRespawnPoint = newRespawnPoint;
    }


    public void RespawnPlayer(Player player)
    {
        ResetScene();
        StartCoroutine(WaitThenRespawn(player));
    }

    private IEnumerator WaitThenRespawn(Player player)
    {
        
        yield return new WaitUntil(() => SceneManager.GetSceneByName(currentGameplayScene).isLoaded);
       if (bench == null)
        {
            bench = FindFirstObjectByType<Bench>();
        }
        respawnPoint = bench.transform.position;
        player.RespawnAt(respawnPoint);
    }

    private void ResetScene()
    {
        currentGameplayScene = GetGameplaySceneName();
        SceneManager.UnloadSceneAsync(currentGameplayScene).completed += (op) =>
        {
            SceneManager.LoadScene(currentGameplayScene, LoadSceneMode.Additive);
        };
    }
    public string GetGameplaySceneName()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "Base" && scene.name != "PauseMenu" && scene.isLoaded)
            {
                return scene.name;
            }
        }
        return null;
    }
    public void SetBench(Bench newBench)
    {
        bench = newBench;
    }







}
