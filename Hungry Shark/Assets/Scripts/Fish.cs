using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public string _name = "";
    public int _eatenValue = 100;

    public bool OnConsumed()
    {
        return GameManager._instance.CheckConsumeTarget(_name);
    }
}
