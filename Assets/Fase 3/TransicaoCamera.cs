using UnityEngine;

public class TransicaoCamera : MonoBehaviour
{
    public Camera cameraExterna;
    public Camera cameraInterna;
    public float tempoTransicao = 3f; // tempo antes de mudar a câmera
    public GameObject vendedor; // referência ao personagem

    void Start()
    {
        // Garante que só a câmera externa está ativa no início
        cameraExterna.gameObject.SetActive(true);
        cameraInterna.gameObject.SetActive(false);

        // Começa a simulação da chegada
        StartCoroutine(ChegadaDoVendedor());
    }

    System.Collections.IEnumerator ChegadaDoVendedor()
    {
        // Aqui o vendedor pode andar até a porta, se tiver animação
        if (vendedor != null)
        {
            // Exemplo: mover o vendedor para frente lentamente
            Vector3 destino = vendedor.transform.position + vendedor.transform.forward * 5f;
            float duracao = tempoTransicao;
            float tempo = 0;

            Vector3 inicio = vendedor.transform.position;
            while (tempo < duracao)
            {
                vendedor.transform.position = Vector3.Lerp(inicio, destino, tempo / duracao);
                tempo += Time.deltaTime;
                yield return null;
            }
        }

        // Troca de câmera
        cameraExterna.gameObject.SetActive(false);
        cameraInterna.gameObject.SetActive(true);
    }
}

