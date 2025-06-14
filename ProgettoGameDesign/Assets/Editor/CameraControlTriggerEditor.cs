using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraControlTrigger))]
public class CameraControlTriggerEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var customObjects = cameraControlTrigger.customInspectorObjects;

        if (customObjects.swapCameras)
        {
            customObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera On Left", customObjects.cameraOnLeft, typeof(CinemachineCamera), true) as CinemachineCamera;
            customObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera On Right", customObjects.cameraOnRight, typeof(CinemachineCamera), true) as CinemachineCamera;
        }

        if (customObjects.panCameraOnContact)
        {
            customObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Pan Direction", customObjects.panDirection);
            customObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", customObjects.panDistance);
            customObjects.panTime = EditorGUILayout.FloatField("Pan Time", customObjects.panTime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }
}
