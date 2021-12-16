using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb;

    bool hasLanded;
    bool thrown;

    Vector3 initPosition;

    public DiceWalls[] diceWalls;
    public int diceValue;


    // Start is called before the first frame update
    void Start()
    {

        initPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

    }

    public void RollDice()
    {

        Reset();
        if (!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
        }

        else if(thrown && hasLanded)
        {
            //reset dice
            Reset();
        }
    }

     void Reset()
    {
        transform.position = initPosition;
        rb.isKinematic = false;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

     void Update()
    {
        if (rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            rb.isKinematic = true;


            // walls value check
            WallValueCheck();
        }
        else if (rb.IsSleeping() && hasLanded && diceValue == 0)
        {
            //roll again
            RollAgain();
        }



    }

    void RollAgain()
    {
        Reset();
        thrown = true;
        rb.useGravity = true;
        rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
    }

    void WallValueCheck()
    {
        diceValue = 0;
        foreach(DiceWalls wall in diceWalls)
        {
            if(wall.OnGround())
            {
                diceValue = wall.sideValue;

                //report result to gamemanger
                GameManager.instance.RollDice(diceValue);
            }
        }
    }
}

  
