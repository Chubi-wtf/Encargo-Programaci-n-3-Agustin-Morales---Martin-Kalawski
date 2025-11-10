using UnityEngine;
using System.Collections.Generic;

public class NPC_Jefe : MonoBehaviour
{
    #region Variables
    [Header("Referencias de Sistemas")]
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private UI_ManagerDeLista uiManagerLista;
    [SerializeField] private GestorDeListas gestorDeListas;

    [Header("Interacción")]
    public float interactionRange = 3f;
    public Transform player;
    public string npcName = "El Jefe";

    private bool listaEntregada = false;
    private bool compraFinalizada = false;

    private AudioSource miAudioSource;
    #endregion

    #region Eventos de Unity
    void Start()
    {
        if (player == null)
        {
            enabled = false;
            return;
        }

        if (dialogManager == null) dialogManager = Object.FindAnyObjectByType<DialogManager>();
        if (uiManagerLista == null) uiManagerLista = Object.FindAnyObjectByType<UI_ManagerDeLista>();
        if (gestorDeListas == null) gestorDeListas = Object.FindAnyObjectByType<GestorDeListas>();

        miAudioSource = GetComponent<AudioSource>();
    }

    public void MostrarDialogoCastigo(string mensaje)
    {
        dialogManager.ShowDialogue(npcName, mensaje, dialogManager.cajeroPortrait, miAudioSource);
    }

    void Update()
    {
        if (dialogManager == null || player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialogManager.IsDialogueActive)
                {
                    dialogManager.CloseDialogue();
                }
                else
                {
                    TriggerDialogue();
                }
            }
        }
    }
    #endregion

    #region Lógica de Diálogo y Evaluación
    public void FinalizarCompra(float dineroGastado, float totalListaRequerida)
    {
        compraFinalizada = true;
        gestorDeListas.miDinero = dineroGastado;
        gestorDeListas.totalLista = totalListaRequerida;
    }

    public void TriggerDialoguePublic()
    {
        TriggerDialogue();
    }

    private void TriggerDialogue()
    {
        if (!listaEntregada)
        {
            MostrarDialogoInicial();
        }
        else if (compraFinalizada)
        {
            MostrarDialogoFinal();
        }
        else
        {
            MostrarDialogoIntermedio();
        }

        if (miAudioSource != null && miAudioSource.clip != null)
        {
            miAudioSource.Play();
        }
    }

    private void MostrarDialogoInicial()
    {
        if (gestorDeListas != null)
        {
            gestorDeListas.GenerarNuevaLista();
        }

        string dialogo = "¡Eres mi mejor empleado! Necesito que hagas la compra del mes. La lista se ha generado, pulsa TAB para verla. ¡No olvides el dinero que te di!";

        dialogManager.ShowDialogue(npcName, dialogo, dialogManager.cajeroPortrait, miAudioSource);

        listaEntregada = true;
    }

    private void MostrarDialogoIntermedio()
    {
        string dialogo = "¿Aún no has terminado? ¡Date prisa! El supermercado cierra en poco tiempo.";

        dialogManager.ShowDialogue(npcName, dialogo, dialogManager.cajeroPortrait, miAudioSource);
    }

    private void MostrarDialogoFinal()
    {
        string resultado = EvaluarCompra();

        dialogManager.ShowDialogue(npcName, resultado, dialogManager.cajeroPortrait, miAudioSource);
    }

    private string EvaluarCompra()
    {
        int itemsEncontrados = gestorDeListas.ItemsEncontrados;
        int itemsRequeridos = gestorDeListas.listaDeCompraActual.Count;
        float dineroGastado = gestorDeListas.totalLista;

        float diferencia = itemsRequeridos - itemsEncontrados;

        if (diferencia == 0)
        {
            return $"¡Excelente, has traído los {itemsRequeridos} productos! Gastaste ${dineroGastado:F2}. Eres eficiente y ahorrador.";
        }
        else if (diferencia > 0)
        {
            return $"Trajiste solo {itemsEncontrados} de {itemsRequeridos} productos. ¡Te faltaron {diferencia}! Y gastaste ${dineroGastado:F2}. ¿Qué pasó?";
        }
        else
        {
            return $"Trajiste {itemsEncontrados} productos (¡más de los que pedí!). Pero la lista está completa. Gastaste ${dineroGastado:F2}. Buen trabajo, aunque derrochador.";
        }
    }
    #endregion
}