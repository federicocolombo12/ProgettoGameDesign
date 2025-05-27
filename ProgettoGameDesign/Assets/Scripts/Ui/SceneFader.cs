using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    private Image fadeImage;
    
    [SerializeField]
    public float fadeTime = 1.0f;

    public enum FadeDirection
    {
        In,
        Out
    }

    void Awake()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // start transparent
    }

    public IEnumerator Fade(FadeDirection _fadeDirection)
    {
        float _targetAlpha = _fadeDirection == FadeDirection.Out ? 0 : 1;

        fadeImage.enabled = true;

        // Avvia il tween e aspetta che finisca
        Tween tween = fadeImage.DOFade(_targetAlpha, fadeTime).SetEase(Ease.InOutQuad);
        yield return tween.WaitForCompletion();

        if (_fadeDirection == FadeDirection.Out)
            fadeImage.enabled = false;
    }

    public IEnumerator FadeAndLoadScene(FadeDirection _fadeDirection, string _levelToLoad)
    {
        fadeImage.enabled = true;
        yield return StartCoroutine(Fade(_fadeDirection));
        SceneController.Instance.LoadAdditiveScene(_levelToLoad);
    }
}
