using UnityEngine;
using UnityEngine.UI;

public class PainelTutorial : MonoBehaviour
{
    [Header("UI")]
    public GameObject painel;
    public Text texto;
    public Button btnContinuar;
    public Button btnVoltar;
    public Button btnSair;

    [Header("Páginas")]
    [TextArea]
    public string[] paginas;
    private int index = 0;

    void Start()
    {
        FecharPainel(); // garante que começa fechado
        AbrirPainel(); // abre sozinho ao iniciar a cena
    }

    public void AbrirPainel()
    {
        painel.SetActive(true);
        AtualizarPagina();
        TravarPlayer(true);
    }

    public void FecharPainel()
    {
        painel.SetActive(false);
        TravarPlayer(false);
    }

    public void ProximaPagina()
    {
        if (index < paginas.Length - 1)
        {
            index++;
            AtualizarPagina();
        }
    }

    public void PaginaAnterior()
    {
        if (index > 0)
        {
            index--;
            AtualizarPagina();
        }
    }

    private void AtualizarPagina()
    {
        texto.text = paginas[index];

        // mostrar / esconder botões
        btnVoltar.gameObject.SetActive(index > 0);
        btnContinuar.gameObject.SetActive(index < paginas.Length - 1);
        btnSair.gameObject.SetActive(index == paginas.Length - 1);
    }

    private void TravarPlayer(bool travar)
    {
        VendedorController player = FindObjectOfType<VendedorController>();
        if (player != null)
        {
            player.podeMover = !travar;
        }
    }
}

