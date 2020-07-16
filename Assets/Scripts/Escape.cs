using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escape : MonoBehaviour {

    [SerializeField]
    private float distance;

    [SerializeField]
    private GameObject hero;
	
	void Update ()
    {
        distance = Vector3.Distance(transform.position, hero.transform.position);
        if(distance<= 3.351184f)
        {
            GameObject.Find("hero").transform.FindChild("Main Camera").GetComponentInChildren<MeshRenderer>().enabled = true;
            SceneManager.LoadScene("Final");
        }            
	}
}
