using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_ManagerDeLista : MonoBehaviour
{
    #region Variables
    [Header("Conexión")]
    [SerializeField] private GestorDeListas gestorDeListas;

    [Header("Elementos UI (Lista de Compras)")]
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private TextMeshProUGUI textoDineroQueTengo;
    [SerializeField] private TextMeshProUGUI textoTotalDeLaLista;
    [SerializeField] private TextMeshProUGUI textoItemsEnLista;
    [SerializeField] private Button botonCerrar;

    [Header("Prefabs de Lista")]
    [SerializeField] private GameObject prefabItemUI;
    [SerializeField] private Transform contenedorDeItems;

    [Header("Elementos UI (Boleta)")]
    [SerializeField] private GameObject panelBoleta;
    [SerializeField] private TextMeshProUGUI textoBoletaSubtotal;
    [SerializeField] private TextMeshProUGUI textoBoletaDescuentos;
    [SerializeField] private TextMeshProUGUI textoBoletaTotalFinal;
    [SerializeField] private TextMeshProUGUI textoBoletaDineroRestante;
    [SerializeField] private Transform contenedorItemsBoleta;
    [SerializeField] private GameObject prefabItemBoleta;
    #endregion

    #region Eventos de Unity
    void Start()
    {
        if (gestorDeListas == null)
        {
            gestorDeListas = Object.FindAnyObjectByType<GestorDeListas>();
        }

        GestorDeCarrito.OnItemAgregado += MarcarItemComoEncontrado;
        GestorDeCarrito.OnItemQuitado += DesmarcarItemComoEncontrado;

        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(CerrarPanelLista);
        }

        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(false);
        }

        if (panelBoleta != null)
        {
            panelBoleta.SetActive(false);
        }

        ActualizarPanelUI();
    }

    void OnDestroy()
    {
        GestorDeCarrito.OnItemAgregado -= MarcarItemComoEncontrado;
        GestorDeCarrito.OnItemQuitado -= DesmarcarItemComoEncontrado;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (panelBoleta != null && panelBoleta.activeSelf) return;

            if (panelPrincipal != null && panelPrincipal.activeSelf)
            {
                CerrarPanelLista();
            }
            else
            {
                AbrirPanelLista();
            }
        }
    }
    #endregion

    #region Lógica de Panel UI
    public void AbrirPanelLista()
    {
        if (panelPrincipal != null)
        {
            ActualizarPanelUI();
            panelPrincipal.SetActive(true);
            DesbloquearCursor();
            Time.timeScale = 0f;
        }
    }

    public void CerrarPanelLista()
    {
        if (panelPrincipal != null)
        {
            panelPrincipal.SetActive(false);
            BloquearCursor();
            Time.timeScale = 1f;
        }
    }

    public void ActualizarPanelUI()
    {
        if (textoDineroQueTengo != null && gestorDeListas != null)
        {
            textoDineroQueTengo.text = $"Tu Dinero: ${gestorDeListas.miDinero:F2}";
        }
        else if (textoDineroQueTengo != null)
        {
            textoDineroQueTengo.text = "Tu Dinero: $0.00 (Error)";
        }

        if (textoTotalDeLaLista != null && gestorDeListas != null)
        {
            textoTotalDeLaLista.text = $"Total Lista: ${gestorDeListas.totalLista:F2}";
        }
        else if (textoTotalDeLaLista != null)
        {
            textoTotalDeLaLista.text = "Total Lista: $0.00 (Error)";
        }

        foreach (Transform child in contenedorDeItems)
        {
            Destroy(child.gameObject);
        }

        if (gestorDeListas == null || gestorDeListas.listaDeCompraActual == null || gestorDeListas.listaDeCompraActual.Count == 0)
        {
            if (textoItemsEnLista != null)
            {
                textoItemsEnLista.text = "Items: 0/0";
            }
            return;
        }

        for (int i = 0; i < gestorDeListas.listaDeCompraActual.Count; i++)
        {
            ItemEnLista item = gestorDeListas.listaDeCompraActual[i];
            GameObject objItemUI = Instantiate(prefabItemUI, contenedorDeItems);

            UI_ItemEnLista uiItem = objItemUI.GetComponent<UI_ItemEnLista>();
            if (uiItem != null)
            {
                uiItem.ActualizarDatos(item, i + 1);
            }
        }

        if (textoItemsEnLista != null)
        {
            textoItemsEnLista.text = $"Items: {gestorDeListas.ItemsEncontrados}/{gestorDeListas.listaDeCompraActual.Count}";
        }
    }
    #endregion

    #region Lógica de Boleta
    public void MostrarBoleta(List<ItemInstance> itemsComprados, float subtotal, float descuentos, float totalFinal, float dineroRestante)
    {
        CerrarPanelLista();

        if (textoBoletaSubtotal != null) textoBoletaSubtotal.text = $"Subtotal: ${subtotal:F2}";
        if (textoBoletaDescuentos != null) textoBoletaDescuentos.text = $"Descuentos: -${descuentos:F2}";
        if (textoBoletaTotalFinal != null) textoBoletaTotalFinal.text = $"Total Final: ${totalFinal:F2}";
        if (textoBoletaDineroRestante != null) textoBoletaDineroRestante.text = $"Dinero Restante: ${dineroRestante:F2}";

        foreach (Transform child in contenedorItemsBoleta)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemInstance item in itemsComprados)
        {
            GameObject objBoletaItem = Instantiate(prefabItemBoleta, contenedorItemsBoleta);

            TextMeshProUGUI itemText = objBoletaItem.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = $"{item.datosDelItem.itemName} (${item.precioDeEsteItem:F2})";
            }
        }

        AbrirPanelBoleta();
    }

    private void AbrirPanelBoleta()
    {
        if (panelBoleta != null)
        {
            panelBoleta.SetActive(true);
            DesbloquearCursor();
            Time.timeScale = 0f;
        }
    }

    public void CerrarPanelBoleta()
    {
        if (panelBoleta != null)
        {
            panelBoleta.SetActive(false);
            BloquearCursor();
            Time.timeScale = 1f;
        }
    }
    #endregion

    #region Lógica de Tachar Items
    private void MarcarItemComoEncontrado(ItemData itemData)
    {
        if (gestorDeListas == null) return;

        ItemEnLista item = gestorDeListas.listaDeCompraActual.Find(i => i.datosDelItem == itemData);
        if (item != null && !item.encontrado)
        {
            item.encontrado = true;
            gestorDeListas.ItemsEncontrados++;
            ActualizarPanelUI();
        }
    }

    private void DesmarcarItemComoEncontrado(ItemData itemData)
    {
        if (gestorDeListas == null) return;

        ItemEnLista item = gestorDeListas.listaDeCompraActual.Find(i => i.datosDelItem == itemData);
        if (item != null && item.encontrado)
        {
            item.encontrado = false;
            gestorDeListas.ItemsEncontrados--;
            ActualizarPanelUI();
        }
    }
    #endregion

    #region Lógica del Cursor
    private void BloquearCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DesbloquearCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
}