using DG.Tweening;
using UnityEngine;

public class WalkParticle : MonoBehaviour
{
    
    [SerializeField] private Transform groundCheck;
    // Update is called once per frame
    void Update()
    {
        transform.position = groundCheck.position;
    }
}
