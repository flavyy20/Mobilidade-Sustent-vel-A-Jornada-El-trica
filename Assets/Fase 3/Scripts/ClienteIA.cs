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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        etapa = 0;
        IrPara(pontoEntrada);
    }

    void Update()
    {
        AtualizarFluxoIA();
        AtualizarAnimacao();
    }

    void AtualizarAnimacao()
    {
        bool andando = agent.velocity.magnitude > 0.1f;
        anim.SetBool("andando", andando);
    }

    void AtualizarFluxoIA()
    {
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
                }
                break;
        }
    }

    bool Chegou()
    {
        if (agent.pathPending) return false;
        return agent.remainingDistance <= agent.stoppingDistance + 0.05f;
    }

    void IrPara(Transform destino)
    {
        if (destino == null) return;
        agent.isStopped = false;
        agent.SetDestination(destino.position);
    }
}

