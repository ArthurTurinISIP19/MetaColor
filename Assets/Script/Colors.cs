using System;
using UnityEngine;

public class Colors : MonoBehaviour
{

    [SerializeField] public static Color _color = Color.black;
    private GameObject _target;
    private float _rotateSpeed = 10000;
    private bool _isRotateLeft = false;

    Color[] posColors = { Color.black, Color.red, Color.blue, Color.green };

    public void SetColor(int color)
    {
        _color = posColors[color];
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Paintable")
            _target = GameObject.Find(other.gameObject.name);
    }

    public void OnRotateObjectLeftDown()
    {
        _isRotateLeft = true;
    }
    public void OnRotateObjectLeftUp()
    {
        _isRotateLeft = false;
    }
    public void FixedUpdate()
    {
        if (_isRotateLeft)
        {
            _target.transform.Rotate(0, 0, Time.deltaTime * _rotateSpeed);
        }
        else
        {

        }

    }
}
