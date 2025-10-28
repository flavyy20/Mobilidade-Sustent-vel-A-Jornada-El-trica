using UnityEngine;

public class FollowCar : MonoBehaviour
{
    [Header("Referências")]
    public Transform target;       // o carro que será seguido
    public Vector3 offset = new Vector3(0, 5, -10); // posição relativa atrás do carro
    public float followSpeed = 5f; // suavidade do movimento
    public float rotationSpeed = 3f; // suavidade da rotação da câmera

    void LateUpdate()
    {
        if (target == null) return;

        // Define a posição desejada baseada no offset (atrás do carro)
        Vector3 desiredPosition = target.TransformPoint(offset);

        // Move suavemente a câmera até a posição desejada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Calcula a rotação para olhar suavemente na direção do carro
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

        // Suaviza a rotação da câmera
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
