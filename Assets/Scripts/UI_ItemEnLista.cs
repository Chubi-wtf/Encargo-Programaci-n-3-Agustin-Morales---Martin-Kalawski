using UnityEngine;
using TMPro;

public class UI_ItemEnLista : MonoBehaviour
{
    #region Variables
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoPrecio;

    [Header("Configuración de Tachado")]
    [SerializeField] private Color colorNormal = Color.white;
    [SerializeField] private Color colorTachado = Color.red;
    #endregion

    #region Lógica de UI
    public void ActualizarDatos(ItemEnLista item, int numero)
    {
        string nombreStr = item.datosDelItem.itemName;
        string precioStr = $"${item.precioGenerado:F2}";

        if (item.encontrado)
        {
            if (textoNombre != null) textoNombre.color = colorTachado;
            if (textoPrecio != null) textoPrecio.color = colorTachado;

            if (textoNombre != null) textoNombre.text = $"<s>{nombreStr}</s>";
            if (textoPrecio != null) textoPrecio.text = $"<s>{precioStr}</s>";
        }
        else
        {
            if (textoNombre != null) textoNombre.color = colorNormal;
            if (textoPrecio != null) textoPrecio.color = colorNormal;

            if (textoNombre != null) textoNombre.text = nombreStr;
            if (textoPrecio != null) textoPrecio.text = precioStr;
        }
    }
    #endregion
}