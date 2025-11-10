using UnityEngine;
using System.Collections.Generic;

public class GestorDeItems : MonoBehaviour
{
    #region Variables
    [Header("Base de Datos")]
    [Tooltip("Lista de todos los Scriptable Objects ItemData disponibles en el juego.")]
    public List<ItemData> TodosLosItemsDisponibles = new List<ItemData>();
    #endregion

    #region Lógica de Precios
    public float GenerarPrecioParaItem(ItemData datosDelItem)
    {
        return Random.Range(1.0f, 10.0f);
    }
    #endregion
}