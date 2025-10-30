using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] float smoothTime = 0.2f;
    [SerializeField] Vector2 yClamp = new Vector2(-1000f, 1000f);

    Vector3 velocity;

    void Awake()
    {
        if (target == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        var desired = new Vector3(target.position.x, target.position.y, 0f) + offset;
        desired.y = Mathf.Clamp(desired.y, yClamp.x, yClamp.y);
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}