using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private GameObject _HUD;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "obj")
        {
            _HUD.SetActive(true);
            Move.turnSpeed = 0;
            //Cursor.visible = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "obj")
        {
            Move.turnSpeed = 4f;
            _HUD.SetActive(false);
           // Cursor.visible = false;
        }
    }
}
