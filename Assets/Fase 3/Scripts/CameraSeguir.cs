using UnityEngine;

public class CameraSeguir : MonoBehaviour
{
    [Header("Alvo e Offset")]
    public Transform alvo;                        // Vendedor
    public Vector3 offsetExterno = new Vector3(0, 6, -8);
    public Vector3 offsetInterno = new Vector3(0, 3, -4);

    [Header("Controle")]
    public float suavizar = 5f;
    public bool estaDentro = false;              // Muda entre offsets interno/externo

    void LateUpdate()
    {
        if (alvo == null) return;

        // Escolhe offset atual
        Vector3 offsetAtual = estaDentro ? offsetInterno : offsetExterno;

        // Posição desejada = posição do alvo + offset
        Vector3 posDesejada = alvo.position + offsetAtual;

        // Move a câmera suavemente
        transform.position = Vector3.Lerp(transform.position, posDesejada, Time.deltaTime * suavizar);

        // Olha levemente acima do centro do personagem (1.5 unidades)
        Vector3 lookTarget = alvo.position + Vector3.up * 1.5f;
        transform.LookAt(lookTarget);
    }
}



