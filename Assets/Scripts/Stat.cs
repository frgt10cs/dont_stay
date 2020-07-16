using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour {

    [SerializeField]
    private float distance_to_hide;

    [SerializeField]
    private GameObject hero;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private float distance_between;

    [SerializeField]
    private int hiding;

    void Update ()
    {        
        distance_between = Vector3.Distance(transform.position, hero.transform.position);
        if (distance_between <= distance_to_hide)
        {
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(transform.position, (hero.transform.position - transform.position).normalized, out hit, Vector3.Distance(transform.position, hero.transform.position));
            if (hit.collider == null)
            {
                if (GameObject.Find("hide_Timer") == null && !(hero.GetComponent<Hero>().hide))
                {
                    Timer.StartNewTimer("hide", 0.5f, 1, delegate
                    {
                        hero.GetComponent<Hero>().hide = true;
                        hero.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
                        hiding = 0;
                    }
                   );
                }
            }
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F))
        {
            hiding = 0;
            hero.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            hero.GetComponent<Hero>().hide = false;
            if((GameObject.Find("hide_Timer")))
                Destroy(GameObject.Find("hide_Timer"));
        }
    }
}
