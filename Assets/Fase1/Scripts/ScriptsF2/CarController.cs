using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Configurações de movimento")]
    public float acceleration = 10f;
    public float maxSpeed = 15f;
    public float turnSpeed = 80f;

    [Header("Controle de ativação")]
    public bool podeControlar = false;

    private float currentSpeed = 0f;

    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;

    void Update()
    {
        if (!podeControlar) return;

        float moveInput = 0;
        float turnInput = 0;

        // --- PC
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

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

                moveInput = delta.y;
                turnInput = delta.x;
            }
        }

        // aceleração
        currentSpeed += moveInput * acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);

        // mover
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // virar
        if (Mathf.Abs(turnInput) > 0.1f)
            transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);

        // desaceleração natural
        if (moveInput == 0)
            currentSpeed = Mathf.Lerp(currentSpeed, 0, 2f * Time.deltaTime);
    }
}
