using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Info", menuName = "ScriptableObjects", order = 21)]
public class ObjInfo : ScriptableObject
{
    public GameObject oobj;
    public Material material;
}
