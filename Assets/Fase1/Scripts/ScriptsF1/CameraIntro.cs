using UnityEngine;
using System.Collections;

public class CameraIntro : MonoBehaviour
{
    public Transform pointA;     // posição inicial (vista de cima/longe)
    public Transform pointB;     // posição final (no player)
    public float duration = 5f;  // tempo da transição em segundos

    private Quaternion fixedRotation;

    void Start()
    {
        // guarda a rotação original da câmera
        fixedRotation = transform.rotation;

        // começa no ponto A
        transform.position = pointA.position;

        // inicia a animação
        StartCoroutine(MoveCameraIntro());
    }

    IEnumerator MoveCameraIntro()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            // move posição suavemente
            transform.position = Vector3.Lerp(pointA.position, pointB.position, t);

            // mantém sempre a rotação original
            transform.rotation = fixedRotation;

            yield return null;
        }

        // garante que termina exatamente no ponto B, com a rotação fixa
        transform.position = pointB.position;
        transform.rotation = fixedRotation;

        Debug.Log("Intro finalizada!");
    }
}
