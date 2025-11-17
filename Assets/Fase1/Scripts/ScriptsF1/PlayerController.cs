using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform;
    public float rotationSpeed = 12f;

    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;

    void Update()
    {
        float moveX = 0;
        float moveZ = 0;

        // --- PC (Editor)
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        // --- MOBILE
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                touchStartPos = touch.position;

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchCurrentPos = touch.position;
                Vector2 delta = (touchCurrentPos - touchStartPos).normalized;

                moveX = delta.x;
                moveZ = delta.y;
            }
        }

        // direção relativa à câmera
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 moveDir = forward * moveZ + right * moveX;

        if (moveDir.magnitude > 0.1f)
        {
            transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
