// UIManager.cs
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Painel")]
    public GameObject painelInfo;
    public Text infoText;

    [Header("Botões")]
    public Button cargaLenta;
    public Button cargaRapida;
    public Button cargaUltraRapida;

    [Header("Cores (ativa / desativada)")]
    public Color corLentaAtiva = new Color(1f, 0.6f, 0f); // laranja padrão
    public Color corRapidaAtiva = Color.green;
    public Color corUltraAtiva = Color.blue;
    public Color corDesativada = Color.white;

    private VehicleInteraction activeVehicle;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (painelInfo != null) painelInfo.SetActive(false);
        SetButtonsState(false);
    }

    void SetButtonsState(bool ativo)
    {
        cargaLenta.interactable = ativo;
        cargaRapida.interactable = ativo;
        cargaUltraRapida.interactable = ativo;

        cargaLenta.image.color = ativo ? corLentaAtiva : corDesativada;
        cargaRapida.image.color = ativo ? corRapidaAtiva : corDesativada;
        cargaUltraRapida.image.color = ativo ? corUltraAtiva : corDesativada;
    }

    // Chamado por um veículo quando o player entra no trigger
    public void ShowVehicleInfo(VehicleInteraction vehicle)
    {
        activeVehicle = vehicle;
        if (painelInfo != null) painelInfo.SetActive(true);
        if (infoText != null) infoText.text = vehicle.GetDescricaoTipo();
        SetButtonsState(true);
    }

    // Chamado por um veículo quando o player sai do trigger
    public void HideVehicleInfo(VehicleInteraction vehicle)
    {
        // só fecha se o veículo que solicita for o mesmo que abriu
        if (activeVehicle != vehicle) return;

        activeVehicle = null;
        if (painelInfo != null) painelInfo.SetActive(false);
        SetButtonsState(false);
    }
}
