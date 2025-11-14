using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    public enum TipoVeiculo { A, B, C }
    public TipoVeiculo tipo = TipoVeiculo.A;

    public static bool UIEnabled = true; // 🔥 controla se a UI deve aparecer na fase 1

    [Header("Cores por tipo")]
    public Color corA = new Color(1f, 0.5f, 0f);
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

    public string GetDescricaoTipo()
    {
        switch (tipo)
        {
            case TipoVeiculo.A:
                return "Veículo Tipo A (BEV) — 100% elétrico. Depende exclusivamente da bateria e precisa de carga completa.";

            case TipoVeiculo.B:
                return "Veículo Tipo B (HEV) — Híbrido não plug-in. Recarrega em movimento, não precisa de tomada.";

            case TipoVeiculo.C:
                return "Veículo Tipo C (PHEV) — Híbrido plug-in. Pode usar bateria + combustível e ser recarregado externamente.";

            default:
                return "Veículo desconhecido.";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!UIEnabled)
        {
            // se o painel ainda estiver aberto, forçamos esconder
            if (UIManager.Instance != null)
                UIManager.Instance.HideVehicleInfo(this);

            return;
        }

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
