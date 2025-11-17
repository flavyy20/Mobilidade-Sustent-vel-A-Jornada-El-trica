using UnityEngine;
using UnityEngine.UI;

public class TimeBarController : MonoBehaviour
{
    [Header("Configurações do Timer")]
    public float startTime = 60f;
    private float currentTime;

    [Header("UI")]
    public Slider timeSlider;
    public GameObject gameOverPanel;

    [Header("Referência geral para reset")]
    public InventoryManager inventoryManager;

    private bool timerActive = false;
    private bool venceu = false;   // evita Game Over se já venceu

    void Start()
    {
        // inicializa slider e estado
        if (timeSlider != null)
        {
            timeSlider.maxValue = startTime;
            timeSlider.value = startTime;
        }

        currentTime = startTime;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // não atualiza se pausado ou se já venceu
        if (!timerActive || venceu) return;

        currentTime -= Time.deltaTime;

        if (timeSlider != null)
            timeSlider.value = Mathf.Max(0f, currentTime);

        if (currentTime <= 0f)
        {
            TimeIsUp();
        }
    }

    public void StartTimer()
    {
        currentTime = startTime;

        if (timeSlider != null)
            timeSlider.value = startTime;

        timerActive = true;
        venceu = false;   // garante que o timer volte ao estado padrão
    }

    private void TimeIsUp()
    {
        if (venceu) return;  // se já venceu, não mostra game over

        timerActive = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("Tempo acabou! GAME OVER!");
    }

    // >>>> CHAMADO QUANDO O CHECKPOINT É ATINGIDO (ou quando vencer)
    public void StopTimerAfterWin()
    {
        venceu = true;     // impede game over
        timerActive = false;
        Debug.Log("Timer parado porque venceu!");
    }

    // Mantém compatibilidade com chamadas antigas (MenuManager etc.)
    // Reseta sem iniciar: volta ao estado inicial do menu (timer parado, slider no inicio, painel escondido)
    public void ResetTimer()
    {
        // para o timer
        timerActive = false;
        venceu = false;

        // reseta valor do slider
        if (timeSlider != null)
        {
            timeSlider.maxValue = startTime;
            timeSlider.value = startTime;
        }

        currentTime = startTime;

        // garante painel de game over escondido
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Debug.Log("TimeBarController: ResetTimer() chamado — timer zerado e parado.");
    }

    // (opcional) reinicia e já ativa o timer do zero
    public void StartTimerFromZero()
    {
        currentTime = startTime;
        if (timeSlider != null) timeSlider.value = startTime;
        timerActive = true;
        venceu = false;
    }
}
