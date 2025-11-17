using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events; // << adicionado

public class MenuManager : MonoBehaviour
{
    [Header("Cutscenes")]
    public VideoClip cutsceneFase1;

    [Header("Painéis")]
    public GameObject menuPainel;
    public GameObject creditosPainel;
    public GameObject tutorial1Painel;
    public GameObject tutorial2Painel;
    public GameObject tutorial3Painel;

    private int tutorialAtual = 0;

    void Start()
    {
        Time.timeScale = 0f;
    }

    public void Jogar()
    {
        // >>> ATIVA O PLAYER DA FASE 1 NOVAMENTE <<<
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.gameObject.SetActive(true);

        // >>> REATIVA A PROGRESSBAR DA FASE 1 <<<
        InventoryManager invTemp = FindObjectOfType<InventoryManager>();
        if (invTemp != null && invTemp.progressBarObject != null)
            invTemp.progressBarObject.SetActive(true);

        if (menuPainel != null)
            menuPainel.SetActive(false);

        // RODA CUTSCENE DA FASE 1 ANTES DO TUTORIAL
        // cria evento que chama OnCutsceneFase1Ended quando terminar
        if (CutscenePlayer.Instance != null)
        {
            UnityEvent ev = new UnityEvent();
            ev.AddListener(OnCutsceneFase1Ended);
            CutscenePlayer.Instance.PlayCutscene(cutsceneFase1, ev);
        }
        else
        {
            // fallback: se CutscenePlayer não existir, inicia tutorial imediatamente
            tutorialAtual = 1;
            AtualizarTelasTutorial();
        }

        Debug.Log("Tutorial iniciado!");
    }

    // chamado QUANDO a cutscene da fase 1 terminar
    public void OnCutsceneFase1Ended()
    {
        tutorialAtual = 1;
        AtualizarTelasTutorial();
        Debug.Log("Cutscene fase1 terminou -> mostrando tutorial 1");
    }

    public void Continuar()
    {
        tutorialAtual++;

        if (tutorialAtual > 3)
        {
            DesativarTodosTutoriais();
            Time.timeScale = 1f;

            Debug.Log("Jogo iniciado!");
        }
        else
        {
            AtualizarTelasTutorial();
            Debug.Log("Avançando para o tutorial " + tutorialAtual);
        }
    }

    void AtualizarTelasTutorial()
    {
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
        DesativarTodosTutoriais();

        if (tutorial3Painel != null)
            tutorial3Painel.SetActive(true);

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

    // ---------------------------------------------------------
    // >>>>>> FUNÇÃO ADICIONADA, SEM ALTERAR SUA ESTRUTURA <<<<<<
    // ---------------------------------------------------------

    public void ResetarJogoCompleto()
    {
        Time.timeScale = 1f;

        // Resetar posição do carro se existir
        CarController car = FindObjectOfType<CarController>();
        if (car != null)
        {
            Transform spawn = GameObject.Find("CarStartPoint")?.transform;
            if (spawn != null)
            {
                car.transform.position = spawn.position;
                car.transform.rotation = spawn.rotation;
            }

            car.enabled = false;
            car.podeControlar = false;
        }

        // Resetar timer (se existir)
        TimeBarController timer = FindObjectOfType<TimeBarController>();
        if (timer != null)
            timer.ResetTimer();

        // Resetar UI / câmeras da fase 2
        InventoryManager inv = FindObjectOfType<InventoryManager>();
        if (inv != null)
        {
            if (inv.openMinimapButton != null)
                inv.openMinimapButton.SetActive(false);

            if (inv.timeBarUI != null)
                inv.timeBarUI.SetActive(false);

            if (inv.carCamera != null)
                inv.carCamera.enabled = false;

            if (inv.mainCamera != null)
                inv.mainCamera.enabled = true;

            //  Reativar o PLAYER ROBÔ da fase 1
            if (inv.playerController != null)
            {
                inv.playerController.gameObject.SetActive(true);

                Collider[] cols = inv.playerController.GetComponentsInChildren<Collider>();
                foreach (var c in cols)
                    c.enabled = true;
            }

            //  Reativar o PanelRecargas da fase 1
            if (inv.panelRecargas != null)
                inv.panelRecargas.SetActive(true);
        }

        // Resetar interface da fase 1 (botões + panelInfo)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideAllImmediate();
        }

        Debug.Log("Jogo completamente resetado para o MENU.");
    }
}
