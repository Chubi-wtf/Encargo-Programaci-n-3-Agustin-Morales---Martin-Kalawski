using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EventoDeDescuento : MonoBehaviour
{
    #region Variables
    [Header("Referencias")]
    [SerializeField] private GestorDeItems gestorDeItems;
    [SerializeField] private DialogManager dialogManager;

    [Header("Configuración de Evento")]
    [SerializeField] private float tiempoParaEvento = 60f;
    [SerializeField] private string nombreNPCAnuncio = "Altavoz del Supermercado";

    private float tiempoRestante;
    private bool eventoActivado = false;

    public ItemData ItemEnPromocion { get; private set; }
    #endregion

    #region Eventos de Unity
    void Start()
    {
        tiempoRestante = tiempoParaEvento;

        if (gestorDeItems == null) gestorDeItems = Object.FindAnyObjectByType<GestorDeItems>();
        if (dialogManager == null) dialogManager = Object.FindAnyObjectByType<DialogManager>();

        if (gestorDeItems == null || dialogManager == null)
        {
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (eventoActivado) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            ActivarPromocion();
        }
    }
    #endregion

    #region Lógica de Promoción
    private void ActivarPromocion()
    {
        if (eventoActivado) return;

        if (gestorDeItems.TodosLosItemsDisponibles.Count == 0)
        {
            return;
        }

        eventoActivado = true;

        int indexAleatorio = Random.Range(0, gestorDeItems.TodosLosItemsDisponibles.Count);
        ItemEnPromocion = gestorDeItems.TodosLosItemsDisponibles[indexAleatorio];

        string anuncio = $"¡ATENCIÓN COMPRADORES! ¡El {ItemEnPromocion.itemName} ahora tiene un 50% de descuento! ¡Apresúrese antes de que se acabe la oferta!";

        dialogManager.ShowDialogue(
            nombreNPCAnuncio,
            anuncio,
            null,
            null
        );
    }

    public bool VerificarPromocion(ItemData item)
    {
        return eventoActivado && item == ItemEnPromocion;
    }
    #endregion
}