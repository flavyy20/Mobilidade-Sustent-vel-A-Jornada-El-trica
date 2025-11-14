using UnityEngine;

public class EntradaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraSeguir cam = Camera.main.GetComponent<CameraSeguir>();
            cam.estaDentro = true;
        }
    }
}
