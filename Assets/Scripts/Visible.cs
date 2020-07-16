using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visible : MonoBehaviour {
    
    public GameObject hero;

    [SerializeField]
    private float distance;

    private void Start()
    {
        hero= GameObject.Find("hero");
    }

    void Update ()
    {
        Vector3 vect = new Vector3(transform.position.x,hero.transform.position.y,transform.position.z);        
        
        distance = Vector3.Distance(hero.transform.position, vect);

        RaycastHit ray = new RaycastHit();
        Physics.Raycast(vect, hero.transform.position-vect, out ray,distance);

        //Debug.DrawRay(vect, hero.transform.position - transform.position);

        if (distance <= (gameObject.tag!="trap"?1.5f:1.022618f) && ray.collider==null)            
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        else
            gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
