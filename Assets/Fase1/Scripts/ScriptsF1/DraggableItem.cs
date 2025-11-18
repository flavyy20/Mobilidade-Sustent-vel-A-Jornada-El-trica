using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private bool initialized = false;

    // usado durante o drag (não altera a origem salva)
    private Transform originalParent;
    private Vector2 originalPosition;

    // SALVA a origem permanente do item (para reset do minigame)
    [HideInInspector] public Transform initialParent;
    [HideInInspector] public Vector2 initialAnchoredPosition;

    public Transform OriginalParent => originalParent;
    public Vector2 OriginalPosition => originalPosition;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Salva a origem PARA SEMPRE (parent + anchored position) - isso permite resetar depois
        initialParent = transform.parent;
        initialAnchoredPosition = rectTransform.anchoredPosition;
    }
    private void Start()
    {
        if (!initialized)
        {
            originalParent = transform.parent;
            originalPosition = GetComponent<RectTransform>().anchoredPosition;
            initialized = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Salva posição e parent temporários (para voltar se o player soltar fora do slot)
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        transform.SetParent(transform.root); // traz pra frente (acima de tudo)
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Segue o mouse / toque
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Se ainda estiver sem um novo pai (não solto em slot)
        if (transform.parent == transform.root)
        {
            // Volta pro ponto de origem temporário (onde estava antes do drag)
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    // Chamado pelo MenuManager para forçar o item de volta para o local inicial
    public void ResetToOrigin()
    {
        // Se a origem foi destruída por acaso, apenas ativamos o objeto e deixamos como estava
        if (initialParent != null)
        {
            transform.SetParent(initialParent, false);
            rectTransform.anchoredPosition = initialAnchoredPosition;
        }

        // garante que o item esteja visível / interagível após reset
        canvasGroup.blocksRaycasts = true;
        gameObject.SetActive(true);
    }
}
