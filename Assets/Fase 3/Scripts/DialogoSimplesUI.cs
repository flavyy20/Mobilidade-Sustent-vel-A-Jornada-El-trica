using UnityEngine;
using UnityEngine.UI;

public class DialogoSimplesUI : MonoBehaviour
{
    public static DialogoSimplesUI Instance;

    public GameObject painelCliente;
    public Text txtCliente;
    public Text txtAtender;

    public Button btnAtender;
    public Button btnContinuar;

    public GameObject painelTiposCarros; // BEV | HEV | PHEV

    private void Awake()
{
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
}

    private void Start()
    {
        painelCliente.SetActive(false);
        btnContinuar.interactable = false;

        btnAtender.onClick.AddListener(BotaoAtender);
        btnContinuar.onClick.AddListener(AbrirPainelTipos);
    }

    public void AbrirPainelCliente(string texto)
    {
        painelCliente.SetActive(true);
        txtCliente.text = texto;

        btnContinuar.interactable = false; // só ativa depois do atender
    }

    public void DefinirTextoAtender(string texto)
    {
        txtAtender.text = texto;
    }

    private void BotaoAtender()
    {
        // Limpa o texto anterior
        txtCliente.text = "";

        // Mostra a segunda frase
        txtAtender.text = "Estou à procura de um carro mais sustentável para o meio ambiente, com condução suave e silenciosa e boa economia a longo prazo.";

        // Habilita botão continuar
        btnContinuar.interactable = true;
    }



    private void AbrirPainelTipos()
    {
        painelCliente.SetActive(false);
        painelTiposCarros.SetActive(true);
    }
}
