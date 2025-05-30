using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    private CanvasGroup fadeImage;
    
    [SerializeField]
    public float fadeTime = 1.0f;
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
    

    public IEnumerator Fade(FadeDirection _fadeDirection)
    {
        float _targetAlpha = _fadeDirection == FadeDirection.Out ? 0 : 1;

        

        // Avvia il tween e aspetta che finisca
        Tween tween = fadeImage.DOFade(_targetAlpha, fadeTime).SetEase(Ease.InOutQuad);
        yield return tween.WaitForCompletion();

       
    }

    public IEnumerator FadeAndLoadScene(FadeDirection _fadeDirection, string _levelToLoad)
    {
        fadeImage.enabled = true;
        yield return StartCoroutine(Fade(_fadeDirection));
        SceneController.Instance.LoadAdditiveScene(_levelToLoad);
    }
    
}
