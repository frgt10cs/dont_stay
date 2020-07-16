using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	
	

    public void disap() // начало анимации атаки
    {
        GameObject.Find("enemy").GetComponent<Enemy>().disap();
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void show() // окончание анимации атаки
    {
        GameObject.Find("enemy").GetComponent<Enemy>().show();
        GameObject.Find("hero").GetComponent<AudioSource>().volume = GameObject.Find("hero").GetComponent<Hero>().hp>0?1:0;
        GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("hero").GetComponent<AudioSource>().clip = GameObject.Find("hero").GetComponent<Hero>().moan;
        GameObject.Find("hero").GetComponent<AudioSource>().Play();
    }
}
