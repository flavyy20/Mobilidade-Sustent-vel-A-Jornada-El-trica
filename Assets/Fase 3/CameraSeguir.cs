using UnityEngine;

public class CameraSeguir : MonoBehaviour
{
    public Transform alvo;
    public Vector3 offsetExterno = new Vector3(0, 6, -8);
    public Vector3 offsetInterno = new Vector3(0, 3, -4);

    public float suavizar = 5f;

    public bool estaDentro = false;

    void LateUpdate()
    {
        Vector3 offsetAtual = estaDentro ? offsetInterno : offsetExterno;

        Vector3 posDesejada = alvo.position + offsetAtual;
        transform.position = Vector3.Lerp(transform.position, posDesejada, Time.deltaTime * suavizar);

        transform.LookAt(alvo);
    }
}


