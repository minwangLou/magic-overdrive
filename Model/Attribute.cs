using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute
{
    public int idLevel;
    public int id;
    public string name;
    public float value;

    public void Info()
    {
        Debug.Log("Attribute: " +
                    "\nid: " + id + 
                    "\nname: " + name + 
                    "\nvalue: " + value);
    }
}
