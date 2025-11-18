using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [Header("Referências UI")]
    public Text textoTutorial;
    public GameObject painelTutorial;

    [Header("Botões")]
    public Button btnContinuar;
    public Button btnVoltar;
    public Button btnSair;

    [Header("Páginas do Tutorial")]
    [TextArea(3, 5)]
    public string[] paginas;

    private int paginaAtual = 0;

    void Start()
    {
        painelTutorial.SetActive(false);

        btnContinuar.onClick.RemoveAllListeners();
        btnVoltar.onClick.RemoveAllListeners();
        btnSair.onClick.RemoveAllListeners();

        btnContinuar.onClick.AddListener(ProximaPagina);
        btnVoltar.onClick.AddListener(PaginaAnterior);
        btnSair.onClick.AddListener(FecharTutorial);

        // Só ativa automático se for painel inicial
        if (gameObject.name == "PanelInicial")
            Invoke(nameof(AbrirTutorial), 4f);
    }

    public void AbrirTutorial()
    {
        paginaAtual = 0;
        AtualizarTexto();
        painelTutorial.SetActive(true);
    }

    void AtualizarTexto()
    {
        textoTutorial.text = paginas[paginaAtual];
    }

    public void ProximaPagina()
    {
        if (paginaAtual < paginas.Length - 1)
            paginaAtual++;

        AtualizarTexto();
    }

    public void PaginaAnterior()
    {
        if (paginaAtual > 0)
            paginaAtual--;

        AtualizarTexto();
    }

    public void FecharTutorial()
    {
        painelTutorial.SetActive(false);
    }
}

