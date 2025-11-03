using UnityEngine;
using TMPro;

public class UI_ItemEnLista : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoNumero; 
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoPrecio;

    public void ActualizarDatos(ItemEnLista item, int numero)
    {
        if (textoNumero != null)
            textoNumero.text = $"-{numero}"; 

        if (textoNombre != null)
            textoNombre.text = item.datosDelItem.itemName;

        if (textoPrecio != null)
            textoPrecio.text = $"${item.precioGenerado}";
    }
}