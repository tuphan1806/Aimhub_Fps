using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SelecteLevel1()
    {
        SceneManager.LoadScene(2);
    }

    public void SelecteLevel2()
    {
        SceneManager.LoadScene(3);
    }
}
