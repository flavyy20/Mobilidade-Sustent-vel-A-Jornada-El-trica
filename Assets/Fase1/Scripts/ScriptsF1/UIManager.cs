using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Painel de Informação")]
    public GameObject painelInfo;
    public Text infoText;

    [Header("Botões de Recarga")]
    public Button cargaLenta;
    public Button cargaRapida;
    public Button cargaUltraRapida;

    [Header("Cores (ativa / desativada)")]
    public Color corLentaAtiva = new Color(1f, 0.6f, 0f); // laranja
    public Color corRapidaAtiva = Color.green;
    public Color corUltraAtiva = Color.blue;
    public Color corDesativada = Color.white;

    [HideInInspector]
    public VehicleInteraction activeVehicle; // público para o InventoryManager limpar

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (painelInfo != null)
            painelInfo.SetActive(false);

        SetButtonsState(false);
    }


    // ==========================================================
    // LIGA OU DESLIGA OS BOTÕES DE RECARGA
    // ==========================================================
    void SetButtonsState(bool ativo)
    {
        cargaLenta.interactable = ativo;
        cargaRapida.interactable = ativo;
        cargaUltraRapida.interactable = ativo;

        cargaLenta.image.color = ativo ? corLentaAtiva : corDesativada;
        cargaRapida.image.color = ativo ? corRapidaAtiva : corDesativada;
        cargaUltraRapida.image.color = ativo ? corUltraAtiva : corDesativada;
    }


    // ==========================================================
    // EXIBE INFORMAÇÃO DO VEÍCULO QUANDO PLAYER ENCOSTA
    // ==========================================================
    public void ShowVehicleInfo(VehicleInteraction vehicle)
    {
        activeVehicle = vehicle;

        if (painelInfo != null)
            painelInfo.SetActive(true);

        if (infoText != null)
            infoText.text = vehicle.GetDescricaoTipo();

        SetButtonsState(true);
    }


    // ==========================================================
    // ESCONDE QUANDO PLAYER SAI DO CARRO
    // ==========================================================
    public void HideVehicleInfo(VehicleInteraction vehicle)
    {
        // só fecha se for o mesmo carro que estava ativo
        if (activeVehicle != vehicle)
            return;

        activeVehicle = null;

        if (painelInfo != null)
            painelInfo.SetActive(false);

        SetButtonsState(false);
    }


    // ==========================================================
    // LIMPAR IMEDIATAMENTE TODA UI DA FASE 1
    // (chamado quando termina fase 1)
    // ==========================================================
    public void HideAllImmediate()
    {
        if (painelInfo != null)
            painelInfo.SetActive(false);

        if (infoText != null)
            infoText.text = "";

        activeVehicle = null;

        SetButtonsState(false);
    }
}
