using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("UI da Fase 1")]
    public GameObject vehicleInfoPanel;
    public UIManager uiManager;

    [Header("Player e Carro (controle)")]
    public PlayerController playerController;
    public CarController carController;

    [Header("Slots do inventário")]
    public DropSlot[] slots;

    [Header("Painéis e Câmeras")]
    public GameObject inventoryPanel;
    public GameObject panelRecargas;
    public GameObject openMinimapButton;
    public Camera mainCamera;
    public Camera carCamera;

    [Header("Progressão")]
    public Slider progressBar;
    public GameObject progressBarObject;

    [Header("Timer da Fase 2")]
    public TimeBarController timeBarController;
    public GameObject timeBarUI;

    [Header("Configurações")]
    public float transitionDelay = 5f;
    public float startCheckDelay = 2f;

    private bool transitionTriggered = false;
    private bool canCheckSlots = false;


    void Awake()
    {
        // Minimapa sempre começa DESATIVADO
        if (openMinimapButton != null)
            openMinimapButton.SetActive(false);

        // Barra da fase 2 começa desativada
        if (timeBarUI != null)
            timeBarUI.SetActive(false);
    }

    void Start()
    {
        // Configura estado inicial das câmeras
        if (mainCamera != null) mainCamera.enabled = true;
        if (carCamera != null) carCamera.enabled = false;

        if (slots == null || slots.Length == 0)
            TryAutoFindSlots();

        transitionTriggered = false;

        // limpar slots
        if (slots != null)
        {
            foreach (DropSlot slot in slots)
            {
                if (slot != null)
                {
                    slot.ClearSlot();
                    foreach (Transform child in slot.transform)
                        Destroy(child.gameObject);
                }
            }
        }

        if (progressBar != null)
            progressBar.value = 0;

        StartCoroutine(EnableSlotCheckAfterDelayRealtime());
    }

    IEnumerator EnableSlotCheckAfterDelayRealtime()
    {
        yield return new WaitForSecondsRealtime(startCheckDelay);
        canCheckSlots = true;
    }

    void Update()
    {
        if (!canCheckSlots || transitionTriggered)
            return;

        if (inventoryPanel != null && inventoryPanel.activeInHierarchy)
        {
            if (AllSlotsFilled())
            {
                transitionTriggered = true;
                StartCoroutine(TransitionToNextPhase());
            }
        }

        if (progressBar != null && progressBar.value >= progressBar.maxValue)
        {
            CleanPhase1UI_And_DisablePlayer();
        }
    }

    private void CleanPhase1UI_And_DisablePlayer()
    {
        if (vehicleInfoPanel != null)
            vehicleInfoPanel.SetActive(false);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.painelInfo.SetActive(false);
            UIManager.Instance.infoText.text = "";
            UIManager.Instance.activeVehicle = null;
        }

        foreach (var v in FindObjectsOfType<VehicleInteraction>())
            v.enabled = false;

        if (playerController != null && playerController.gameObject.activeSelf)
        {
            Collider[] cols = playerController.GetComponentsInChildren<Collider>();
            foreach (var c in cols)
                c.enabled = false;

            playerController.gameObject.SetActive(false);
        }
    }

    public void CheckSlotsNow()
    {
        if (!canCheckSlots || transitionTriggered) return;

        if (inventoryPanel.activeInHierarchy && AllSlotsFilled())
        {
            transitionTriggered = true;
            StartCoroutine(TransitionToNextPhase());
        }
    }

    bool AllSlotsFilled()
    {
        int filled = 0;

        foreach (var slot in slots)
        {
            if (slot != null && slot.HasItem())
                filled++;
        }

        return (filled > 0 && filled == slots.Length);
    }

    IEnumerator TransitionToNextPhase()
    {
        Debug.Log("Slots completos! Preparando transição...");

        CleanPhase1UI_And_DisablePlayer();

        yield return new WaitForSeconds(transitionDelay);

        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (panelRecargas != null) panelRecargas.SetActive(false);

        if (progressBar != null) progressBar.value = 0;

        // Oculta progress bar da fase 1
        if (progressBarObject != null)
            progressBarObject.SetActive(false);

        // Troca câmeras
        mainCamera.enabled = false;
        carCamera.enabled = true;

        // Carro ativo
        carController.enabled = true;
        carController.podeControlar = true;

        // Minimapa só AQUI, na fase 2
        if (openMinimapButton != null)
            openMinimapButton.SetActive(true);

        // Ativa barra da fase 2
        if (timeBarUI != null)
            timeBarUI.SetActive(true);

        if (timeBarController != null)
            timeBarController.StartTimer();

        Debug.Log("FASE 2 iniciada!");
    }


    private void TryAutoFindSlots()
    {
        if (inventoryPanel != null)
        {
            DropSlot[] found = inventoryPanel.GetComponentsInChildren<DropSlot>(true);
            if (found.Length > 0)
            {
                slots = found;
                return;
            }
        }
    }
}
