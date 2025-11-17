using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events; // << adicionado

public class InventoryManager : MonoBehaviour
{
    public VideoClip cutsceneFase2;

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
        if (openMinimapButton != null)
            openMinimapButton.SetActive(false);

        if (timeBarUI != null)
            timeBarUI.SetActive(false);
    }

    void Start()
    {
        if (mainCamera != null) mainCamera.enabled = true;
        if (carCamera != null) carCamera.enabled = false;

        if (slots == null || slots.Length == 0)
            TryAutoFindSlots();

        transitionTriggered = false;

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

        if (progressBarObject != null)
            progressBarObject.SetActive(false);

        // Tocar cutscene da fase 2 ANTES de ativar a fase 2
        if (CutscenePlayer.Instance != null)
        {
            UnityEvent ev = new UnityEvent();
            ev.AddListener(OnCutsceneFase2Ended);
            CutscenePlayer.Instance.PlayCutscene(cutsceneFase2, ev);
        }
        else
        {
            // Se não houver CutscenePlayer (fallback), chama direto
            OnCutsceneFase2Ended();
        }

        Debug.Log("Cutscene fase 2 disparada (aguardando finalizar)...");
    }

    // chamado QUANDO a cutscene da fase 2 terminar
    public void OnCutsceneFase2Ended()
    {
        // Depois da cutscene: ativa fase 2 (mesma ordem que tinha antes)
        mainCamera.enabled = false;
        carCamera.enabled = true;

        carController.enabled = true;
        carController.podeControlar = true;

        if (openMinimapButton != null)
            openMinimapButton.SetActive(true);

        if (timeBarUI != null)
            timeBarUI.SetActive(true);

        if (timeBarController != null)
            timeBarController.StartTimer();

        Debug.Log("FASE 2 iniciada (após cutscene)!");
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
