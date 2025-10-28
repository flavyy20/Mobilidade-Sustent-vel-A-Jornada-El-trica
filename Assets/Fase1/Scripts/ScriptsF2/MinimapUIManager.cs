using UnityEngine;

public class MinimapUIManager : MonoBehaviour
{
    [Header("Painel do minimapa e botões")]
    public GameObject minimapPanel; // painel com o mapa em si
    public GameObject openButton;   // botão para abrir o minimapa
    public GameObject closeButton;  // botão para fechar (dentro do painel)

    void Start()
    {
        // Inicia com o botão de abrir visível e o painel fechado
        if (minimapPanel != null) minimapPanel.SetActive(false);
        if (openButton != null) openButton.SetActive(true);
        if (closeButton != null) closeButton.SetActive(false);
    }

    public void OpenMinimap()
    {
        if (minimapPanel != null) minimapPanel.SetActive(true);
        if (openButton != null) openButton.SetActive(false);
        if (closeButton != null) closeButton.SetActive(true);

        Debug.Log("Minimapa aberto!");
    }

    public void CloseMinimap()
    {
        if (minimapPanel != null) minimapPanel.SetActive(false);
        if (openButton != null) openButton.SetActive(true);
        if (closeButton != null) closeButton.SetActive(false);

        Debug.Log("Minimapa fechado!");
    }
}
