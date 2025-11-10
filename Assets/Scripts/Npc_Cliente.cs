using UnityEngine;

public class NPC_Cliente : MonoBehaviour
{
    #region Variables
    public float interactionRange = 4f;
    public Transform player;
    public string npcName = "Cliente Zombie";
    private DialogManager dialogManager;
    private AudioSource miAudioSource;
    #endregion

    #region Ciclo de Vida
    void Start()
    {
        if (player == null)
        {
            enabled = false;
            return;
        }

        dialogManager = Object.FindAnyObjectByType<DialogManager>();

        if (dialogManager == null)
        {
            enabled = false;
            return;
        }

        miAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (dialogManager == null) return;
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
                    TriggerDialoguePublic();
                }
            }
        }
    }
    #endregion

    #region Diálogo
    public void TriggerDialoguePublic()
    {
        string dialogoTexto = dialogManager.GetRandomZombieDialogue();

        if (dialogoTexto != null)
        {
            dialogManager.ShowDialogue(
                npcName,
                dialogoTexto,
                dialogManager.clientePortrait,
                miAudioSource
            );
        }
    }
    #endregion
}