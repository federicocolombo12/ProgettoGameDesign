using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
public class FieldOfViewGizmo : MonoBehaviour
{
    public float fieldOfView = 120f;
    public float viewDistance = 10f;
    public Color gizmoColor = new Color(1f, 1f, 0f, 0.25f); // giallo semi-trasparente

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Vector3 forward = transform.right * Mathf.Sign(transform.localScale.x);
        Quaternion leftRotation = Quaternion.Euler(0, 0, -fieldOfView / 2);
        Quaternion rightRotation = Quaternion.Euler(0, 0, fieldOfView / 2);

        Vector3 leftRay = leftRotation * forward * viewDistance;
        Vector3 rightRay = rightRotation * forward * viewDistance;

        Vector3 origin = transform.position;

        // Arco
        int segments = 30;
        float angleStep = fieldOfView / segments;
        Vector3 prevPoint = origin + leftRay;

        for (int i = 1; i <= segments; i++)
        {
            float angle = -fieldOfView / 2 + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 point = origin + (rotation * forward * viewDistance);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }

        // Raggi principali
        Gizmos.DrawLine(origin, origin + leftRay);
        Gizmos.DrawLine(origin, origin + rightRay);
    }
}
#endif
