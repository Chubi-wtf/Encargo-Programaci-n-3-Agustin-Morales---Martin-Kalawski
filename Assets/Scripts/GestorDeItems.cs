using UnityEngine;
using System.Collections.Generic;

public class GestorDeItems : MonoBehaviour
{
    #region Variables del Inspector
    [Header("Base de Datos de Items")]
    [Tooltip("Arrastra AQUÍ TODOS tus ScriptableObjects de 'ItemData' (Café, Manzana, Ketchup...)")]
    public List<ItemData> todosLosItemsDisponibles;
    #endregion

    #region Lógica de Precios
    public float GenerarPrecioParaItem(ItemData item)
    {
        float precioBase = GetPrecioBasePorCategoria(item.itemCategory);
        float factorRandom = Random.Range(0.85f, 1.15f);
        float precioConRandom = precioBase * factorRandom;
        float precioFinal = Mathf.Round(precioConRandom * 100f) / 100f;
        return precioFinal;
    }

   
    private float GetPrecioBasePorCategoria(string categoria)
    {
        string cat = categoria.ToLower().Trim();

        switch (cat)
        {
            
            case "fruta":
            case "verdura": 
                return 1.20f;
            
            case "bebida":
            case "botella": 
            case "café":    
                return 2.50f;

            case "enlatado":
                return 1.80f;

            case "condimento":
                return 2.10f;
                
            case "panaderia": 
                return 0.80f;

            case "limpieza":
            case "confort": 
                return 3.50f;


            case "dulces":             
            case "barra de chocolate": 
                return 1.50f; 

            case "carne":    
            case "embutido": 
                return 4.00f; 

            default:
                Debug.LogWarning($"Categoría '{categoria}' no reconocida. Usando precio base por defecto.");
                return 1.00f;
        }
    }
    #endregion
}