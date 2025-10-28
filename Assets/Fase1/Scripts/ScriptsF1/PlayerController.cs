using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform; // arrasta a câmera principal no Inspector

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
        Vector3 move = (forward * moveZ + right * moveX) * speed * Time.deltaTime;

        transform.Translate(move, Space.World);
    }
}
