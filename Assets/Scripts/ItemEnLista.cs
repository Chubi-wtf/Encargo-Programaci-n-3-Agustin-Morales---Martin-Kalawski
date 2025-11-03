using UnityEngine;

[System.Serializable]
public class ItemEnLista
{
    public ItemData datosDelItem;
    public float precioGenerado;

    public ItemEnLista(ItemData datos, float precio)
    {
        this.datosDelItem = datos;
        this.precioGenerado = precio;
    }
}