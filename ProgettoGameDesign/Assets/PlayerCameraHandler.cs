using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour
{
    Rigidbody2D rb;
    private float fallSpeedYDampingChangeTreshhold;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fallSpeedYDampingChangeTreshhold = CameraManager.Instance.fallSpeedYDampingChangeTreshhold;

    }
    public void CameraYDamping(){
        if (rb.linearVelocity.y<fallSpeedYDampingChangeTreshhold&&!CameraManager.Instance.IsLerpingYDamping
        &&!CameraManager.Instance.LerpFromPlayerFall)
        {
            CameraManager.Instance.LerpYDamping(true);
        }
        if (rb.linearVelocity.y>=0&&!CameraManager.Instance.IsLerpingYDamping&&CameraManager.Instance.LerpFromPlayerFall)
        {
            CameraManager.Instance.LerpFromPlayerFall = false;
            CameraManager.Instance.LerpYDamping(false);
        }
    }
}
