using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Variables
    public Button button;
    #endregion

    #region Métodos Públicos
    public void SceneChange()
    {
        SceneManager.LoadScene("SampleScene");
    }
    #endregion
}