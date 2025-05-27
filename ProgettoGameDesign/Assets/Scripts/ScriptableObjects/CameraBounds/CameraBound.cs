using UnityEngine;

[CreateAssetMenu(fileName = "CameraBound", menuName = "Scriptable Objects/CameraBound")]
public class CameraBound : ScriptableObject
{
    public GameObject _cameraBoundObject;
    public string sceneName;
}
