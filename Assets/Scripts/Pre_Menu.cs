using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pre_Menu : MonoBehaviour
{
    public void contiune()
    {
        Cursor.visible = false;
        //GameObject.Find("hero").GetComponent<AudioListener>().enabled = true;
        AudioListener.volume = 1;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }       

    public void menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void exit()
    {
        Application.Quit();
    }
}
