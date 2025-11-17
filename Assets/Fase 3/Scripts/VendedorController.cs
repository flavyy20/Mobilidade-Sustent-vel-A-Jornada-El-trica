using UnityEngine;

public class VendedorController : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 7f;
    public float rotacaoSuave = 10f;

    [Header("Componentes")]
    public Animator animator;
    private CharacterController controller;

    [Header("Input")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform cameraFPS;

    [Header("Controle")]
    public bool podeMover = true;

    [Header("Debug")]
    public bool mostrarLogs = true;

    // Suavização da velocidade da animação
    private float velocidadeAnimada = 0f;
    private float suavizacaoVelocidade = 0.15f;  // ajuste fino

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null && mostrarLogs)
            Debug.LogError("[VendedorController] CharacterController não encontrado.");
    }

    private void Start()
    {
        if (animator == null && mostrarLogs)
            Debug.LogWarning("[VendedorController] Animator não atribuído.");

        if (joystick == null && mostrarLogs)
            Debug.LogError("[VendedorController] Joystick não atribuído.");

        if (cameraFPS == null && mostrarLogs)
            Debug.LogError("[VendedorController] cameraFPS não atribuído.");
    }

    private void Update()
    {
        if (!podeMover)
        {
            controller.Move(Vector3.zero);
            AtualizarVelocidadeAnimacao(0f);
            return;
        }

        if (controller == null || joystick == null || cameraFPS == null)
            return;

        // Lê o joystick
        Vector3 input = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        // Magnitude entre 0 e 1
        float inputMagnitude = Mathf.Clamp01(input.magnitude);

        // Atualiza a animação suavizada
        AtualizarVelocidadeAnimacao(inputMagnitude);

        // Se não há input, não move
        if (inputMagnitude < 0.25f)  // antes 0.05
            inputMagnitude = 0;      // evita passos falsos


        // Direção relativa à câmera
        Vector3 forward = cameraFPS.forward;
        Vector3 right = cameraFPS.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * input.z + right * input.x;

        // Movimento do CharacterController
        controller.Move(moveDir * velocidade * Time.deltaTime);

        // Rotação suave
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotacaoSuave * Time.deltaTime);
        }
    }

    private void AtualizarVelocidadeAnimacao(float alvo)
    {
        // Suaviza a transição da velocidade enviada ao Blend Tree
        velocidadeAnimada = Mathf.Lerp(velocidadeAnimada, alvo, Time.deltaTime / suavizacaoVelocidade);

        if (animator != null)
            animator.SetFloat("velocidade", velocidadeAnimada);
    }
}

