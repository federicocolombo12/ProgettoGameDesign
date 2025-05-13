using UnityEngine;

using Unity.Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] CinemachineCamera[] virtualCameras;

    [Header("Camera Settings")]
    [SerializeField] private float fallPanAmount = 0.5f;
    [SerializeField] private float fallPanTime = 0.5f;
    public float fallSpeedYDampingChangeTreshhold = 0.5f;

    public bool IsLerpingYDamping { get; private set; } = false;
    public bool LerpFromPlayerFall = false;
    private Coroutine LerpYPanCoroutine;
    private Coroutine _PanCameraCoroutine;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    CinemachineCamera currentCamera;
    CinemachinePositionComposer currentComposer;
    private Vector2 _startingTrackedObjectOffset;
    private float _normYPanAmount;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
        
        impulseSource = GetComponentInChildren<CinemachineImpulseSource>();
            
    }
    void Start()
    {
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (virtualCameras[i].isActiveAndEnabled)
            {
                currentCamera = virtualCameras[i];
                currentComposer = currentCamera.GetComponent<CinemachinePositionComposer>();
                break;
            }
        }
        _normYPanAmount = currentComposer.Damping.y;
        _startingTrackedObjectOffset = currentComposer.TargetOffset;
    }
    public void LerpYDamping(bool isPlayerFalling)
    {
        LerpYPanCoroutine = StartCoroutine(LerpYDampingCoroutine(isPlayerFalling));
    }
    IEnumerator LerpYDampingCoroutine(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;
        float startDampAmount = currentComposer.Damping.y;
        float endDampAmount = 0;
        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            LerpFromPlayerFall = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
           
        }
        float elapsedTime = 0;
        while (elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;
           float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / fallPanTime);
            currentComposer.Damping.y = lerpedPanAmount;
            yield return null;
        }
        IsLerpingYDamping = false;

    }
    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos){
        _PanCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }
    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startPos = Vector2.zero;
        if (!panToStartingPos){
            switch (panDirection)
            {
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                default:
                    break;
            }
            endPos *= panDistance;
            startPos = _startingTrackedObjectOffset;
            endPos += startPos;
        }
        else
        {
            startPos = currentComposer.TargetOffset;
            endPos = _startingTrackedObjectOffset;
        }
        float elapsedTime = 0;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;
            Vector2 panLerp= Vector3.Lerp(startPos, endPos, elapsedTime / panTime);
            currentComposer.TargetOffset = panLerp;
            yield return null;
        }
    }

    public void ShakeCamera(float intensity)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(intensity);
        }
    }
}
