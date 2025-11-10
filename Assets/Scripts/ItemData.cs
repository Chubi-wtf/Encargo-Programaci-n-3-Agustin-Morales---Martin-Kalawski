using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Item", menuName = "Supermercado/Item Data")]
public class ItemData : ScriptableObject
{
    #region Variables
    [Header("Información del Producto")]
    public string itemName = "Nombre del Item";
    public int price = 1;
    public string itemCategory = "Categoría";
    public Sprite itemIcon;

    [TextArea(3, 5)]
    public string itemDescription = "Descripción...";
    #endregion
}