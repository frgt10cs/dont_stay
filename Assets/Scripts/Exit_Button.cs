using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_Button : MonoBehaviour
{
    private void OnMouseDown()
    {        
        Application.Quit();       
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
