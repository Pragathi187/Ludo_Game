﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [System.Serializable]
    public class Entity
    {
        public string playerName;
        public Stone[] mystones;
        public bool hasTurn;
        public enum PlayerTypes
        {
            Human,
            CPU,
            NoPlayer
        }
        public PlayerTypes playerType;
        public bool hasWon;

    }

    public List<Entity> playerList = new List<Entity>();

    //statemachines
    public enum States
    {
        Waiting,
        RollDice,
        SwitchPlayer
    }

    public States state;

    public int activePlayer;
    bool switchingPlayer;

    //human inputs
    //gaameobjects for the button
    //int rolledhumanDice;

    void Awake()
    {
        instance = this;
    }
    void Update()
    {

        if(playerList[activePlayer].playerType == Entity.PlayerTypes.CPU)
        {
            switch (state)
            {
                case States.RollDice:
                    {
                        StartCoroutine(RollDiceDelay());
                        state = States.Waiting;
                    }
                    break;
                case States.Waiting:
                    {

                    }
                    break;
                case States.SwitchPlayer:
                    {
                        StartCoroutine(SwitchPlayer());
                        state = States.Waiting;
                    }
                    break;
            }
        }
       
    }

    void RollDice()
    {
        int diceNumber = Random.Range(1, 7);
        //int diceNumber = 6;
        if (diceNumber==6)
        {
            //check for the start node
            CheckStartNode(diceNumber);
        }
        if(diceNumber <6)
        {
            //check for move
            MoveAStone(diceNumber);
        }
        print(diceNumber);
    }

    IEnumerator RollDiceDelay()
    {
        yield return new WaitForSeconds(2);
        RollDice();
    }

    void CheckStartNode(int diceNumber)
    {
        
        //if anyone on the start node
        bool startNodeFull = false;
        for(int i=0; i < playerList[activePlayer].mystones.Length; i++)
        {
            if(playerList[activePlayer].mystones[i].currentNode==playerList[activePlayer].mystones[i].startNode)
            {
                startNodeFull = true;
                break;
            }
        }
        if(startNodeFull)
        {
            //move a stone
            MoveAStone(diceNumber);
            print("start node full");
        }
        else  
        {
            //if atleast one is inside the base
            for (int i = 0; i < playerList[activePlayer].mystones.Length; i++)
            {
                if(!playerList[activePlayer].mystones[i].ReturnIsOut())
                {
                    //leave the base
                    
                    playerList[activePlayer].mystones[i].LeaveBase();
                    state = States.Waiting;
                    return;
                }
            }

            //move a stone  
            MoveAStone(diceNumber);

        }
    }

    void MoveAStone(int diceNumber)
    {
        List<Stone> movableStones = new List<Stone>();
        List<Stone> moveKickStones = new List<Stone>();

       
        //fill the lists
        for(int i=0; i<playerList[activePlayer].mystones.Length; i++)
        {
            
            if (playerList[activePlayer].mystones[i].ReturnIsOut())
            {
               
                //check for possible kick
                if (playerList[activePlayer].mystones[i].CheckPossibleKick(playerList[activePlayer].mystones[i].stoneId, diceNumber))
                {
                    moveKickStones.Add(playerList[activePlayer].mystones[i]);
                    continue;
                }

                //check for possible move
                if (playerList[activePlayer].mystones[i].CheckPossibleMove(diceNumber))
                {
                    
                    movableStones.Add(playerList[activePlayer].mystones[i]);
                   
                }
            }
        }

        //perform kick if possible
        if(moveKickStones.Count>0)
        {

            int num = Random.Range(0, moveKickStones.Count);
            moveKickStones[num].StartTheMove(diceNumber);
            state = States.Waiting;
            return;
        }
        //perform move if possible
        if (movableStones.Count > 0)
        {

            int num = Random.Range(0, movableStones.Count);
            movableStones[num].StartTheMove(diceNumber);
            state = States.Waiting;
            return;
        }

        //none is possible
        //switching the player
        state = States.SwitchPlayer;
        Debug.Log("Should Switch Player");


    }

    IEnumerator SwitchPlayer()
    {
        if(switchingPlayer)
        {
            yield break;
        }

        switchingPlayer = true;

        yield return new WaitForSeconds(2);
        //set next player
        SetNextActivePlayer();

        switchingPlayer = false;
    }

    void SetNextActivePlayer()
    {
        activePlayer++;
        activePlayer %= playerList.Count;

        int available = 0;
        for(int i=0; i < playerList.Count; i++)
        {
            if(!playerList[i].hasWon)
            {
                available++;
            }
        }

        if(playerList[activePlayer].hasWon && available>1)
        {
            SetNextActivePlayer();
            return;
        }
        else if(available<2)
        {
            state = States.Waiting;
            return;
        }

        state = States.RollDice;
    }
}
