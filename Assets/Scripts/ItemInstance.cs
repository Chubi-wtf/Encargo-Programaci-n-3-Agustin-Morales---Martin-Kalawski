using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemInstance : MonoBehaviour
{
    #region Variables
    public ItemData datosDelItem;
    private Rigidbody miRigidbody;
    private const string tagCestaCarrito = "CartBasket";
    #endregion

    #region Eventos de Unity
    void Awake()
    {
        miRigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider otroCollider)
    {
        if (otroCollider.CompareTag(tagCestaCarrito))
        {
            miRigidbody.isKinematic = true;

            Rigidbody rbDelCarrito = otroCollider.GetComponentInParent<Rigidbody>();
            if (rbDelCarrito != null)
            {
                transform.SetParent(rbDelCarrito.transform);
            }
        }
    }

    void OnTriggerExit(Collider otroCollider)
    {
        if (otroCollider.CompareTag(tagCestaCarrito))
        {
            miRigidbody.isKinematic = false;
            transform.SetParent(null);
        }
    }
    #endregion
}