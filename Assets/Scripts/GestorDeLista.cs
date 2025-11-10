using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GestorDeListas : MonoBehaviour
{
    #region Variables Públicas
    public List<ItemEnLista> listaDeCompraActual = new List<ItemEnLista>();
    public int ItemsEncontrados { get; set; } = 0;
    public float miDinero = 27.28f;
    public float totalLista { get; set; } = 0f;

    [Header("Configuración de Generación")]
    [SerializeField] private GestorDeItems gestorDeItems;
    [SerializeField] private int minItems = 5;
    [SerializeField] private int maxItems = 10;
    #endregion

    #region Ciclo de Vida
    void Awake()
    {
        if (gestorDeItems == null)
        {
            gestorDeItems = Object.FindAnyObjectByType<GestorDeItems>();
        }
    }

    void Start()
    {
        GenerarNuevaLista();
    }
    #endregion

    #region Lógica de Lista
    public void GenerarNuevaLista()
    {
        if (gestorDeItems == null || gestorDeItems.TodosLosItemsDisponibles.Count == 0)
        {
            listaDeCompraActual.Clear();
            totalLista = 0f;
            return;
        }

        listaDeCompraActual.Clear();
        ItemsEncontrados = 0;
        float totalGastoRequerido = 0f;

        HashSet<ItemData> itemsUnicosSeleccionados = new HashSet<ItemData>();
        int maxItemsDisponibles = gestorDeItems.TodosLosItemsDisponibles.Count;
        int cantidadTotal = Mathf.Min(maxItems, maxItemsDisponibles);
        int cantidadItemsAGenerar = Random.Range(minItems, cantidadTotal + 1);

        while (itemsUnicosSeleccionados.Count < cantidadItemsAGenerar)
        {
            int indiceAleatorio = Random.Range(0, maxItemsDisponibles);
            ItemData datosAleatorios = gestorDeItems.TodosLosItemsDisponibles[indiceAleatorio];

            if (itemsUnicosSeleccionados.Add(datosAleatorios))
            {
                float precioGenerado = Random.Range(1.00f, 10.00f);
                ItemEnLista nuevoItem = new ItemEnLista(datosAleatorios, precioGenerado);
                listaDeCompraActual.Add(nuevoItem);

                totalGastoRequerido += precioGenerado;
            }
        }

        totalLista = Mathf.Round(totalGastoRequerido * 100f) / 100f;

        float margenSeguridad = Random.Range(5.00f, 10.00f);

        miDinero = totalLista + margenSeguridad;
        miDinero = Mathf.Round(miDinero * 100f) / 100f;

        UI_ManagerDeLista uiManager = Object.FindAnyObjectByType<UI_ManagerDeLista>();
        if (uiManager != null)
        {
            uiManager.ActualizarPanelUI();
        }
    }
    #endregion
}