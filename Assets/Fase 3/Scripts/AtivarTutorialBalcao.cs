using UnityEngine;

public class AtivarTutorialBalcao : MonoBehaviour
{
    public TutorialController tutorial;
    private bool jaAtivado = false;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (jaAtivado) return;

        // Verifica se bateu no balcão
        if (hit.collider.CompareTag("Balcao"))
        {
            jaAtivado = true;
            tutorial.AbrirTutorial();
        }
    }
}

