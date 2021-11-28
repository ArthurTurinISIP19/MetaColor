using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEdit : MonoBehaviour
{
    public void OpenEditor()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
