using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemInstance : MonoBehaviour
{
    #region Variables
    public ItemData datosDelItem;
    public float precioDeEsteItem { get; private set; }

    private Rigidbody miRigidbody;
    private Collider[] todosMisColliders;
    private GestorDeItems gestorDeItems;
    private const string tagCestaCarrito = "CartBasket";
    #endregion

    #region Eventos de Unity
    void Start()
    {
        miRigidbody = GetComponent<Rigidbody>();
        todosMisColliders = GetComponentsInChildren<Collider>();

        // --- LÍNEA CORREGIDA ---
        gestorDeItems = FindFirstObjectByType<GestorDeItems>();

        if (gestorDeItems != null)
        {
            precioDeEsteItem = gestorDeItems.GenerarPrecioParaItem(datosDelItem);
        }
        else
        {
            Debug.LogError("ItemInstance no pudo encontrar el GestorDeItems en la escena!");
            precioDeEsteItem = 1.0f;
        }
    }

    void OnTriggerEnter(Collider otroCollider)
    {
        if (otroCollider.CompareTag(tagCestaCarrito))
        {
            miRigidbody.isKinematic = true;
            SetCollidersEnabled(false);

            Rigidbody rbDelCarrito = otroCollider.GetComponentInParent<Rigidbody>();
            if (rbDelCarrito != null)
            {
                transform.SetParent(rbDelCarrito.transform);
            }

            // Esta línea dará error hasta que arregles el Paso 1
            GestorDeCarrito carrito = rbDelCarrito.GetComponent<GestorDeCarrito>();
            if (carrito != null)
            {
                carrito.AnadirItemAlTotal(this);
            }
        }
    }

    void OnTriggerExit(Collider otroCollider)
    {
        if (otroCollider.CompareTag(tagCestaCarrito))
        {
            miRigidbody.isKinematic = false;
            SetCollidersEnabled(true);
            transform.SetParent(null);

            // Esta línea dará error hasta que arregles el Paso 1
            GestorDeCarrito carrito = otroCollider.GetComponentInParent<GestorDeCarrito>();
            if (carrito != null)
            {
                carrito.QuitarItemDelTotal(this);
            }
        }
    }
    #endregion

    #region Funciones Helper
    private void SetCollidersEnabled(bool estaActivo)
    {
        foreach (Collider col in todosMisColliders)
        {
            if (!col.isTrigger)
            {
                col.enabled = estaActivo;
            }
        }
    }
    #endregion
}