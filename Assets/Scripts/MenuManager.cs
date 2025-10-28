using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public Button button;
  
    public void SceneChange()
    {

        SceneManager.LoadScene("SampleScene");
      
    }
}
