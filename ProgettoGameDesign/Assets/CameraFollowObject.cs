using DG.Tweening;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    private Player player;
    private bool _isFacingRight;
    [SerializeField] private float _flipRotationTime = 0.5f;

    void Start()
    {
        player = Player.Instance;
        _isFacingRight = player.pState.lookingRight;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
    public void CallTurn(){
        transform.DORotate(DetermineEndRotation(), _flipRotationTime).SetEase(Ease.InOutSine);
    }
    private Vector3 DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        if (_isFacingRight)
        {
            
            return (new Vector3(0, 0, 0));
        }
        else
        {
            
            return (new Vector3(0, 180f, 0));
        }
    }
}
