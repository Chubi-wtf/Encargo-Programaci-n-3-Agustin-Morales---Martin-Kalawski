using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ManagerDeLista : MonoBehaviour
{
    #region Variables
    [Header("Conexión")]
    [SerializeField] private GestorDeListas gestorDeListas;

    [Header("Elementos UI")]
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private TextMeshProUGUI textoDineroQueTengo;
    [SerializeField] private TextMeshProUGUI textoTotalDeLaLista;
    [SerializeField] private TextMeshProUGUI textoItemsEnLista;
    [SerializeField] private Button botonCerrar;

    [Header("Prefabs de Lista")]
    [SerializeField] private GameObject prefabItemUI;
    [SerializeField] private Transform contenedorDeItems;
    #endregion

    #region Eventos de Unity


    void Start()
    {
        if (gestorDeListas == null)
        {
            gestorDeListas = FindObjectOfType<GestorDeListas>();
        }

        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(CerrarPanel);
        }

        gestorDeListas.GenerarNuevaLista();
        ActualizarPanelUI();
        AbrirPanel();



        GestorDeCarrito.OnItemAgregado += MarcarItemComoEncontrado;
        GestorDeCarrito.OnItemQuitado += DesmarcarItemComoEncontrado;
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
            bool estaActivo = panelPrincipal.activeSelf;
            if (estaActivo) { CerrarPanel(); }
            else { AbrirPanel(); }
        }
    }
    #endregion

    #region Lógica del Panel
    void ActualizarPanelUI()
    {
        if (gestorDeListas == null) return;

        textoDineroQueTengo.text = $"Tu Dinero: ${gestorDeListas.miDinero}";
        textoTotalDeLaLista.text = $"Total Lista: ${gestorDeListas.totalLista}";
        textoItemsEnLista.text = $"Items: {gestorDeListas.cantidadItems}";

        foreach (Transform child in contenedorDeItems)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < gestorDeListas.listaDeCompraActual.Count; i++)
        {
            ItemEnLista item = gestorDeListas.listaDeCompraActual[i];
            GameObject objItemUI = Instantiate(prefabItemUI, contenedorDeItems);
            objItemUI.GetComponent<UI_ItemEnLista>().ActualizarDatos(item, i + 1);
        }
    }

    public void AbrirPanel()
    {
        panelPrincipal.SetActive(true);
        DesbloquearCursor();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CerrarPanel()
    {
        panelPrincipal.SetActive(false);
        BloquearCursor();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Lógica de Tachar Items (NUEVO)

    private void MarcarItemComoEncontrado(ItemData itemData)
    {
        foreach (ItemEnLista itemEnLista in gestorDeListas.listaDeCompraActual)
        {
            if (!itemEnLista.encontrado && itemEnLista.datosDelItem == itemData)
            {
                itemEnLista.encontrado = true;
                break; 
            }
        }

        ActualizarPanelUI();
    }

    private void DesmarcarItemComoEncontrado(ItemData itemData)
    {
        for (int i = gestorDeListas.listaDeCompraActual.Count - 1; i >= 0; i--)
        {
            ItemEnLista itemEnLista = gestorDeListas.listaDeCompraActual[i];
            if (itemEnLista.encontrado && itemEnLista.datosDelItem == itemData)
            {
                itemEnLista.encontrado = false;
                break; 
            }
        }

        ActualizarPanelUI();
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