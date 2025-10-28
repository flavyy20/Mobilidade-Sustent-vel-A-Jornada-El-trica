using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform; // arrasta a c�mera principal no Inspector

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // dire��o relativa � c�mera (sem inclinar no eixo Y)
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();

        // movimento relativo � c�mera
        Vector3 move = (forward * moveZ + right * moveX) * speed * Time.deltaTime;

        transform.Translate(move, Space.World);
    }
}
