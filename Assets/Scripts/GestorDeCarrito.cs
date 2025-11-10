using UnityEngine;
using System.Collections.Generic;

public class GestorDeCarrito : MonoBehaviour
{
    #region Variables y Eventos
    public static event System.Action<ItemData> OnItemAgregado;
    public static event System.Action<ItemData> OnItemQuitado;

    [Header("Estado del Carrito")]
    public float totalGastado = 0f;
    public List<ItemInstance> itemsDentroDelCarrito;
    #endregion

    #region Ciclo de Vida
    void Awake()
    {
        itemsDentroDelCarrito = new List<ItemInstance>();
    }
    #endregion

    #region Lógica del Carrito
    public void AnadirItemAlTotal(ItemInstance item)
    {
        if (!itemsDentroDelCarrito.Contains(item))
        {
            itemsDentroDelCarrito.Add(item);
            RecalcularTotal();

            OnItemAgregado?.Invoke(item.datosDelItem);
        }
    }

    public void QuitarItemDelTotal(ItemInstance item)
    {
        if (itemsDentroDelCarrito.Contains(item))
        {
            itemsDentroDelCarrito.Remove(item);
            RecalcularTotal();

            OnItemQuitado?.Invoke(item.datosDelItem);
        }
    }

    void RecalcularTotal()
    {
        totalGastado = 0f;
        foreach (ItemInstance item in itemsDentroDelCarrito)
        {
            totalGastado += item.precioDeEsteItem;
        }
        totalGastado = Mathf.Round(totalGastado * 100f) / 100f;
    }
    #endregion
}