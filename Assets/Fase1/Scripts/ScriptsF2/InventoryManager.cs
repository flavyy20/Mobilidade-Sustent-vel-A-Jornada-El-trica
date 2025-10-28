using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Player e Carro (controle)")]
    public PlayerController playerController;
    public CarController carController;

    [Header("Slots do inventário (arraste no Inspector, opcional)")]
    public DropSlot[] slots;

    [Header("Painéis e Câmeras")]
    public GameObject inventoryPanel;
    public GameObject panelRecargas;
    public GameObject openMinimapButton; // botão para abrir o minimapa (começa escondido)
    public Camera mainCamera;
    public Camera carCamera;

    [Header("Progressão")]
    public Slider progressBar;

    [Header("Configurações")]
    public float transitionDelay = 5f;
    public float startCheckDelay = 2f; // tempo antes de começar a checar (realtime)

    private bool transitionTriggered = false;
    private bool canCheckSlots = false;

    void Awake()
    {
        // garante que o botão do minimapa comece desligado (independente do estado no Inspector)
        if (openMinimapButton != null)
            openMinimapButton.SetActive(false);
    }

    void Start()
    {
        if (mainCamera != null) mainCamera.enabled = true;
        if (carCamera != null) carCamera.enabled = false;

        // se o array não foi preenchido no Inspector, tenta descobrir slots automaticamente
        if (slots == null || slots.Length == 0)
        {
            TryAutoFindSlots();
        }

        // garante estado limpo
        transitionTriggered = false;

        // limpa todos os slots no início (se houver)
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

        // zera barra de progresso
        if (progressBar != null)
            progressBar.value = 0f;

        // garante que o botão do minimapa esteja oculto até a fase 2 (redundante, mas seguro)
        if (openMinimapButton != null)
            openMinimapButton.SetActive(false);

        // usa WaitForSecondsRealtime para não travar enquanto o jogo estiver pausado no tutorial
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

        if (inventoryPanel != null && inventoryPanel.activeInHierarchy)
        {
            if (AllSlotsFilled())
            {
                transitionTriggered = true;
                StartCoroutine(TransitionToNextPhase());
            }
        }
    }

    // método chamado pelo DropSlot após um OnDrop (via Invoke) para forçar rechecagem imediata
    public void CheckSlotsNow()
    {
        if (!canCheckSlots || transitionTriggered) return;

        if (inventoryPanel != null && inventoryPanel.activeInHierarchy && AllSlotsFilled())
        {
            transitionTriggered = true;
            StartCoroutine(TransitionToNextPhase());
        }
    }

    bool AllSlotsFilled()
    {
        // se não há slots definidos, retorna false (não transiciona automaticamente)
        if (slots == null || slots.Length == 0)
        {
            Debug.Log("[InventoryManager] AllSlotsFilled(): nenhum slot configurado ainda.");
            return false;
        }

        int filled = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            DropSlot slot = slots[i];
            bool has = (slot != null) && slot.HasItem();
            // somente debug se necessário (comente se poluir o console)
            Debug.Log($"{(slot!=null?slot.name:"SlotNull")} -> {(has ? "OCUPADO" : "VAZIO")}");
            if (has) filled++;
        }

        Debug.Log($"[DEBUG] Slots ocupados: {filled}/{slots.Length}");
        return (filled > 0) && (filled == slots.Length); // só true se pelo menos 1 slot existe e todos ocupados
    }

    IEnumerator TransitionToNextPhase()
    {
        Debug.Log("Todos os slots preenchidos! Transição em " + transitionDelay + " segundos...");
        yield return new WaitForSeconds(transitionDelay);

        // Desativa o painel de inventário e painel de recargas
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (panelRecargas != null)
            panelRecargas.SetActive(false);

        // Zera a barra de progresso
        if (progressBar != null)
            progressBar.value = 0f;

        // Troca as câmeras
        if (mainCamera != null) mainCamera.enabled = false;
        if (carCamera != null) carCamera.enabled = true;

        // Alterna controle entre player e carro
        if (playerController != null)
        {
            playerController.enabled = false;
            Debug.Log("PlayerController desativado.");
        }

        if (carController != null)
        {
            carController.podeControlar = true;
            carController.enabled = true;
            Debug.Log("Controle do carro liberado — fase 2 iniciada!");
        }

        // Ativa o botão do minimapa quando a fase 2 começar
        if (openMinimapButton != null)
        {
            openMinimapButton.SetActive(true);
            Debug.Log("Botão do minimapa ativado — disponível para o jogador.");
        }
    }

    // tenta preencher slots automaticamente a partir do inventoryPanel ou de objetos DropSlot na cena
    private void TryAutoFindSlots()
    {
        // prioridade: procurar DropSlot abaixo do inventoryPanel
        if (inventoryPanel != null)
        {
            DropSlot[] found = inventoryPanel.GetComponentsInChildren<DropSlot>(true);
            if (found != null && found.Length > 0)
            {
                slots = found;
                Debug.Log($"[InventoryManager] Auto-detected {slots.Length} DropSlot(s) under inventoryPanel.");
                return;
            }
        }

        // fallback: localizar todos os DropSlot na cena
        DropSlot[] all = FindObjectsOfType<DropSlot>();
        if (all != null && all.Length > 0)
        {
            slots = all;
            Debug.Log($"[InventoryManager] Auto-detected {slots.Length} DropSlot(s) in scene (fallback).");
            return;
        }

        // se aqui, não encontrou slots — slots ficará vazio e checagem não ocorrerá
        slots = new DropSlot[0];
        Debug.LogWarning("[InventoryManager] Não foram encontrados DropSlot(s) automaticamente.");
    }
}
