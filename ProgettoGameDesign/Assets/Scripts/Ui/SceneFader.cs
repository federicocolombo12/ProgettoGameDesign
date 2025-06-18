using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SceneFader : MonoBehaviour
{
    private CanvasGroup fadeImage;
    public TextMeshProUGUI textMeshPro;
   
    public static SceneFader Instance { get; private set; }

    public enum FadeDirection
    {
        In,
        Out
    }

    void Awake()
    {
        fadeImage = GetComponent<CanvasGroup>();
        fadeImage.alpha = 0f; // start transparent
       
        if (SceneFader.Instance != null && SceneFader.Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            SceneFader.Instance = this;
        }
    }
    

    public IEnumerator Fade(FadeDirection _fadeDirection, float fadeTime=1f, bool callFromRespawn=true)
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image is not assigned in SceneFader.");
            yield break;
        }
        float _targetAlpha = _fadeDirection == FadeDirection.Out ? 0 : 1;
        textMeshPro.enabled = callFromRespawn;


        // Avvia il tween e aspetta che finisca
        Tween tween = fadeImage.DOFade(_targetAlpha, fadeTime).SetEase(Ease.InOutQuad);
      
        yield return tween.WaitForCompletion();

       
    }

    public IEnumerator FadeAndLoadScene(FadeDirection _fadeDirection, ScenesToLoad scenesToLoad)
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image is not assigned in SceneFader.");
            yield break;
        }
        fadeImage.enabled = true;
        yield return StartCoroutine(Fade(_fadeDirection));
        SceneController.Instance.LoadSceneSet(scenesToLoad);
    }
    
}
