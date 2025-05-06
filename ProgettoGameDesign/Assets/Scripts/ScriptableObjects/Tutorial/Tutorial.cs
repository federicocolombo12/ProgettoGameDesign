using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Scriptable Objects/Tutorial")]
public class Tutorial : ScriptableObject
{
    public Sprite tutorialImage;
    public string tutorialText;
    public string tutorialTitle;
}
