using UnityEngine;
using TMPro;

public class UI_ItemEnLista : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoNumero;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoPrecio;
    [SerializeField] private TextMeshProUGUI textoTachado; 

    public void ActualizarDatos(ItemEnLista item, int numero)
    {
        if (textoNumero != null)
            textoNumero.text = numero.ToString();

        if (textoNombre != null)
            textoNombre.text = item.datosDelItem.itemName;

        if (textoPrecio != null)
            textoPrecio.text = $"${item.precioGenerado}";

        
        if (textoTachado != null)
        {
            
            textoTachado.text = item.encontrado ? "---PALABRA DE PRUEBA---" : "";
        }
        
    }
}