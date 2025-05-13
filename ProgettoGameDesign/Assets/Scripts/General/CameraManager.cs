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

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    CinemachineCamera currentCamera;
    CinemachinePositionComposer currentComposer;
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

    public void ShakeCamera(float intensity)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(intensity);
        }
    }
}
