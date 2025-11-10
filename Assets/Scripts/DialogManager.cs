using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DialogManager : MonoBehaviour
{
    #region Variables y Propiedades
    public bool IsDialogueActive { get; private set; } = false;

    private AudioSource currentAudioSource = null;

    [Header("Retratos (Texturas)")]
    public Sprite cajeroPortrait;
    public Sprite clientePortrait;
    public Sprite guardiaPortrait;

    [Header("Panel de Diálogo Único")]
    [SerializeField] private GameObject panelDialogoPrincipal;

    [Header("Componentes de Texto Único")]
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoContenido;

    private List<string> dialogosCajero = new List<string>
    {
        "¡Bienvenido! ¿Viene por la promoción de cerebros?",
        "El checkout está por allá. ¡Solo si ha encontrado todo, claro!",
        "La máquina de tarjetas está rota. Solo efectivo, por favor.",
        "Señor, tiene una araña en la cabeza. ¿La quiere en bolsa?",
        "No hemos tenido un cliente vivo en... ¡mucho tiempo!",
        "La carne está fresca, ¡la matamos esta mañana! (Es broma... creo).",
        "Por favor, no deje sus miembros cortados en el pasillo 3.",
        "Recuerde: 'Verduras Gratis' si compra dos latas de ojos.",
        "Escuché que tienen 50% de descuento en la sección de licores embrujados.",
        "¡No olvide su lista! Es el último día de 2x1 en la sección de panadería."
    };

    private List<string> dialogosGuardia = new List<string>
    {
        "¡Alto! ¿Tienes permiso para existir? ¿O solo vienes por el pan?",
        "Rango... rango... ¡todo está en orden en el rango!",
        "No se permiten espectros en el pasillo de lácteos.",
        "Te vigilo... tengo tres ojos puestos en ti.",
        "¡Los humanos son tan lentos! Muevete o te convertirás en un producto.",
        "Si necesitas saber algo, pregúntale a ese cliente... si es que habla.",
        "¡Vi una oferta de '2x1 en Carnes'! Busca el cartel rojo.",
        "Alguien dijo que los cereales están al 50%. No se lo digas a nadie.",
        "¡Protegiendo los cerebros, digo, los productos!",
        "Mi nombre es Bob, y mi trabajo es asustarte. Funciona, ¿verdad?"
    };

    private List<string> dialogosCliente = new List<string>
    {
        "Uhh... la leche está gratis... creo... (balbuceo)",
        "¡Carne! ¡Necesito más carne! ¡Rarrgghh!",
        "Mi cabeza me duele... ¿Dónde está el ibuprofeno de alma?",
        "¿Quién puso el pasillo 7 al revés? ¡Es una pesadilla!",
        "Solo necesito pan, solo pan... (balbuceo constante)",
        "Si miras al final del pasillo 9, las verduras son gratis... (susurro)",
        "No encuentro mi cabeza... ¿has visto mi cabeza?",
        "Yo... yo quería galletas... pero solo tienen huesos...",
        "¡50% en todo! ¡Agarra lo que puedas! ¡Mentira! ¡Solo los licores! ¡Agh!",
        "¡Mi carrito está roto, como mi voluntad de vivir!",
        "Tengo que darme prisa... ¡El jefe es un tirano!"
    };
    #endregion

    #region Métodos de Control
    void Start()
    {
        CloseDialogue();
    }

    public void ShowDialogue(string name, string text, Sprite portrait, AudioSource source)
    {
        if (panelDialogoPrincipal != null)
        {
            if (IsDialogueActive)
            {
                CloseDialogue();
            }

            currentAudioSource = source;

            panelDialogoPrincipal.SetActive(true);
            IsDialogueActive = true;

            if (textoNombre != null)
            {
                textoNombre.text = name;
                textoNombre.ForceMeshUpdate();
            }

            if (textoContenido != null)
            {
                textoContenido.text = text;
                textoContenido.ForceMeshUpdate();
            }

            if (currentAudioSource != null && currentAudioSource.clip != null)
            {
                currentAudioSource.Play();
            }
        }
    }

    public void CloseDialogue()
    {
        if (currentAudioSource != null && currentAudioSource.isPlaying)
        {
            currentAudioSource.Stop();
            currentAudioSource = null;
        }

        if (panelDialogoPrincipal != null) panelDialogoPrincipal.SetActive(false);

        IsDialogueActive = false;
    }
    #endregion

    #region Métodos de Obtención de Diálogos
    public string GetRandomCajeroDialogue()
    {
        if (dialogosCajero.Count == 0) return "El cajero no tiene nada que decir.";
        int index = Random.Range(0, dialogosCajero.Count);
        return dialogosCajero[index];
    }

    public string GetRandomGuardiaDialogue()
    {
        if (dialogosGuardia.Count == 0) return "El guardia está en silencio.";
        int index = Random.Range(0, dialogosGuardia.Count);
        return dialogosGuardia[index];
    }

    public string GetRandomZombieDialogue()
    {
        if (dialogosCliente.Count == 0) return "El cliente solo balbucea.";
        int index = Random.Range(0, dialogosCliente.Count);
        return dialogosCliente[index];
    }
    #endregion
}