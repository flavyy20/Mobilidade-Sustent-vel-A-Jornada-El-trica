using UnityEngine;

public class AbrirTutorialInicial : MonoBehaviour
{
    public PainelTutorial tutorial;

    void Start()
    {
        StartCoroutine(AbrirDepoisDe10s());
    }

    private System.Collections.IEnumerator AbrirDepoisDe10s()
    {
        yield return new WaitForSeconds(10f);
        tutorial.AbrirPainel();
    }
}


