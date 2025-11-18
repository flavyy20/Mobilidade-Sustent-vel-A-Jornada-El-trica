using UnityEngine;
using UnityEngine.AI;

public class ClienteIA : MonoBehaviour
{
    public Transform pontoEntrada;
    public Transform pontoCarros;
    public Transform pontoBalcao;

    public float tempoOlharCarros = 3f;

    private NavMeshAgent agent;
    private Animator anim;
    private int etapa;
    private float tempoRestante;

    private bool iaAtiva = false;
    public float delayInicio = 20f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        etapa = 0;

        agent.isStopped = true;
        anim.SetBool("andando", false);

        Invoke(nameof(AtivarIA), delayInicio);
    }

    void AtivarIA()
    {
        iaAtiva = true;

        if (agent.isOnNavMesh)
            IrPara(pontoEntrada);
        else
            Debug.LogWarning($"{name} NÃO está sobre o NavMesh!");
    }

    void Update()
    {
        if (!iaAtiva) return;

        AtualizarFluxoIA();
        AtualizarAnimacao();
    }

    void AtualizarAnimacao()
    {
        if (!agent.isOnNavMesh)
        {
            anim.SetBool("andando", false);
            return;
        }

        bool andando = agent.remainingDistance > agent.stoppingDistance + 0.1f;

        if (agent.pathPending)
            andando = true;

        anim.SetBool("andando", andando);
    }

    void AtualizarFluxoIA()
    {
        if (!agent.isOnNavMesh)
            return;

        switch (etapa)
        {
            case 0:
                if (Chegou())
                {
                    etapa = 1;
                    IrPara(pontoCarros);
                }
                break;

            case 1:
                if (Chegou())
                {
                    agent.isStopped = true;
                    tempoRestante = tempoOlharCarros;
                    etapa = 2;
                }
                break;

            case 2:
                tempoRestante -= Time.deltaTime;
                if (tempoRestante <= 0f)
                {
                    agent.isStopped = false;
                    etapa = 3;
                    IrPara(pontoBalcao);
                }
                break;

            case 3:
                if (Chegou())
                {
                    agent.isStopped = true;
                    etapa = 4;

                    // Apenas abre o painel com a primeira frase
                    DialogoSimplesUI.Instance.AbrirPainelCliente(
                        "Olá, tudo bem? Gostaria de ver um carro elétrico."
                    );

                    // A segunda frase só será mostrada quando o jogador apertar "Atender"
                    // CarSelectionUI ainda pode definir a característica do cliente
                    CarSelectionUI.Instance.DefinirCaracteristicaCliente(
                        "sustentável meio ambiente suave silenciosa economia"
                    );
                }
                break;
        }
    }

    bool Chegou()
    {
        if (!agent.isOnNavMesh) return false;
        if (agent.pathPending) return false;
        if (!agent.hasPath) return false;

        return agent.remainingDistance <= agent.stoppingDistance + 0.05f;
    }

    void IrPara(Transform destino)
    {
        if (destino == null || !agent.isOnNavMesh) return;

        agent.isStopped = false;
        agent.SetDestination(destino.position);
    }
}
