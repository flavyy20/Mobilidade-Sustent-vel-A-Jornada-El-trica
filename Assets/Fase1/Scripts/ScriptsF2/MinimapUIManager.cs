using UnityEngine;

public class MinimapUIManager : MonoBehaviour
{
    [Header("Painel do minimapa e bot�es")]
    public GameObject minimapPanel; // painel com o mapa em si
    public GameObject openButton;   // bot�o para abrir o minimapa
    public GameObject closeButton;  // bot�o para fechar (dentro do painel)

    void Start()
    {
        // Inicia com o bot�o de abrir vis�vel e o painel fechado
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
