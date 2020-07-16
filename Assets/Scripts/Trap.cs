using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap : MonoBehaviour {

    
    public GameObject hero;
   
    private Vector3[] hero_pos = new Vector3[3];

    [SerializeField]
    private Texture2D gotcha;

    [SerializeField]
    private int to_freedom;

    void Update ()
    {
        hero_pos[0] = new Vector3(hero.transform.position.x + 0.16f, hero.transform.position.y, hero.transform.position.z);
        hero_pos[1]= new Vector3(hero.transform.position.x + 0.16f, hero.transform.position.y, hero.transform.position.z+ 0.04f);
        hero_pos[2] = new Vector3(hero.transform.position.x + 0.16f, hero.transform.position.y, hero.transform.position.z- 0.04f);
     
        if (Vector3.Distance(transform.position, hero_pos[0]) <= 3.410056f || Vector3.Distance(transform.position, hero_pos[1]) <= 3.410056f || Vector3.Distance(transform.position, hero_pos[2]) <= 3.410056f) 
        {            
            transform.position = new Vector3(hero.transform.position.x+0.1f, 12, hero.transform.position.z);
            hero.GetComponent<Hero>().can = false;
            gameObject.GetComponent<MeshRenderer>().material.mainTexture = gotcha;
            gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 0);
            transform.localScale = new Vector3(hero.transform.localScale.x > 0 ? 0.011f : -0.011f, transform.localScale.y, transform.localScale.z);            
            if (GameObject.Find("gotcha_Timer") == null)
                Timer.StartNewTimer("gotcha", 1, -1, delegate
                {
                    if (to_freedom != 0 && Input.GetKey(KeyCode.Space))
                    {
                        to_freedom--;
                        hero.GetComponent<Hero>().ProgressBar(true, 0.25f);                        
                    }
                        
                    else
                    {
                        if (to_freedom == 0)
                        {
                            hero.GetComponent<Hero>().can = true;
                            Destroy(gameObject);
                        }
                        to_freedom = 4;
                        hero.GetComponent<Hero>().ProgressBar(false, 0);                        
                        Destroy(GameObject.Find("gotcha_Timer"));
                    }
                }
                );            
        }
    }
}
