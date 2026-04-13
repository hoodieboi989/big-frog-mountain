using UnityEngine;  
using UnityEngine.SceneManagement;  

public class TitleScreenManager : MonoBehaviour  
{  
    public void OnClick() 
    {  
        SceneManager.LoadScene("level 1"); 
    }  
}  