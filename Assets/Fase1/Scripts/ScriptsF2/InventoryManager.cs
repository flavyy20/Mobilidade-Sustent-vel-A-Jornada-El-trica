using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("UI da Fase 1")]
    public GameObject vehicleInfoPanel;   // painel que mostra infos do carro
    public UIManager uiManager;           // referência direta ao UIManager

    [Header("Player e Carro (controle)")]
    public PlayerController playerController;
    public CarController carController;

    [Header("Slots do inventário (arraste no Inspector, opcional)")]
    public DropSlot[] slots;

    [Header("Painéis e Câmeras")]
    public GameObject inventoryPanel;
    public GameObject panelRecargas;
    public GameObject openMinimapButton;
    public Camera mainCamera;
    public Camera carCamera;

    [Header("Progressão")]
    public Slider progressBar;

    [Header("Configurações")]
    public float transitionDelay = 5f;
    public float startCheckDelay = 2f;

    private bool transitionTriggered = false;
    private bool canCheckSlots = false;


    void Awake()
    {
        if (openMinimapButton != null)
            openMinimapButton.SetActive(false);
    }

    void Start()
    {
        if (mainCamera != null) mainCamera.enabled = true;
        if (carCamera != null) carCamera.enabled = false;

        if (slots == null || slots.Length == 0)
            TryAutoFindSlots();

        transitionTriggered = false;

        if (slots != null && slots.Length > 0)
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
            progressBar.value = 0f;

        if (openMinimapButton != null)
            openMinimapButton.SetActive(false);

        StartCoroutine(EnableSlotCheckAfterDelayRealtime());
    }


    IEnumerator EnableSlotCheckAfterDelayRealtime()
    {
        yield return new WaitForSecondsRealtime(startCheckDelay);
        canCheckSlots = true;
    }


    void Update()
    {
        if (!canCheckSlots || transitionTriggered) return;

        // Apenas checa slots se inventário ativo
        if (inventoryPanel != null && inventoryPanel.activeInHierarchy)
        {
            if (AllSlotsFilled())
            {
                transitionTriggered = true;
                StartCoroutine(TransitionToNextPhase());
            }
        }

        // DESATIVA PLAYER E MENSAGENS AO COMPLETAR A BARRA DE PROGRESSO
        if (progressBar != null && progressBar.value >= progressBar.maxValue)
        {
            CleanPhase1UI_And_DisablePlayer(); // <- nova função limpa e desativa
        }
    }


    // ===========================
    // LIMPEZA TOTAL DA FASE 1
    // ===========================
    private void CleanPhase1UI_And_DisablePlayer()
    {
        // 1 — esconde painel de infos
        if (vehicleInfoPanel != null)
            vehicleInfoPanel.SetActive(false);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.painelInfo.SetActive(false);
            UIManager.Instance.infoText.text = "";
            UIManager.Instance.activeVehicle = null;
        }

        // 2 — Desliga TODAS interações de veículos
        foreach (var v in FindObjectsOfType<VehicleInteraction>())
            v.enabled = false;

        // 3 — Desativa COMPLETAMENTE o Player
        if (playerController != null && playerController.gameObject.activeSelf)
        {
            Collider[] cols = playerController.GetComponentsInChildren<Collider>();
            foreach (var c in cols)
                c.enabled = false;

            playerController.gameObject.SetActive(false);

            Debug.Log("Player e UI da fase 1 desativados completamente.");
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
        if (slots == null || slots.Length == 0)
        {
            Debug.Log("[InventoryManager] AllSlotsFilled(): nenhum slot encontrado.");
            return false;
        }

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

        // limpa UI e player ANTES de esperar
        CleanPhase1UI_And_DisablePlayer();

        yield return new WaitForSeconds(transitionDelay);

        // Painéis da fase 1 somem
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (panelRecargas != null)
            panelRecargas.SetActive(false);

        // limpa progresso
        if (progressBar != null)
            progressBar.value = 0;

        // troca câmeras
        if (mainCamera != null) mainCamera.enabled = false;
        if (carCamera != null) carCamera.enabled = true;

        // ativa controle do carro
        if (carController != null)
        {
            carController.enabled = true;
            carController.podeControlar = true;
        }

        // botão minimapa somente agora
        if (openMinimapButton != null)
            openMinimapButton.SetActive(true);

        Debug.Log("FASE 2 iniciada com sucesso.");
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

        DropSlot[] all = FindObjectsOfType<DropSlot>();
        if (all.Length > 0)
        {
            slots = all;
            return;
        }

        slots = new DropSlot[0];
        Debug.LogWarning("[InventoryManager] Nenhum DropSlot encontrado.");
    }
}
