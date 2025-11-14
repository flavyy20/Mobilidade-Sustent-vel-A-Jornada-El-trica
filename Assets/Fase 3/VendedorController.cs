using UnityEngine;

public class VendedorController : MonoBehaviour
{
    public float velocidade = 3f;
    public float rotacaoVelocidade = 200f;

    public Animator animator;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * horizontal * rotacaoVelocidade * Time.deltaTime);

        Vector3 movimento = transform.forward * vertical * velocidade;

        controller.SimpleMove(movimento);

        animator.SetBool("andando", vertical != 0);
    }
}

