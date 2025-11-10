using UnityEngine;

[System.Serializable]
public class ItemEnLista
{
    #region Variables
    public ItemData datosDelItem;
    public float precioGenerado;
    public bool encontrado = false;
    #endregion

    #region Constructor
    public ItemEnLista(ItemData datos, float precio)
    {
        this.datosDelItem = datos;
        this.precioGenerado = precio;
        this.encontrado = false;
    }
    #endregion
}