using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
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


     void Start()
    {
        startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
        CreateFullRoute();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)  && !isMoving)
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
        }
    }
    void CreateFullRoute()
    {
        for(int i=0; i< commonRoute.childNodeList.Count-1; i++)
        {
            int tempPos = startNodeIndex + i;
            tempPos %= commonRoute.childNodeList.Count;

            fullRoute.Add(commonRoute.childNodeList[tempPos].GetComponent<Node>());
        }

        for (int i = 0; i < finalRoute.childNodeList.Count-1; i++)
        {
          
             fullRoute.Add(finalRoute.childNodeList[i].GetComponent<Node>());
        }
    }

    IEnumerator Move()
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
        isMoving = false;
    }


    bool MoveToNextNode(Vector3 goalPos, float speed)
    {
        return goalPos != (transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime));
    }
}
