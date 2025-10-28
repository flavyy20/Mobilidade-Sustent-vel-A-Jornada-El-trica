using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBarController : MonoBehaviour
{
    public static ProgressBarController Instance;

    [Header("Referências")]
    public Slider progressSlider;
    public GameObject transitionPanel; // painel de transição (coloque no Inspector)

    [Header("Configurações")]
    public float delayBeforePanel = 5f; // tempo antes do painel aparecer

    private void Awake()
    {
        Instance = this;
        progressSlider.value = 0;

        if (transitionPanel != null)
            transitionPanel.SetActive(false); // começa invisível
    }

    public void IncrementProgress()
    {
        if (progressSlider.value < progressSlider.maxValue)
        {
            progressSlider.value += 1;
        }

        if (progressSlider.value >= progressSlider.maxValue)
        {
            Debug.Log("Todos os carros foram atendidos!");
            StartCoroutine(ShowPanelAfterDelay());
        }
    }

    IEnumerator ShowPanelAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforePanel);

        if (transitionPanel != null)
        {
            transitionPanel.SetActive(true);
            Debug.Log("Painel de transição ativado!");
        }
    }

    public void ResetProgress()
    {
        progressSlider.value = 0;
        if (transitionPanel != null)
            transitionPanel.SetActive(false);
    }
}
