using UnityEngine;
using System.Collections;

public class CameraIntro : MonoBehaviour
{
    public Transform pointA;     // posi��o inicial (vista de cima/longe)
    public Transform pointB;     // posi��o final (no player)
    public float duration = 5f;  // tempo da transi��o em segundos

    private Quaternion fixedRotation;

    void Start()
    {
        // guarda a rota��o original da c�mera
        fixedRotation = transform.rotation;

        // come�a no ponto A
        transform.position = pointA.position;

        // inicia a anima��o
        StartCoroutine(MoveCameraIntro());
    }

    IEnumerator MoveCameraIntro()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            // move posi��o suavemente
            transform.position = Vector3.Lerp(pointA.position, pointB.position, t);

            // mant�m sempre a rota��o original
            transform.rotation = fixedRotation;

            yield return null;
        }

        // garante que termina exatamente no ponto B, com a rota��o fixa
        transform.position = pointB.position;
        transform.rotation = fixedRotation;

        Debug.Log("Intro finalizada!");
    }
}
