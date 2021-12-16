using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceWalls : MonoBehaviour
{
    bool onGround;
    public int sideValue;

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

     void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    public bool OnGround()
    {
        return onGround;
    }
}
