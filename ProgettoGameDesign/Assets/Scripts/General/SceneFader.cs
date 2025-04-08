using NodeCanvas.Tasks.Actions;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneFader : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField] private float fadeTime = 1f;
    public enum FadeDirection
    {
        In,
        Out
    }
    void Awake()
    {
        fadeImage= GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Fade(FadeDirection _fadeDirection)
    {
        float _alpha = _fadeDirection == FadeDirection.Out ? 1 : 0;
        float _fadeEndValue = _fadeDirection == FadeDirection.Out ? 0 : 1;
        if (_fadeDirection == FadeDirection.Out)
        {
            while (_alpha > _fadeEndValue)
            {
                SetColorImage(ref _alpha, _fadeDirection);
                yield return null;
            }
            fadeImage.enabled = false;
        }
        else
        {
            fadeImage.enabled = true;
            while (_alpha < _fadeEndValue)
            {
                SetColorImage(ref _alpha, _fadeDirection);
                yield return null;
            }
        }

    }
    public IEnumerator FadeAndLoadScene(FadeDirection _fadeDirection, string _levelToLoad)
    {
        fadeImage.enabled = true;
        yield return Fade(_fadeDirection);
        SceneManager.LoadScene(_levelToLoad);
    }
    void SetColorImage(ref float _alpha, FadeDirection _fadeDirection)
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, _alpha);
        _alpha += Time.deltaTime * (1/fadeTime) * (_fadeDirection == FadeDirection.Out ? -1 : 1);
    }
}
