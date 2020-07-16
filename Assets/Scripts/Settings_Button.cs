using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings_Button : MonoBehaviour {

    [SerializeField]
    private GameObject dark;

    [SerializeField]
    private GameObject name;

    [SerializeField]
    private GameObject back;

    [SerializeField]
    private GameObject sry;

    private void OnMouseDown()
    {
        name.SetActive(false);
        back.SetActive(true);
        dark.SetActive(true);
        sry.SetActive(true);
    }

    private void OnMouseEnter()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
    }

    private void OnMouseExit()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
    }

    public void Close()
    {
        name.SetActive(true);
        back.SetActive(false);
        dark.SetActive(false);
        sry.SetActive(false);
    }
}
