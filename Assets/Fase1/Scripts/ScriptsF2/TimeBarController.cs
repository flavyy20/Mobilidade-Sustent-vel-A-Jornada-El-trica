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

    void Start()
    {
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
        if (!timerActive) return;

        currentTime -= Time.deltaTime;

        if (timeSlider != null)
            timeSlider.value = currentTime;

        if (currentTime <= 0)
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
    }

    private void TimeIsUp()
    {
        timerActive = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("Tempo acabou! GAME OVER!");
    }
    public void StartTimerFromZero()
    {
        currentTime = startTime;
        timeSlider.value = startTime;
        timerActive = true;
    }

}
