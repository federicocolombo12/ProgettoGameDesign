using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Scriptable Objects/Tutorial")]
public class Tutorial : ScriptableObject
{
    [PreviewField(100, ObjectFieldAlignment.Left)]
    [HideLabel]
    public Sprite tutorialImage;
    [PreviewField(100, ObjectFieldAlignment.Left)]
    
    public Sprite tastoDaPremere;
    public Sprite secondTastoDaPremere;

    [Title("Titolo del Tutorial")]
    
    public string tutorialTitle;

    [Multiline(4)]
    [LabelText("Testo del Tutorial")]
    public string tutorialText;
    [Title("Eventi del Tutorial")]
    [SerializeField] public bool unlocked;
}
