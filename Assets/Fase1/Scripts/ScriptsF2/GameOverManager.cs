using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Referências")]
    public GameObject gameOverPanel;
    public TimeBarController timeBar;
    public InventoryManager inventoryManager;
    public CarController carController;
    public Transform carStartPoint;  // arraste o empty da posição inicial

    public void RestartPhase2()
    {
        // Fecha painel de Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Despausa caso tenha sido pausado
        Time.timeScale = 1f;

        // Reinicia o timer da fase 2
        if (timeBar != null)
            timeBar.StartTimerFromZero();   // Função abaixo

        // Reposiciona o carro na posição inicial
        if (carController != null && carStartPoint != null)
        {
            carController.transform.position = carStartPoint.position;
            carController.transform.rotation = carStartPoint.rotation;

            carController.enabled = true;
            carController.podeControlar = true;
        }

        // Configura corretamente o estado da Fase 2
        if (inventoryManager != null)
        {
            // GARANTE que a fase 1 fica OFF
            if (inventoryManager.inventoryPanel != null)
                inventoryManager.inventoryPanel.SetActive(false);

            if (inventoryManager.panelRecargas != null)
                inventoryManager.panelRecargas.SetActive(false);

            if (inventoryManager.progressBarObject != null)
                inventoryManager.progressBarObject.SetActive(false);

            // Minimapa ATIVO
            if (inventoryManager.openMinimapButton != null)
                inventoryManager.openMinimapButton.SetActive(true);

            // Cameras corretas
            if (inventoryManager.mainCamera != null)
                inventoryManager.mainCamera.enabled = false;

            if (inventoryManager.carCamera != null)
                inventoryManager.carCamera.enabled = true;
        }

        Debug.Log("Fase 2 reiniciada com sucesso!");
    }
}
