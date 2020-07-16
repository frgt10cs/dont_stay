using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class Hero : MonoBehaviour
{
    public bool hide;

    public bool can;

    [SerializeField]
    private float volume;

    //[SerializeField]
    public int hp;

    public int bantage;

    [SerializeField]
    private int healing;

    public int gear;

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject fence;

    [SerializeField]
    private bool fence_;

    [SerializeField]
    private float dist_to_fence;

    [SerializeField]
    public int adrenaline;

    [SerializeField]
    private float dist_to_adrenaline;

    [SerializeField]
    private float dist_to_enemy;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private GameObject repair;
    
    public AudioClip moan;

    [SerializeField]
    private GameObject map;

    [SerializeField]
    private GameObject pre_menu;

    public void FixedUpdate()
    {
        //Debug.Log(Vector3.Distance(transform.position, repair.transform.position));

        GetComponent<Animator>().SetBool("walkside", false);
        GetComponent<Animator>().SetBool("walkdown", false);
        GetComponent<Animator>().SetBool("walkup", false);

        //volume = gameObject.GetComponent<AudioSource>().volume;        

        dist_to_enemy = Vector3.Distance(transform.position, enemy.transform.position);

        if (dist_to_enemy <= dist_to_adrenaline && GameObject.Find("adr_Timer") == null)
        {
            Timer.StartNewTimer("adr", 1, -1, delegate { if(adrenaline<13)adrenaline += 1; });
        }

        else
        if (dist_to_enemy > dist_to_adrenaline && GameObject.Find("adr_Timer") != null)
        {
            Destroy(GameObject.Find("adr_Timer"));
        }

        //Debug.Log(Vector3.Distance(transform.position, repair.transform.position));
        /*if(Vector3.Distance(transform.position,repair.transform.position)<3.42f && Input.GetKeyDown(KeyCode.E))
        {
            if (gear ==4)
            {
                can = false;
                enemy.GetComponent<Enemy>().can = false;
            }
        }*/

        if (GameObject.Find("haste_Timer") == null)
        {
            if (adrenaline == 12)
            {
                speed = 0.0105f;
                Timer.StartNewTimer("haste", 1, -1, delegate
                {
                    if (adrenaline < 12 && adrenaline != 0)
                        speed -= 0.0002f;
                    adrenaline--;
                    if (adrenaline == 0 || adrenaline < 0)
                    {
                        Destroy(GameObject.Find("haste_Timer"));
                        speed = 0.0085f;
                        adrenaline = 0;
                    }
                });

            }                            
        }
        
        if (can)
        {
            if (Input.GetKeyDown(KeyCode.F) && bantage>0 && hp<10)
            {
                can = false;
                if (GameObject.Find("healing_Timer") == null)
                    Timer.StartNewTimer("healing", 1, -1, delegate
                       {
                           if (healing != 0)
                           {
                               ProgressBar(true, 0.34f);
                               healing--;                              
                           }
                                                                                                                        
                           else
                           {
                               ProgressBar(false, 0);
                               healing = 3;
                               Destroy(GameObject.Find("healing_Timer"));
                               can = true;
                               bantage--;
                               hp = hp + 5 > 10 ? 10 : hp + 5;
                               if (hp == 10)
                               {
                                   if (GameObject.Find("volume_moan_minus_Timer") == null)
                                       Timer.StartNewTimer("volume_moan_minus", 0.2f, -1, delegate
                                       {
                                           gameObject.GetComponent<AudioSource>().volume = volume/100;
                                           if (volume !=0)
                                           {
                                               volume -=10;
                                           }
                                           else
                                           {
                                               Timer.StopTimer("volume_moan_minus");
                                               volume = 100;
                                               Destroy(GameObject.Find("blood partical(Clone)"));
                                               Destroy(GameObject.Find("volume_moan_minus_Timer"));
                                           }
                                       }
                                       );
                               }                                  
                               GameObject.Find("bandage_info").GetComponent<Text>().text = Convert.ToString(bantage);
                           }
                       }
                    );
            }

            if (Input.GetKey(KeyCode.W) && Naezd(new Vector2(-1, 0)))
            {
                GetComponent<Animator>().SetBool("walkup", true);
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += new Vector3(-speed, 0, 0);
            }

            if (Input.GetKey(KeyCode.S) && Naezd(new Vector2(1, 0)))
            {
                GetComponent<Animator>().SetBool("walkdown", true);
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += new Vector3(speed, 0, 0);
            }

            if (Input.GetKey(KeyCode.A) && Naezd(new Vector2(0, -1)))
            {
                if (!(Input.GetKey(KeyCode.W)) && !(Input.GetKey(KeyCode.S)))
                {
                    GetComponent<Animator>().SetBool("walkside", true);
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                transform.position += new Vector3(0, 0, -speed);
            }

            if (Input.GetKey(KeyCode.D) && Naezd(new Vector2(0, 1)))
            {
                if (!(Input.GetKey(KeyCode.W)) && !(Input.GetKey(KeyCode.S)))
                {
                    GetComponent<Animator>().SetBool("walkside", true);
                    transform.localScale = new Vector3(1, 1, 1);
                }
                transform.position += new Vector3(0, 0, speed);
            }

            if(/*Input.GetKey(KeyCode.E)&&*/ Vector3.Distance(transform.position,repair.transform.position)<= 1.85773f && gear==5)
            {
                gear = 0;
                GameObject.Find("gear_info").SetActive(false);
                GameObject.Find("Gear").SetActive(false);
                GameObject.Find("gear_").SetActive(false);
                map.GetComponent<Audio_Map>().play(map.GetComponent<Audio_Map>().opening, 1);
                if (GameObject.Find("screep_Timer") == null)
                    Timer.StartNewTimer("screep", 15, 1, delegate
                    {
                        GameObject.Find("escape").GetComponent<AudioSource>().Play();
                        map.GetComponent<Audio_Map>().play(map.GetComponent<Game>().background, 1);                        
                    }
                    );
                        Vector3[] poss = { new Vector3(-16.5f, 8.47f, -9.78f), new Vector3(-16.5f, 8.47f, -6.5f), new Vector3(-15.73f, 8.47f, -1.16f), new Vector3(-16.5f, 8.47f, -0.24f) };
                int rnd = new System.Random().Next(0, 4);               
                GameObject.Find("escape").transform.position = poss[rnd];
            }            
        }        

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Time.timeScale = 0;
            pre_menu.SetActive(true);
            //GetComponent<AudioListener>().enabled = false;
            AudioListener.volume = 0;
        }
            
    }

    private bool Naezd(Vector2 direction)
    {
        float height = 0.16f;
        float width = 0.04f;
        float distancez = 0.06f;
        float distancex = 0.167f; //0.164
        bool z = new Vector2(0, 1) == direction || new Vector2(0, -1) == direction ? true : false;
        Vector3 direction3 = new Vector3(direction.x, 0, direction.y);
        RaycastHit[] hits = new RaycastHit[3];
        bool[] not_null = new bool[3];
        bool[] is_wall = new bool[3];        

        Physics.Raycast(transform.position, direction3, out hits[0]);
        Physics.Raycast(transform.position + (z ? new Vector3(height, 0) : new Vector3(0, 0, width)), direction3, out hits[1]);
        Physics.Raycast(transform.position + (z ? new Vector3(-height, 0) : new Vector3(0, 0, -width)), direction3, out hits[2]);

        //Debug.DrawRay(transform.position, direction3, Color.red);
        // Debug.DrawRay(transform.position + (z ? new Vector3(height,0) : new Vector3(0,0,width)), direction3,Color.red);
        //Debug.DrawRay(transform.position + (z ? new Vector3(-height,0) : new Vector3(0,0,-width)), direction3, Color.red);

        for (int i = 0; i < hits.Length; i++)
        {
            not_null[i] = hits[i].collider != null;
            is_wall[i] = not_null[i] ? hits[i].collider.tag.Equals("wall") : false;            
        }

        if (!not_null[0] && !not_null[1] && !not_null[2])
            return true;

        for (int i = 0; i < 3; i++)
        {
            bool _z = hits[i].distance <= distancez;
            bool _x = hits[i].distance <= distancex;


            if (is_wall[i] && (z ? _z : _x))
                return false;
        }
        return true;
    }    

    public void ProgressBar(bool work,float amount)
    {
        GameObject.Find("pb_back").GetComponent<Image>().enabled = work;
        GameObject.Find("pb_front").GetComponent<Image>().enabled = work;
        if (work)
            GameObject.Find("pb_front").GetComponent<Image>().fillAmount += amount;
        else
            GameObject.Find("pb_front").GetComponent<Image>().fillAmount = amount;
    }

    public void CanvasUpdate()
    {
        GameObject.Find("bandage_info").GetComponent<Text>().text = Convert.ToString(GetComponent<Hero>().bantage);
        GameObject.Find("gear_info").GetComponent<Text>().text = Convert.ToString(GetComponent<Hero>().gear);
    }
}


