using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Configurações de movimento")]
    public float acceleration = 10f;   // quão rápido acelera
    public float maxSpeed = 15f;       // limite de velocidade
    public float turnSpeed = 80f;      // velocidade de rotação

    [Header("Controle de ativação")]
    public bool podeControlar = false; // só verdadeiro após troca de câmera/fase

    private float currentSpeed = 0f;   // velocidade atual

    void Update()
    {
        // Só permite controle se a flag estiver ativa
        if (!podeControlar) return;

        // Entrada do jogador
        float moveInput = Input.GetAxis("Vertical");   // W/S
        float turnInput = Input.GetAxis("Horizontal"); // A/D

        // Acelera gradualmente
        currentSpeed += moveInput * acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);

        // Move o carro para frente (no eixo local)
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Rotação suave
        if (Mathf.Abs(turnInput) > 0.1f)
        {
            transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);
        }

        // Atrito natural
        if (moveInput == 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, 2f * Time.deltaTime);
        }
    }
}
