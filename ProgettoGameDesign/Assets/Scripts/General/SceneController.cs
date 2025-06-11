using UnityEngine;
using System.Collections.Generic;
using Dev.Nicklaj.Butter;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

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






    public void LoadSceneSet(ScenesToLoad scenesToLoad)
    {
        if (scenesToLoad == null || scenesToLoad.scenesToLoad.Count == 0)
        {
            Debug.LogWarning("LoadSceneSet chiamato con un oggetto ScenesToLoad vuoto o una lista vuota.");
            return;
        }

        StartCoroutine(LoadScenesRoutine(scenesToLoad.scenesToLoad));
    }
private IEnumerator LoadScenesRoutine(List<string> scenesToLoad)
{
    List<Scene> currentlyLoaded = new List<Scene>();
    for (int i = 0; i < SceneManager.sceneCount; i++)
    {
        currentlyLoaded.Add(SceneManager.GetSceneAt(i));
        Debug.Log($"Scena attualmente caricata: {currentlyLoaded[i].name}");
    }

    List<AsyncOperation> loadingOps = new List<AsyncOperation>();

    // Avvia il caricamento delle nuove scene
    foreach (string sceneName in scenesToLoad)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            Debug.Log($"Carico scena additiva: {sceneName}");
            var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadingOps.Add(loadOp);
        }
    }

    // Aspetta che tutte le scene siano caricate
    foreach (var op in loadingOps)
    {
        yield return new WaitUntil(() => op.isDone);
    }

    // Imposta la prima scena come attiva
    string firstScene = scenesToLoad[0];
    Scene newActive = SceneManager.GetSceneByName(firstScene);
    if (newActive.IsValid())
    {
        SceneManager.SetActiveScene(newActive);
        Debug.Log($"Scena attiva impostata su: {firstScene}");
    }
    else
    {
        Debug.LogWarning("Impossibile impostare la scena attiva.");
    }

    // Scarica tutte le scene non più necessarie
    foreach (Scene scene in currentlyLoaded)
    {
        if (!scenesToLoad.Contains(scene.name))
        {
            if (scene.isLoaded)
            {
                Debug.Log($"Scarico scena: {scene.name}");
                SceneManager.UnloadSceneAsync(scene.name);
            }
        }
    }
}



       
        
    

}
