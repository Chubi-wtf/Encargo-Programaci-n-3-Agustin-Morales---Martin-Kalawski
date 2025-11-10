using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerControl : MonoBehaviour
{
    #region Variables del Inspector
    [Header("Componentes")]
    [SerializeField] private Transform camaraDelPlayer;

    [Header("Movimiento")]
    [SerializeField] private float velocidadCaminar = 5f;
    [SerializeField] private float velocidadCorrer = 8f;

    [Header("Cámara")]
    [SerializeField] private float sensibilidadMouse = 100f;

    [Header("Interacción (Raycast)")]
    [SerializeField] private float distanciaRaycast = 6f;
    [SerializeField] private LayerMask capaInteractuable;
    [SerializeField] private LayerMask capaPiso;

    [Header("Interacción (Agarrar)")]
    [SerializeField] private Transform holderParaItems;
    [SerializeField] private KeyCode teclaInteractuar = KeyCode.E;

    [Header("Interacción (Carrito)")]
    [SerializeField] private string tagDelCarrito = "Carrito";
    [SerializeField] private float suavidadSeguirCarrito = 15f;
    [SerializeField] private float alturaFlotarCarrito = 0.0001f;

    [Header("UI de Interacción")]
    [SerializeField] private GameObject panelInfoItem;
    [SerializeField] private TextMeshProUGUI textoNombreItem;
    [SerializeField] private TextMeshProUGUI textoPrecioItem;
    #endregion

    #region Variables Privadas
    private Rigidbody miRigidbody;
    private Collider miCollider;
    private float velocidadActual;

    private GameObject objetoAgarrado;
    private Rigidbody rbObjetoAgarrado;
    private Collider colliderObjetoAgarrado;

    private bool isPaused = false;
    [SerializeField] private GameObject pauseMenuUI;

    private Rigidbody rbCarritoAgarrado = null;
    private Collider colliderCarritoAgarrado = null;
    private Vector3 offsetPosCarrito;
    private Quaternion offsetRotCarrito;

    private float rotacionX = 0f;
    private float rotacionY = 0f;
    private Vector3 inputMovimiento;

    private DialogManager dialogManager;
    #endregion

    #region Eventos de Unity
    void Awake()
    {
        miRigidbody = GetComponent<Rigidbody>();
        miCollider = GetComponent<Collider>();
        miRigidbody.freezeRotation = true;

        dialogManager = Object.FindAnyObjectByType<DialogManager>();
    }

    void Start()
    {
        velocidadActual = velocidadCaminar;

        if (panelInfoItem != null)
        {
            panelInfoItem.SetActive(false);
        }
    }

    void Update()
    {
        float mouseInputX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseInputY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

        rotacionY += mouseInputX;
        rotacionX -= mouseInputY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        transform.rotation = Quaternion.Euler(0f, rotacionY, 0f);
        camaraDelPlayer.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        inputMovimiento = (transform.forward * inputV + transform.right * inputH).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocidadActual = velocidadCorrer;
        }
        else
        {
            velocidadActual = velocidadCaminar;
        }

        ManejarRaycastUI();
        ManejarInputInteraccion();

        if (Input.GetKey(KeyCode.P))
        {
            pauseOnOff();
        }
    }

    void FixedUpdate()
    {
        Vector3 velocidadObjetivo = inputMovimiento * velocidadActual;
        velocidadObjetivo.y = miRigidbody.linearVelocity.y;
        miRigidbody.linearVelocity = velocidadObjetivo;

        if (rbCarritoAgarrado != null)
        {
            ManejarFisicasCarrito();
        }
    }
    #endregion

    #region Lógica de Interacción y UI
    void ManejarRaycastUI()
    {
        if (objetoAgarrado != null || rbCarritoAgarrado != null)
        {
            panelInfoItem.SetActive(false);
            return;
        }

        if (dialogManager != null && dialogManager.IsDialogueActive)
        {
            panelInfoItem.SetActive(false);
            return;
        }

        Ray rayo = new Ray(camaraDelPlayer.position, camaraDelPlayer.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(rayo, out hitInfo, distanciaRaycast, capaInteractuable))
        {
            ItemInstance itemInstance = hitInfo.collider.GetComponent<ItemInstance>();
            if (itemInstance != null)
            {
                panelInfoItem.SetActive(true);
                textoNombreItem.text = itemInstance.datosDelItem.itemName;
                textoPrecioItem.text = $"${itemInstance.precioDeEsteItem:F2} - {itemInstance.datosDelItem.itemCategory}";
            }
            else
            {
                panelInfoItem.SetActive(false);
            }
        }
        else
        {
            panelInfoItem.SetActive(false);
        }
    }

    private void ManejarInputInteraccion()
    {
        if (!Input.GetKeyDown(teclaInteractuar))
        {
            return;
        }

        Ray rayo = new Ray(camaraDelPlayer.position, camaraDelPlayer.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(rayo, out hitInfo, distanciaRaycast, capaInteractuable))
        {
            NPC_Cajero cajero = hitInfo.collider.GetComponent<NPC_Cajero>();
            NPC_Jefe jefe = hitInfo.collider.GetComponent<NPC_Jefe>();
            NPC_Guardia guardia = hitInfo.collider.GetComponent<NPC_Guardia>();
            NPC_Cliente cliente = hitInfo.collider.GetComponent<NPC_Cliente>();

            if (cajero != null || guardia != null || cliente != null)
            {
                if (dialogManager.IsDialogueActive)
                {
                    dialogManager.CloseDialogue();
                }
                else
                {
                    if (jefe != null) jefe.TriggerDialoguePublic();
                    else if (guardia != null) guardia.TriggerDialoguePublic();
                    else if (cliente != null) cliente.TriggerDialoguePublic();
                    else if (cajero != null) cajero.TriggerDialoguePublic();
                }
                return;
            }
        }

        if (dialogManager != null && dialogManager.IsDialogueActive)
        {
            return;
        }

        if (rbCarritoAgarrado != null)
        {
            SoltarCarrito();
            return;
        }

        if (objetoAgarrado != null)
        {
            SoltarObjeto();
            return;
        }

        if (Physics.Raycast(rayo, out hitInfo, distanciaRaycast, capaInteractuable))
        {
            ItemInstance itemInstance = hitInfo.collider.GetComponent<ItemInstance>();
            if (itemInstance != null)
            {
                AgarrarObjeto(itemInstance.gameObject);
            }
            else if (hitInfo.collider.CompareTag(tagDelCarrito))
            {
                AgarrarCarrito(hitInfo.collider.gameObject);
            }
            else if (hitInfo.collider.transform.parent != null && hitInfo.collider.transform.parent.CompareTag(tagDelCarrito))
            {
                AgarrarCarrito(hitInfo.collider.transform.parent.gameObject);
            }
        }
    }

    private void pauseOnOff()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);
        }

    }
    #endregion

    #region Lógica del Carrito
    private void AgarrarCarrito(GameObject objetoCarrito)
    {
        Rigidbody rbDelCarrito = objetoCarrito.GetComponentInParent<Rigidbody>();
        if (rbDelCarrito == null) return;

        rbCarritoAgarrado = rbDelCarrito;

        offsetPosCarrito = transform.InverseTransformPoint(rbCarritoAgarrado.position);
        offsetRotCarrito = Quaternion.Inverse(transform.rotation) * rbCarritoAgarrado.rotation;

        colliderCarritoAgarrado = objetoCarrito.GetComponentInParent<Collider>();
        if (colliderCarritoAgarrado != null && miCollider != null)
        {
            Physics.IgnoreCollision(miCollider, colliderCarritoAgarrado, true);
        }

        rbCarritoAgarrado.useGravity = false;
    }

    private void SoltarCarrito()
    {
        if (colliderCarritoAgarrado != null && miCollider != null)
        {
            Physics.IgnoreCollision(miCollider, colliderCarritoAgarrado, false);
        }

        if (rbCarritoAgarrado != null)
        {
            rbCarritoAgarrado.useGravity = true;
        }

        rbCarritoAgarrado = null;
        colliderCarritoAgarrado = null;
    }

    private void ManejarFisicasCarrito()
    {
        Quaternion rotacionObjetivo = transform.rotation * offsetRotCarrito;

        Quaternion rotacionSuave = Quaternion.Slerp(
            rbCarritoAgarrado.rotation,
            rotacionObjetivo,
            suavidadSeguirCarrito * Time.fixedDeltaTime
        );
        rbCarritoAgarrado.MoveRotation(rotacionSuave);

        Vector3 posicionObjetivo = transform.TransformPoint(offsetPosCarrito);

        RaycastHit hitInfo;
        Vector3 puntoChequeoPiso = new Vector3(posicionObjetivo.x, transform.position.y + 1f, posicionObjetivo.z);

        if (Physics.Raycast(puntoChequeoPiso, Vector3.down, out hitInfo, 2f, capaPiso))
        {
            posicionObjetivo.y = hitInfo.point.y + alturaFlotarCarrito;
        }

        Vector3 nuevaPosicion = Vector3.Lerp(rbCarritoAgarrado.position, posicionObjetivo, suavidadSeguirCarrito * Time.fixedDeltaTime);
        rbCarritoAgarrado.MovePosition(nuevaPosicion);
    }
    #endregion

    #region Lógica de Items
    private void AgarrarObjeto(GameObject itemParaAgarrar)
    {
        objetoAgarrado = itemParaAgarrar;
        rbObjetoAgarrado = itemParaAgarrar.GetComponent<Rigidbody>();
        colliderObjetoAgarrado = itemParaAgarrar.GetComponent<Collider>();

        rbObjetoAgarrado.isKinematic = true;
        objetoAgarrado.transform.SetParent(holderParaItems);
        objetoAgarrado.transform.localPosition = Vector3.zero;
        objetoAgarrado.transform.localRotation = Quaternion.identity;

        if (miCollider != null && colliderObjetoAgarrado != null)
        {
            Physics.IgnoreCollision(miCollider, colliderObjetoAgarrado, true);
        }

        objetoAgarrado.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
    private void SoltarObjeto()
    {
        if (miCollider != null && colliderObjetoAgarrado != null)
        {
            Physics.IgnoreCollision(miCollider, colliderObjetoAgarrado, false);
        }

        rbObjetoAgarrado.isKinematic = false;
        objetoAgarrado.transform.SetParent(null);
        objetoAgarrado.layer = LayerMask.NameToLayer("Interactable");

        objetoAgarrado = null;
        rbObjetoAgarrado = null;
        colliderObjetoAgarrado = null;
    }
    #endregion
}