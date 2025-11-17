using UnityEngine;

public class MinimapUIManager : MonoBehaviour
{
    [Header("Painel do minimapa e botões")]
    public GameObject minimapPanel;
    public GameObject openButton;
    public GameObject closeButton;

    void Start()
    {
        // Painel começa fechado
        if (minimapPanel != null)
            minimapPanel.SetActive(false);

        // Botões começam desativados (InventoryManager controla isso)
        if (openButton != null)
            openButton.SetActive(false);

        if (closeButton != null)
            closeButton.SetActive(false);
    }

    public void OpenMinimap()
    {
        if (minimapPanel != null) minimapPanel.SetActive(true);
        if (openButton != null) openButton.SetActive(false);
        if (closeButton != null) closeButton.SetActive(true);
    }

    public void CloseMinimap()
    {
        if (minimapPanel != null) minimapPanel.SetActive(false);
        if (openButton != null) openButton.SetActive(true);
        if (closeButton != null) closeButton.SetActive(false);
    }
}
