using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventComponent : MonoBehaviour
{
    public void NewGameButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsButton()
    {
        
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
