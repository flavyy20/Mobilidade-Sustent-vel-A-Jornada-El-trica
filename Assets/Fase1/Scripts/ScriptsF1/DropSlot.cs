using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    private DraggableItem currentItem;

    public void OnDrop(PointerEventData eventData)
    {
        if (currentItem != null) return;

        DraggableItem item = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if (item != null)
        {
            // mantem transform local correto usando worldPositionStays = false
            item.transform.SetParent(transform, false);
            item.transform.localPosition = Vector3.zero;

            currentItem = item;
            Debug.Log($"Item {item.name} colocado em {name}");

            // força uma verificação no InventoryManager (pequeno delay para garantir que Unity finalize hierarquia)
            InventoryManager inv = FindObjectOfType<InventoryManager>();
            if (inv != null)
            {
                // chama CheckSlotsNow sem bloquear; Invoke é OK aqui
                inv.Invoke(nameof(InventoryManager.CheckSlotsNow), 0.05f);
            }
        }
    }

    public bool HasItem()
    {
        return currentItem != null && currentItem.gameObject.activeInHierarchy;
    }

    public void ClearSlot()
    {
        currentItem = null;
    }

    private void OnTransformChildrenChanged()
    {
        // atualiza currentItem automaticamente (procura DraggableItem entre filhos)
        currentItem = GetComponentInChildren<DraggableItem>();
        // não fazemos logs aqui para evitar spam
    }
}
