using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenesToLoad", menuName = "Scriptable Objects/ScenesToLoad")]
public class ScenesToLoad : ScriptableObject
{
    public List<string> scenesToLoad = new List<string>();
}
