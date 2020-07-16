using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    [SerializeField]
    private GameObject text;

    [SerializeField]
    private GameObject button;

    void Start()
    {
        if (GameObject.Find("titrs_Timer") == null)
            Timer.StartNewTimer("titrs", 0.05f, -1, delegate
            {
                if (transform.localPosition.y < 900)
                    transform.localPosition += new Vector3(0, 1.5f, 0);
                else if (text.GetComponent<Text>().color.a <= 1)
                    text.GetComponent<Text>().color = new Color(1, 1, 1, text.GetComponent<Text>().color.a + 0.004f);
                else
                {
                    button.SetActive(true);
                    Cursor.visible = true;
                    GameObject.Destroy(GameObject.Find("titrs_Timer"));                   
                }
               
            });
    }

    public void to_menu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            button.SetActive(true);
            Cursor.visible = true;
        }
    }
}
