using UnityEngine;
using UnityEngine.Tilemaps;

public class StickyWallFeedback : MonoBehaviour
{
    TilemapRenderer sr;
    private void OnEnable()
    {
        PlayerTransform.OnTransform += PlayerTransform_OnTransform;
        sr = GetComponent<TilemapRenderer>();
    }
    private void OnDisable()
    {
        PlayerTransform.OnTransform -= PlayerTransform_OnTransform;
    }

    private void PlayerTransform_OnTransform()
    {
        if (Player.Instance.playerTransformation.abilityType == PlayerTransformation.AbilityType.WallSlide)
        {
            // Play sticky wall feedback
            sr.material.color = Color.green;

        }
        else
        {
            // Reset sticky wall feedback
            sr.material.color = Color.white;
        }
    }
}
