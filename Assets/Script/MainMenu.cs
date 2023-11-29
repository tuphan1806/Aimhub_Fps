using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsPanel;

    public void SelectLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void SelectOptions()
    {
        mainMenu.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void SelectMenu()
    {
        mainMenu.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
