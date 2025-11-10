using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GestorDeCheckout : MonoBehaviour
{
    #region Variables y Referencias
    [Header("Referencias")]
    [SerializeField] private GestorDeCarrito gestorDeCarrito;
    [SerializeField] private GestorDeListas gestorDeListas;
    [SerializeField] private UI_ManagerDeLista uiManagerLista;
    [SerializeField] private NPC_Jefe npcJefe;

    [Header("Interacción")]
    public float interactionRange = 3f;
    public Transform player;
    public KeyCode interactionKey = KeyCode.E;

    private bool hasCheckedOut = false;
    #endregion

    #region Ciclo de Vida
    void Start()
    {
        if (gestorDeCarrito == null) gestorDeCarrito = Object.FindAnyObjectByType<GestorDeCarrito>();
        if (gestorDeListas == null) gestorDeListas = Object.FindAnyObjectByType<GestorDeListas>();
        if (uiManagerLista == null) uiManagerLista = Object.FindAnyObjectByType<UI_ManagerDeLista>();
        if (npcJefe == null) npcJefe = Object.FindAnyObjectByType<NPC_Jefe>();

        if (player == null)
        {
            PlayerControl pc = Object.FindAnyObjectByType<PlayerControl>();
            if (pc != null) player = pc.transform;
        }
    }

    void Update()
    {
        if (gestorDeCarrito == null || player == null || hasCheckedOut) return;

        if (Vector3.Distance(player.position, transform.position) <= interactionRange)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                if (gestorDeCarrito.itemsDentroDelCarrito.Count > 0)
                {
                    ProcesarCheckout();
                }
            }
        }
    }
    #endregion

    #region Lógica de Checkout
    private void ProcesarCheckout()
    {
        if (hasCheckedOut) return;

        List<ItemInstance> itemsAComprar = gestorDeCarrito.itemsDentroDelCarrito.ToList();

        var (subtotal, totalDescuento) = GestorDePromociones.AplicarDescuentos(itemsAComprar);

        float totalFinal = subtotal - totalDescuento;
        totalFinal = Mathf.Round(totalFinal * 100f) / 100f;

        if (totalFinal > gestorDeListas.miDinero)
        {
            return;
        }

        float dineroRestante = gestorDeListas.miDinero - totalFinal;
        dineroRestante = Mathf.Round(dineroRestante * 100f) / 100f;

        hasCheckedOut = true;

        if (npcJefe != null)
        {
            string mensajeCastigo = $"Gracias por comprar aquí. Su tiempo de vida como alma se ha reducido un {totalFinal:F2} segundos.";
            npcJefe.MostrarDialogoCastigo(mensajeCastigo);
        }

        if (uiManagerLista != null)
        {
            uiManagerLista.MostrarBoleta(itemsAComprar, subtotal, totalDescuento, totalFinal, dineroRestante);
        }

        if (npcJefe != null)
        {
            npcJefe.FinalizarCompra(dineroRestante, totalFinal);
        }
    }
    #endregion
}