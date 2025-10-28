using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    public float moveSpeed = 5f;      // velocidade de movimento
    public float mouseSensitivity = 2f; // sensibilidade do mouse

    float rotationY = 0f; // controle da rotação horizontal

    void Update()
    {
        // --- Movimento com WASD (apenas no plano XZ) ---
        float moveX = Input.GetAxis("Horizontal"); // A / D
        float moveZ = Input.GetAxis("Vertical");   // W / S

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.y = 0; // impede movimentar na vertical
        transform.position += move * moveSpeed * Time.deltaTime;

        // --- Rotação da câmera com o mouse (somente horizontal) ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += mouseX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }
}
