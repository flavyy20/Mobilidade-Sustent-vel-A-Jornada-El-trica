using UnityEngine;
using UnityEngine.UI;

public class CarSelectionUI : MonoBehaviour
{
    public static CarSelectionUI Instance;   // ← AQUI CRIA O INSTANCE

    [Header("Painel de Seleção de Carros")]
    public GameObject painelSelecao;
    public Button btnFechar;

    [Header("Botões dos Tipos de Carro")]
    public Button btnCarroBEV;
    public Button btnCarroHEV;
    public Button btnCarroPHEV;

    [Header("Painel de Informações do Carro")]
    public GameObject painelInfoCarro;
    public Text txtInfoCarro;
    public Button btnIndicar;

    private string caracteristicaCliente = "";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        painelSelecao.SetActive(false);
        painelInfoCarro.SetActive(false);

        // Eventos
        btnFechar.onClick.AddListener(() => painelSelecao.SetActive(false));

        btnCarroBEV.onClick.AddListener(() => MostrarInfo("BEV"));
        btnCarroHEV.onClick.AddListener(() => MostrarInfo("HEV"));
        btnCarroPHEV.onClick.AddListener(() => MostrarInfo("PHEV"));

        btnIndicar.onClick.AddListener(IndicarCarro);
    }

    public void DefinirCaracteristicaCliente(string texto)
    {
        caracteristicaCliente = texto;
    }

    public void AbrirSelecao()
    {
        painelSelecao.SetActive(true);
        painelInfoCarro.SetActive(false);
    }

    void MostrarInfo(string tipo)
    {
        painelInfoCarro.SetActive(true);

        if (tipo == "BEV")
        {
            txtInfoCarro.text =
                "BEV - Carro 100% elétrico\n" +
                "- Zero emissões\n" +
                "- Condução silenciosa\n" +
                "- Manutenção baixa\n" +
                "- Sustentável para o meio ambiente";
        }
        else if (tipo == "HEV")
        {
            txtInfoCarro.text =
                "HEV - Híbrido\n" +
                "- Combinação gasolina + elétrico\n" +
                "- Boa economia\n" +
                "- Recarrega enquanto dirige";
        }
        else if (tipo == "PHEV")
        {
            txtInfoCarro.text =
                "PHEV - Híbrido Plug-in\n" +
                "- Pode rodar só no elétrico\n" +
                "- Carrega na tomada\n" +
                "- Ótima economia em curtas distâncias";
        }
    }

    void IndicarCarro()
    {
        Debug.Log("Carro indicado para o cliente com base em: " + caracteristicaCliente);
    }
}

