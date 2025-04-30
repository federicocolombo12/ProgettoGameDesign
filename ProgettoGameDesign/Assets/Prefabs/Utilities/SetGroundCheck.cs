using UnityEngine;

public class SetGroundCheck : MonoBehaviour
{
    
    
    private void OnEnable()
    {
        PlayerTransform.OnTransform += SetGroundCheckPosition;
    }
    void SetGroundCheckPosition()
    {
        PlayerTransformation playerTransformation = Player.Instance.playerTransformation;
        transform.position = playerTransformation.GetGroundCheckPosition();
    }
}
