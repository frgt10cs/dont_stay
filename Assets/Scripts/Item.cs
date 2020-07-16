using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public GameObject hero;

    [SerializeField]
    private bool gear;

	void Update ()
    {       
        if (GetComponent<MeshRenderer>().enabled && Vector3.Distance(transform.position, hero.transform.position) <= 2.8217f)
        {
            if (gear && hero.GetComponent<Hero>().gear<5)
                hero.GetComponent<Hero>().gear++;
            else if (!gear && hero.GetComponent<Hero>().bantage < 2)
                hero.GetComponent<Hero>().bantage++;

            hero.GetComponent<Hero>().CanvasUpdate();

            Destroy(gameObject);
        }
	}
}
