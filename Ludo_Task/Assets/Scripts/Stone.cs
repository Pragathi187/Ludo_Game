using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{

    public int stoneId;
    [Header("ROUTES")]
    public Route commonRoute; //outer route
    public Route finalRoute;//inner route

    public List<Node> fullRoute = new List<Node>();

    [Header("NODES")]
    public Node startNode;//start point
    public Node currentNode;
    public Node goalNode;
    public Node baseNode;//node at home

    int routePosition;
    int startNodeIndex;
    int steps;//dice output
    int doneSteps;

    [Header("BOOLS")]
    public bool IsOut;
    public bool isMoving;

    bool HasTurn;

    [Header("SELECTOR")]
    public GameObject selector;

    //arc movement
    float amplitude = 0.5f;
    float ctime = 0f;


     void Start()
    {
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
        CreateFullRoute();
        SetSelector(false);
    }


    void Update()
    {
      /*  if(Input.GetKeyDown(KeyCode.Space)  && !isMoving)
        {
            steps = Random.Range(1, 7);
            if (doneSteps + steps < fullRoute.Count)
            {
                StartCoroutine(Move());
            }
            else
            {
                Debug.Log("Number is to High");
            }
        }*/
    }
    void CreateFullRoute()
    {
        for(int i=0; i< commonRoute.childNodeList.Count-1; i++)
        {
            int tempPos = startNodeIndex + i;
            tempPos %= commonRoute.childNodeList.Count;

            fullRoute.Add(commonRoute.childNodeList[tempPos].GetComponent<Node>());
        }

        for (int i = 0; i < finalRoute.childNodeList.Count; i++)
        {
          
             fullRoute.Add(finalRoute.childNodeList[i].GetComponent<Node>());
        }
    }

    IEnumerator Move(int diceNumber)
    {
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;

        while(steps>0)
        {
            routePosition++;

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            while (MoveToNextNode(nextPos, 8f))
            {
                yield return null;
            
            }
            yield return new WaitForSeconds(0.1f);
            steps--;
            doneSteps++;
    
        }
        goalNode = fullRoute[routePosition];
        
        //check possible kick
        if(goalNode.isTaken)
        {
            
            if(goalNode.isSafeNode)
            {
                goalNode.isTaken = false;
            }
            //kick the other stone
            if (!(goalNode.stone.stoneId==this.stoneId))
            {
                goalNode.stone.ReturnToBase();
            }
          
        }

        currentNode.stone = null;
        currentNode.isTaken = false;

        goalNode.stone = this;
        if(!goalNode.isSafeNode)
        {
            goalNode.isTaken = true;
        }

        currentNode = goalNode;
        goalNode = null;

        //report to gamemanager
        //check for win condition
        if(WinCondition())
        {
            GameManager.instance.ReportWinning();
        }
        if(diceNumber<6)
        {
            GameManager.instance.state = GameManager.States.SwitchPlayer;
            //switch the player
        }
        else
        {
            GameManager.instance.state = GameManager.States.RollDice;
        }

        isMoving = false;
    }


    bool MoveToNextNode(Vector3 goalPos, float speed)
    {
        return goalPos != (transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime));
    }

   /* bool MoveInArcToNextPosistion(Vector3 startPos,Vector3 goalPos, float speed)
    {
        ctime += speed * Time.deltaTime;
        Vector3 myposition = Vector3.Lerp(startPos, goalPos, ctime);

        myposition.y += amplitude * Mathf.Sin(Mathf.Clamp01(ctime) * Mathf.PI);

        return goalPos != (transform.position = Vector3.Lerp(transform.position, myposition, ctime));

    }*/

    public bool ReturnIsOut()
    {
        return IsOut;
    }

    public void LeaveBase()
    {
        steps = 1;
        IsOut = true;
        routePosition = 0;
        StartCoroutine(MoveOut());
        //call corotinue to move the stone out
    }

    IEnumerator MoveOut()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while (steps > 0)
        {
           // routePosition++;

            Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
            while (MoveToNextNode(nextPos, 8f))
            {
                yield return null;

            }
            yield return new WaitForSeconds(0.1f);
            steps--;
            doneSteps++;

        }

        //update the node
        goalNode = fullRoute[routePosition];
        //check for kicking other stone
        if(goalNode.isTaken)
        {


            //return to base node
            goalNode.stone.ReturnToBase();
        }

        goalNode.stone = this;
        goalNode.isTaken = false;

        currentNode = goalNode;
        goalNode = null;

        //report back to gamemanager
        GameManager.instance.state = GameManager.States.RollDice;


       
         isMoving = false;
    }

    public bool CheckPossibleMove(int diceNumber)
    {
        
        int tempPos = routePosition + diceNumber;
        if(tempPos >= fullRoute.Count)
        {
            return false;
        }

        return !fullRoute[tempPos].isTaken;
    }

    public bool CheckPossibleKick(int stoneID, int diceNumber)
    {
       

        int tempPos = routePosition + diceNumber;
        if (tempPos >= fullRoute.Count)
        {
            return false;
        }
        if(fullRoute[tempPos].isTaken)
        {
            if (stoneID == fullRoute[tempPos].stone.stoneId)
            {
                //not a kickable stone
               
                return false;
            }
            return true;
        }
        return false;
    }

    public void StartTheMove(int diceNumber)
    {
        steps = diceNumber;
        StartCoroutine(Move(diceNumber));
    }

    public void ReturnToBase()
    {
        StartCoroutine(Return());
    }

    IEnumerator Return()
    {
        print("return to base");
        GameManager.instance.ReportTurnPossible(false);
        routePosition = 0;
        currentNode = null;
        goalNode = null;
        IsOut = false;
        doneSteps = 0;

        Vector3 baseNodePos = baseNode.gameObject.transform.position;
        while(MoveToNextNode(baseNodePos,100f))
        {
            yield return null;
        }
        GameManager.instance.ReportTurnPossible(true);
    }

    bool WinCondition()
    {
        for(int i =0; i < finalRoute.childNodeList.Count; i++)
        {
            if(!finalRoute.childNodeList[i].GetComponent<Node>().isTaken)
            {
                return false;
            }
        }
        return true;
    }

    public void SetSelector(bool on)
    {

        
        selector.SetActive(on);
        HasTurn = on; // check  for interactivity
    }

     void OnMouseDown()
    {
        if(HasTurn)
        {
            if(!IsOut)
            {
                LeaveBase();
            }
            else
            {
                StartTheMove(GameManager.instance.rolledhumanDice);
            }
            GameManager.instance.DeactivateSelectors();
        }
            
        
        
    }
}
