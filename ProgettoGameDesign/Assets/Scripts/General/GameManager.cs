using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Nicklaj.Butter;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;
    public static GameManager Instance { get; private set; }
    
    public Vector2 platformRespawnPoint;
    public Vector2 respawnPoint;
    public string currentGameplayScene;
    bool loaded;
    
    [SerializeField] ScenesToLoad gameplayScenesToLoad;
    [SerializeField] Bench currentBench;
    [SerializeField] Bench[] benchList;
    [SerializeField] GameEvent saveGameEvent;
    [SerializeField] GameEvent loadGameEvent;
    [SerializeField] GameObjectRuntimeSet benchSet;
    [SerializeField] IntVariable benchIndexVariable;
    [SerializeField] bool debugFinished = false; // Set to true for debugging purposes

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
    private void Start()
    {
        
        
        Debug.Log(saveGameEvent);
    }
    void OnEnable()
    {
        SceneController.OnSceneLoaded+= OnSceneLoaded;
    }

    private void OnSceneLoaded()
    {
        loaded = true;
    }

    void OnDisable()
    {
        SceneController.OnSceneLoaded -= OnSceneLoaded;
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

        yield return new WaitUntil(() => loaded);
        loadGameEvent.Raise();

        for (int i = 0; i < benchSet.Items.Count; i++)
        {
            Debug.Log(benchSet.Items[i].GetComponent<Bench>().benchIndex);
            if (benchSet.Items[i].GetComponent<Bench>().benchIndex == benchIndexVariable.Value)
            {
                Debug.Log("Found bench with index: " + benchIndexVariable.Value);
                currentBench = benchSet.Items[i].GetComponent<Bench>();
                break;
            }
        }


        respawnPoint =currentBench.transform.position;
        CameraManager.Instance.SetCamera(currentBench.benchCamera);
        player.RespawnAt(respawnPoint);
        SceneFader.Instance.StartCoroutine(SceneFader.Instance.Fade(SceneFader.FadeDirection.Out));
        loaded = false;
    }
    

    private void ResetScene()
    {

        SceneController.Instance.ReloadSpecificScenesOnly(gameplayScenesToLoad);

       
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
    public void SetBench()
    {

        Debug.Log("Save Game Event Raised" + saveGameEvent.name);
        saveGameEvent.Raise();


    }
    







}
