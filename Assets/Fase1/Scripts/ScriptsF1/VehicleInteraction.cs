using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    public enum TipoVeiculo { A, B, C }
    public TipoVeiculo tipo = TipoVeiculo.A; // escolha no Inspector

    [Header("Cores por tipo (opcional override)")]
    public Color corA = new Color(1f, 0.5f, 0f); // laranja
    public Color corB = Color.green;
    public Color corC = Color.blue;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        AplicarCorPorTipo();
    }

    void AplicarCorPorTipo()
    {
        if (rend == null) return;
        switch (tipo)
        {
            case TipoVeiculo.A: rend.material.color = corA; break;
            case TipoVeiculo.B: rend.material.color = corB; break;
            case TipoVeiculo.C: rend.material.color = corC; break;
        }
    }

    // Exposto para o UIManager
    public string GetDescricaoTipo()
    {
        switch (tipo)
        {
            case TipoVeiculo.A:
                return "Veículo Tipo A (BEV) — 100% elétrico. Depende exclusivamente da bateria e precisa de uma carga completa.";
            case TipoVeiculo.B:
                return "Veículo Tipo B (HEV) — Híbrido não plug-in. Utiliza combustível e recarrega sozinho durante o movimento, sem precisar de tomada.";
            case TipoVeiculo.C:
                return "Veículo Tipo C (PHEV) — Híbrido plug-in. Combina bateria e combustível, mas também pode ser recarregado externamente.";
            default:
                return "Veículo desconhecido.";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (UIManager.Instance != null)
            UIManager.Instance.ShowVehicleInfo(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (UIManager.Instance != null)
            UIManager.Instance.HideVehicleInfo(this);
    }
}
