using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play_Button : MonoBehaviour
{
    [SerializeField]
    private GameObject load_screen;

    [SerializeField]
    private Texture2D cursor;

    public void Start()
    {
        AudioListener.volume = 1;
        Cursor.SetCursor(cursor,new Vector2(8,6),CursorMode.ForceSoftware);
        Cursor.visible = true;
    }

    private void OnMouseDown()
    {
        GameObject.Find("Text").GetComponent<Text>().enabled = false;
        GameObject.Find("Menyu_1_3").GetComponent<AudioSource>().Stop();
        load_screen.SetActive(true);
        SceneManager.LoadSceneAsync("DS_game");
    }

    private void OnMouseEnter()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
    }

    private void OnMouseExit()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
    }
}
