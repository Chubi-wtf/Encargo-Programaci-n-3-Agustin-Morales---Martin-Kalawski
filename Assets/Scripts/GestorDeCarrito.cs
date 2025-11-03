using UnityEngine;
using System.Collections.Generic;

public class GestorDeCarrito : MonoBehaviour
{
    [Header("Estado del Carrito")]
    public float totalGastado = 0f;
    public List<ItemInstance> itemsDentroDelCarrito;

    void Awake()
    {
        itemsDentroDelCarrito = new List<ItemInstance>();
    }

    public void AnadirItemAlTotal(ItemInstance item)
    {
        if (!itemsDentroDelCarrito.Contains(item))
        {
            itemsDentroDelCarrito.Add(item);
            RecalcularTotal();
            Debug.Log($"Añadido: {item.datosDelItem.itemName} (${item.precioDeEsteItem}). Nuevo Total: ${totalGastado}");
        }
    }

    public void QuitarItemDelTotal(ItemInstance item)
    {
        if (itemsDentroDelCarrito.Contains(item))
        {
            itemsDentroDelCarrito.Remove(item);
            RecalcularTotal();
            Debug.Log($"Quitado: {item.datosDelItem.itemName} (${item.precioDeEsteItem}). Nuevo Total: ${totalGastado}");
        }
    }

    private void RecalcularTotal()
    {
        totalGastado = 0f;
        foreach (ItemInstance item in itemsDentroDelCarrito)
        {
            totalGastado += item.precioDeEsteItem;
        }

        totalGastado = Mathf.Round(totalGastado * 100f) / 100f;
    }
}