using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEdit : MonoBehaviour
{
    [SerializeField] private GameObject _HUD;

    public void OpenEditor()
    {
        if (_HUD.activeSelf)
        {
            _HUD.SetActive(false);
        }
        else
        {
            _HUD.SetActive(true);
        }
    }
}
