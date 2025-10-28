using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject menuPainel;       // painel principal do menu
    public GameObject creditosPainel;   // painel de créditos
    public GameObject tutorialPainel;   // painel do tutorial

    void Start()
    {
        // Pausa o jogo ao iniciar o menu
        Time.timeScale = 0f;
    }

    public void Jogar()
    {
        // Desativa o menu e mostra o tutorial
        if (menuPainel != null)
            menuPainel.SetActive(false);

        if (tutorialPainel != null)
            tutorialPainel.SetActive(true);

        Debug.Log("Tutorial iniciado!");
    }

    public void Continuar()
    {
        // Sai do tutorial e começa o jogo
        if (tutorialPainel != null)
            tutorialPainel.SetActive(false);

        // Retoma o tempo do jogo
        Time.timeScale = 1f;

        Debug.Log("Jogo iniciado!");
    }

    public void AbrirCreditos()
    {
        if (creditosPainel != null)
            creditosPainel.SetActive(true);
    }

    public void FecharCreditos()
    {
        if (creditosPainel != null)
            creditosPainel.SetActive(false);
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Jogo encerrado.");
    }
}
