using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    [SerializeField]
    private GameObject gear;

    [SerializeField]
    private GameObject bantage;

    [SerializeField]
    private Vector3[] gear_spawn;

    [SerializeField]
    private Vector3[] bantage_spawn;

    [SerializeField]
    private GameObject load_screen;

    [SerializeField]
    private GameObject[] wicks;

    [SerializeField]
    private Sprite screen_2;

    public AudioClip background;

    public static System.Random rnd = new System.Random();

    private void Start()
    {
        Time.timeScale = 0;
        spawn(gear_spawn, gear);
        spawn(bantage_spawn, bantage);
    }

    private void Update()
    {
        /*if (Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene("Final");
        }*/
    }

    public void play()
    {
        if (GameObject.Find("Play").GetComponentInChildren<Text>().text == "Next")
        {
            GameObject.Find("Image").GetComponent<Image>().sprite = screen_2;
            GameObject.Find("Play").GetComponentInChildren<Text>().text = "Play";
        }
        else
        {
            load_screen.SetActive(false);
            Time.timeScale = 1;
            GameObject.Find("mapa").GetComponent<Audio_Map>().play(background, 1);
            GameObject.Find("enemy").GetComponent<Enemy>().enabled = true;
            foreach (GameObject wick in wicks)
                wick.SetActive(true);
            Cursor.visible = false;
        }
             
    }   

    private void spawn(Vector3[] vect,GameObject obj)
    {
        for (int i = 0; i < (obj!=bantage?vect.Length:6); i += (obj != bantage ? 2 : 3))
        {
            
            int rnd_ = rnd.Next(0, (obj != bantage ? 2:3));
            if (obj == bantage)
                Debug.Log(rnd_+i);
            GameObject item = Instantiate(obj, vect[rnd_ + i], new Quaternion());
            item.GetComponent<Item>().hero = GameObject.Find("hero");
            item.GetComponent<Visible>().hero = GameObject.Find("hero");
            item.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }
}
