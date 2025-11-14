using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject menuPainel;         // painel principal do menu
    public GameObject creditosPainel;     // painel de créditos
    public GameObject tutorial1Painel;    // painel do tutorial 1
    public GameObject tutorial2Painel;    // painel do tutorial 2
    public GameObject tutorial3Painel;    // painel do tutorial 3

    private int tutorialAtual = 0;        // controla em qual tutorial está

    void Start()
    {
        // Pausa o jogo ao iniciar o menu
        Time.timeScale = 0f;
    }

    public void Jogar()
    {
        // Desativa o menu e mostra o primeiro tutorial
        if (menuPainel != null)
            menuPainel.SetActive(false);

        tutorialAtual = 1;
        AtualizarTelasTutorial();

        Debug.Log("Tutorial iniciado!");
    }

    public void Continuar()
    {
        // Avança para o próximo tutorial ou inicia o jogo
        tutorialAtual++;

        if (tutorialAtual > 3)
        {
            // Sai do tutorial e começa o jogo
            DesativarTodosTutoriais();

            // Retoma o tempo do jogo
            Time.timeScale = 1f;

            Debug.Log("Jogo iniciado!");
        }
        else
        {
            AtualizarTelasTutorial();
            Debug.Log($"Avançando para o tutorial {tutorialAtual}");
        }
    }

    void AtualizarTelasTutorial()
    {
        // Garante que apenas o painel do tutorial atual esteja ativo
        if (tutorial1Painel != null) tutorial1Painel.SetActive(tutorialAtual == 1);
        if (tutorial2Painel != null) tutorial2Painel.SetActive(tutorialAtual == 2);
        if (tutorial3Painel != null) tutorial3Painel.SetActive(tutorialAtual == 3);
    }

    void DesativarTodosTutoriais()
    {
        if (tutorial1Painel != null) tutorial1Painel.SetActive(false);
        if (tutorial2Painel != null) tutorial2Painel.SetActive(false);
        if (tutorial3Painel != null) tutorial3Painel.SetActive(false);
    }

    public void ReabrirTutorial()
    {
        // Reabre o último tutorial (tela 3) durante o jogo
        DesativarTodosTutoriais();

        if (tutorial3Painel != null)
            tutorial3Painel.SetActive(true);

        // Pausa o jogo novamente
        Time.timeScale = 0f;

        tutorialAtual = 3;

        Debug.Log("Último tutorial reaberto!");
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
