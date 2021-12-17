using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    bool turnPossible = true;

    //human inputs
    //gaameobjects for the button
    public GameObject diceButton;
    [HideInInspector] public int rolledhumanDice;

    public Dice dice;

    void Awake()
    {
        instance = this;
        for(int i = 0; i < playerList.Count;i++)
        {
            if(SaveSettings.players[i] == "HUMAN")
            {
                playerList[i].playerType = Entity.PlayerTypes.Human;
            }
            if (SaveSettings.players[i] == "CPU")
            {
                playerList[i].playerType = Entity.PlayerTypes.CPU;
            }
            if (SaveSettings.players[i] == "NoPlayer")
            {
                playerList[i].playerType = Entity.PlayerTypes.NoPlayer;
            }

        }
    }

    void Start()
    {
        ActivateButton(false);
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].playerType == Entity.PlayerTypes.NoPlayer)
            {
                for (int j = 0; j < playerList[i].mystones.Length; j++)
                {
                    playerList[i].mystones[j].SetSelector(false);
                    playerList[i].mystones[j].GetComponent<MeshRenderer>().enabled = false;

                }

            }

        }
        //randomizing player

        int randomPlayer = Random.Range(0, playerList.Count);
        activePlayer = randomPlayer;
        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " starts first..!!");
    }
    void Update()
    {

        if(playerList[activePlayer].playerType == Entity.PlayerTypes.CPU)
        {
            switch (state)
            {
                case States.RollDice:
                    {
                        if(turnPossible)
                        {
                            StartCoroutine(RollDiceDelay());
                            state = States.Waiting;
                        }
                        
                    }
                    break;
                case States.Waiting:
                    {

                    }
                    break;
                case States.SwitchPlayer:
                    {
                        if (turnPossible)
                        {
                            StartCoroutine(SwitchPlayer());
                            state = States.Waiting;
                        }
                        
                    }
                    break;
            }
        }
        if (playerList[activePlayer].playerType == Entity.PlayerTypes.Human)
        {
            switch (state)
            {
                case States.RollDice:
                    {
                        if (turnPossible)
                        {
                            ActivateButton(true);
                            state = States.Waiting;
                        }

                    }
                    break;
                case States.Waiting:
                    {

                    }
                    break;
                case States.SwitchPlayer:
                    {
                        if (turnPossible)
                        {
                            StartCoroutine(SwitchPlayer());
                            state = States.Waiting;
                        }

                    }
                    break;
            }
        }
        if (playerList[activePlayer].playerType == Entity.PlayerTypes.NoPlayer)
        {
            state = States.SwitchPlayer;
            StartCoroutine(SwitchPlayer());
        }


    }

    void CPUDice()
    {
        dice.RollDice();
    }

    public void RollDice(int _diceNumber)//call this from dice
    {
        //int diceNumber = Random.Range(1, 7);
        //int diceNumber = 6;
        int diceNumber = _diceNumber;

        if(playerList[activePlayer].playerType == Entity.PlayerTypes.CPU)
        {
            if (diceNumber == 6)
            {
                //check for the start node
                CheckStartNode(diceNumber);
            }
            if (diceNumber < 6)
            {
                //check for move
                MoveAStone(diceNumber);
            }
        }

        if (playerList[activePlayer].playerType == Entity.PlayerTypes.Human)
        {
            rolledhumanDice = _diceNumber;
            HumanRollDice();
        }

        print(diceNumber);
        InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + " " + "has rolled" + " " + _diceNumber );
    }

  /* public void ReRoll()
    {
        if(playerList[activePlayer].playerType== Entity.PlayerTypes.Human)
        {
            ActivateButton(true);
            HumanRollDice();
            

        }

        if (playerList[activePlayer].playerType == Entity.PlayerTypes.CPU)
        {
            CheckStartNode(Random.Range(1, 7));
        }
    }*/

    IEnumerator RollDiceDelay()
    {
        yield return new WaitForSeconds(2);
        // RollDice();
        CPUDice();
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
        if(playerList[activePlayer].playerType == Entity.PlayerTypes.NoPlayer)
        {
            InfoBox.instance.ShowMessage("");
        }
        else
        {
            InfoBox.instance.ShowMessage(playerList[activePlayer].playerName + "'s Turn..!!");
        }
        
        state = States.RollDice;
    }

    public void ReportTurnPossible(bool possible)
    {
        turnPossible = possible;
    }

    public void ReportWinning()
    {
        playerList[activePlayer].hasWon = true;

        for(int i=0; i <SaveSettings.winners.Length; i++)
        {
            SaveSettings.winners[i] = playerList[activePlayer].playerName;
            break;
        }
    }

    #region Player type is Human
    void ActivateButton(bool on)
    {
        diceButton.SetActive(on);
    }

    public void DeactivateSelectors()
    {
        for(int i = 0; i < playerList.Count; i++)
        {
            //each player stones
            for(int j =0; j < playerList[i].mystones.Length; j++)
            {
                playerList[i].mystones[j].SetSelector(false);
            }
        }
    }

    public void HumanRoll()
    {
        dice.RollDice();
        ActivateButton(false);
    }
    
    //dicebutton
    public void HumanRollDice()
    {
        //ActivateButton(false);

        //roll dice
        //rolledhumanDice = Random.Range(1, 7);
        //rolledhumanDice = 6;
        //movablestonelist
        List<Stone> movableStones = new List<Stone>();
       


       //start node full check
        bool startNodeFull = false;
        for (int i = 0; i < playerList[activePlayer].mystones.Length; i++)
        {
            if (playerList[activePlayer].mystones[i].currentNode == playerList[activePlayer].mystones[i].startNode)
            {
                startNodeFull = true;
                break;
            }
        }

        //rolldicenumber <6 
        if(rolledhumanDice < 6)
        {
            movableStones.AddRange(PossibleStones());
        }
        //rolldicenumber ==6 && !startnode
        if(rolledhumanDice == 6 && !startNodeFull)
        {
            //inside base check
            for(int i=0; i <playerList[activePlayer].mystones.Length; i++)
            {
                if(!playerList[activePlayer].mystones[i].ReturnIsOut())
                {
                    movableStones.Add(playerList[activePlayer].mystones[i]);
                }
            }

            //outside check
            movableStones.AddRange(PossibleStones());
        }

        //rolldicenumber ==6 && startnode
        else if(rolledhumanDice == 6 && startNodeFull)
        {
            movableStones.AddRange(PossibleStones());
        }

        //activate all possible selectors
        if(movableStones.Count>0)
        {
             for(int i =0; i < movableStones.Count; i++)
             {
                  movableStones[i].SetSelector(true);
             }
             
         }
        else
        {
            state = States.SwitchPlayer;
        }
    }

    List <Stone> PossibleStones()
    {
        List<Stone> tempList = new List<Stone>();


        for (int i = 0; i < playerList[activePlayer].mystones.Length; i++)
        {
            //if the stone is out already
            if (playerList[activePlayer].mystones[i].ReturnIsOut())
            {
                if (playerList[activePlayer].mystones[i].IsOut)
                {
                    playerList[activePlayer].mystones[i].SetSelector(true);
                }
                if (playerList[activePlayer].mystones[i].CheckPossibleKick(playerList[activePlayer].mystones[i].stoneId, rolledhumanDice))
                {
                    tempList.Add(playerList[activePlayer].mystones[i]);
                    continue;
                }
               
                if (playerList[activePlayer].mystones[i].CheckPossibleMove(rolledhumanDice))
                {
                    tempList.Add(playerList[activePlayer].mystones[i]);

                }

              
            }
        }

        return tempList;
    }
    #endregion

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

}

