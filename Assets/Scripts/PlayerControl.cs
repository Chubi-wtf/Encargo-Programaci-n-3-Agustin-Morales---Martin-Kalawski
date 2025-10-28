using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Transform playerCamera;
    private Rigidbody rb;

    [Header("Movimiento")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    private float currentSpeed;

    [Header("Cámara")]
    [SerializeField] private float mouseSensitivity = 100f;

    [Header("Interacción (Raycast)")]
    [SerializeField] private float raycastDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Interacción (Agarrar)")]
    [SerializeField] private Transform pickupHolder; 
    [SerializeField] private KeyCode interactionKey = KeyCode.E; 

    private GameObject heldObject; 
    private Rigidbody heldObjectRb; 

    [Header("UI de Interacción")]
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSpeed = walkSpeed;

        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        moveInput = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        HandleRaycastUI(); 
        HandleInteractionInput(); 
    }

    private void HandleRaycastUI()
    {
        if (heldObject != null)
        {
            itemInfoPanel.SetActive(false);
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayer))
        {
            ItemInstance item = hit.collider.GetComponent<ItemInstance>();
            if (item != null)
            {
                itemInfoPanel.SetActive(true);
                itemNameText.text = item.data.itemName;
                itemPriceText.text = $"${item.data.price} - {item.data.itemCategory}";
            }
            else
            {
                itemInfoPanel.SetActive(false);
            }
        }
        else
        {
            itemInfoPanel.SetActive(false);
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            if (heldObject != null)
            {
                DropObject();
            }
            else
            {
                Ray ray = new Ray(playerCamera.position, playerCamera.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayer))
                {
                    ItemInstance item = hit.collider.GetComponent<ItemInstance>();
                    if (item != null)
                    {
                        PickUpObject(item.gameObject);
                    }
                }
            }
        }
    }

    private void PickUpObject(GameObject item)
    {
        heldObject = item;
        heldObjectRb = item.GetComponent<Rigidbody>();

        
        heldObjectRb.isKinematic = true;

        heldObject.transform.SetParent(pickupHolder);

        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity; 

        heldObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void DropObject()
    {
        heldObjectRb.isKinematic = false;

        heldObject.transform.SetParent(null);

        heldObject.layer = LayerMask.NameToLayer("Interactable");

        heldObject = null;
        heldObjectRb = null;
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = moveInput * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y; 
        rb.linearVelocity = targetVelocity;
    }
}