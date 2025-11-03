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
    }

    public void CerrarPanel()
    {
        panelPrincipal.SetActive(false);
        BloquearCursor();
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