using UnityEngine;
using System.Collections.Generic;

public class GestorDeListas : MonoBehaviour
{
    #region Variables
    [Header("Conexión")]
    [SerializeField] private GestorDeItems gestorDeItems;

    [Header("Rangos de Generación")]
    [SerializeField] private Vector2Int rangoItems = new Vector2Int(5, 23);

    [Header("Lista Generada (Resultados)")]
    public List<ItemEnLista> listaDeCompraActual;
    public float miDinero { get; private set; }
    public float totalLista { get; private set; }
    public int cantidadItems { get; private set; }
    #endregion

    #region Eventos de Unity
    void Start()
    {
        if (gestorDeItems == null)
        {
            Debug.LogError("¡GestorDeListas no tiene un GestorDeItems asignado!");
            return;
        }

       
    }
    #endregion

    #region Lógica de la Lista
    public void GenerarNuevaLista()
    {
        cantidadItems = Random.Range(rangoItems.x, rangoItems.y + 1);

        
        if (listaDeCompraActual == null)
            listaDeCompraActual = new List<ItemEnLista>();
        else
            listaDeCompraActual.Clear();

        totalLista = 0;

        for (int i = 0; i < cantidadItems; i++)
        {
            int indiceRandom = Random.Range(0, gestorDeItems.todosLosItemsDisponibles.Count);
            ItemData itemElegido = gestorDeItems.todosLosItemsDisponibles[indiceRandom];

            float precioGenerado = gestorDeItems.GenerarPrecioParaItem(itemElegido);

            ItemEnLista nuevoItem = new ItemEnLista(itemElegido, precioGenerado);
            listaDeCompraActual.Add(nuevoItem);

            totalLista += precioGenerado;
        }

        totalLista = Mathf.Round(totalLista * 100f) / 100f;

       
        float extra = (Random.Range(0, 2) == 0) ? 10f : 22f; 
        miDinero = totalLista + extra;
        miDinero = Mathf.Round(miDinero * 100f) / 100f; 

        Debug.Log($"LISTA GENERADA: {cantidadItems} items, con un costo de ${totalLista}. Tienes ${miDinero}.");
    }
    #endregion
}