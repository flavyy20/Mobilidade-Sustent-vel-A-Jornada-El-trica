using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform; // arraste a câmera principal no Inspector
    public float rotationSpeed = 12f; // velocidade da rotação suave

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // direção relativa à câmera (sem inclinar no eixo Y)
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();

        // movimento relativo à câmera
        Vector3 moveDir = forward * moveZ + right * moveX;

        // Movimento
        if (moveDir.magnitude > 0.1f)
        {
            transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);

            // ROTACIONA PARA A DIREÇÃO DO MOVIMENTO
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
