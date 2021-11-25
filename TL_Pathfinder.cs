using System.Collections.Generic;
using UnityEngine;

public class TL_Pathfinder : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Vector3 List for storing waypoints")]
    private List<Vector3> Waypoints = new List<Vector3>();

    [SerializeField]
    [Tooltip("Index for the list of waypoints")]
    private int WaypointIndex;

    [SerializeField]
    [Tooltip("List of nodes for the completed path")]
    public List<Node> CompletePath = new List<Node>();

    [SerializeField]
    [Tooltip("List of nodes that need to be evaluated from the open list")]
    private List<Node> OpenList = new List<Node>();

    [SerializeField]
    [Tooltip("List of nodes that are already evaluated in the closed list")]
    private List<Node> ClosedList = new List<Node>();

    [SerializeField]
    [Tooltip("Index for the completed path")]
    private int PathIndex;

    [SerializeField]
    [Tooltip("How fast is the character moving")]
    private float Speed;

    [SerializeField]
    [Tooltip("Node for the current position")]
    private Node CurrentNode = new Node();

    [SerializeField]
    [Tooltip("Node waypoint for following waypoints")]
    private Node NodeWaypoint = new Node();

    [SerializeField]
    [Tooltip("Script reference for the level manager")]
    private TL_LevelManager LevelManagerScript;

    [SerializeField]
    [Tooltip("Script reference for the sprite manager")]
    private TL_SpriteManager SpriteManagerScript;

    [SerializeField]
    [Tooltip("Script reference for the finite state machine")]
    private TL_FiniteStateMachine FiniteStateMachineScript;


    void Start()
    {
        LevelManagerScript = GameObject.Find("LevelArea").GetComponent<TL_LevelManager>();
        SpriteManagerScript = GetComponent<TL_SpriteManager>();
        FiniteStateMachineScript = GetComponent<TL_FiniteStateMachine>();
    }

    //Sets the current speed
    public void SetSpeed(float NewSpeedValue)
    {
        Speed = NewSpeedValue;
    }

    //Returns the current path index
    public int ReturnPathIndex()
    {
        return PathIndex;
    }

    //Return the current waypoint
    public Node ReturnNodeWaypoint()
    {
        return NodeWaypoint;
    }

    //Make the character chase the player
    public void ChaseTarget()
    {
        //Find the player's position
        Vector3 PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        //Set the current node's position to the character's position
        CurrentNode.Position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);

        //If the target position does not have a position
        if (CompletePath.Count == 0)
        {
            //Create a new target node
            Node TargetNode = new Node();

            //Set the target node's position to the player's position
            TargetNode.Position = new Vector3(Mathf.RoundToInt(PlayerPosition.x), Mathf.RoundToInt(PlayerPosition.y), 0f);

            //Find a new path towards the target
            FindPath(CurrentNode, TargetNode);
        }
        else if(CurrentNode.Position == CompletePath[CompletePath.Count - 1].Position)
        {
            //Set the current node's position to the character's position
            CurrentNode.Position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);

            //Reset the lists used for the pathfinding
            ResetLists();
        }
    }

    //Recursive function to search for all of the parent nodes
    void BacktrackPathFromParentNodes(Node Node)
    {
        //If the node does have a parent node
        if (Node.ParentNode != null)
        {
            //Add the parent node into the list
            CompletePath.Add(Node.ParentNode);

            //Recall this function again with the parent node
            BacktrackPathFromParentNodes(Node.ParentNode);
        }
        else
        {
            //Reverse the list
            CompletePath.Reverse();
        }
    }

    public void FindPath(Node CurrentNode, Node TargetNode)
    {
        //Set the current node's position to the player's position
        CurrentNode.Position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);

        //Add current node into open list to initialize the while loop
        OpenList.Add(CurrentNode);

        //While the open list is not empty
        while (OpenList.Count > 0)
        {
            //Find and return the lowest cost node
            CurrentNode = ReturnLowestCostNode();

            //Search for the old current node's position
            Node OldCurrentNode = OpenList.Find(x => x.Position == CurrentNode.Position);

            //Remove the current node from the open list
            OpenList.Remove(OldCurrentNode);

            //Add the current node to the closed list
            ClosedList.Add(CurrentNode);

            //If the current node is the target node then break the loop
            if (CurrentNode.Position == TargetNode.Position)
            {
                //Empty the open list
                OpenList.Clear();

                //Set the target node to the current node with the target node's position
                TargetNode = CurrentNode;

                //Add the target node into the complete path list
                CompletePath.Add(TargetNode);

                //Find and add all of the parent nodes starting from the target node
                BacktrackPathFromParentNodes(TargetNode);

                //Break out of the while loop
                break;
            }

            //Search for all of the current neighbours
            for (int x = (int)CurrentNode.Position.x - 1; x < (int)CurrentNode.Position.x + 2; x++)
            {
                for (int y = (int)CurrentNode.Position.y - 1; y < (int)CurrentNode.Position.y + 2; y++)
                {
                    if (x == CurrentNode.Position.x - 1 && y == CurrentNode.Position.y - 1 ||
                        x == CurrentNode.Position.x + 1 && y == CurrentNode.Position.y - 1 ||
                        x == CurrentNode.Position.x - 1 && y == CurrentNode.Position.y + 1 ||
                        x == CurrentNode.Position.x + 1 && y == CurrentNode.Position.y + 1)
                    {
                        //Skip all of the diagonals
                        continue;
                    }
                    else if (x == CurrentNode.Position.x && y == CurrentNode.Position.y)
                    {
                        //Skip the current position of the node
                        continue;
                    }
                    else if (x < 0 || y < 0 || x > LevelManagerScript.ReturnLevelAreaArray().GetLength(0) - 1 ||
                             y > LevelManagerScript.ReturnLevelAreaArray().GetLength(1) - 1)
                    {
                        //Skip array out of bounds
                        continue;
                    }
                    else if (LevelManagerScript.ReturnGameObjectFromPosition(x, y) != null)
                    {
                        //Skip all obstacles in the area
                        continue;
                    }
                    //If the adjacent space in the level area is empty
                    else if (LevelManagerScript.ReturnGameObjectFromPosition(x, y) == null)
                    {
                        //Create new neighbour node object
                        Node NeighbourNode = new Node();

                        //Set the G cost of the neighbouring node
                        NeighbourNode.gCost = 1;

                        //Set the position and the costs of the neighbour node
                        NeighbourNode.Position = new Vector3(x, y, 0f);

                        //Calculate the new G cost
                        int NewGCost = CurrentNode.gCost + NeighbourNode.gCost;

                        //Search for the adjacent node's position in the open list
                        Node SearchedOpenNode = OpenList.Find(x => x.Position == NeighbourNode.Position);

                        //Search for the adjacent node's position in the closed list
                        Node SearchedClosedNode = ClosedList.Find(x => x.Position == NeighbourNode.Position);

                        //If the neighbour node is in the closed list then skip to the next neighbour
                        if (ClosedList.Contains(SearchedClosedNode))
                        {
                            continue;
                        }
                        //If the open list contains the neighbouring node
                        else if (OpenList.Contains(SearchedOpenNode))
                        {
                            //If the new G cost is lower than the neighbouring node's G cost
                            if (NewGCost < SearchedOpenNode.gCost)
                            {
                                //Update the node found in the open list with the new G and H cost as well as the parent node
                                SearchedOpenNode.hCost = CalculateManhattanDistance(NeighbourNode, TargetNode);
                                SearchedOpenNode.gCost = NewGCost;
                                SearchedOpenNode.ParentNode = NeighbourNode.ParentNode;
                            }
                        }
                        //If the open list does not contain the neighbouring node
                        else if (!OpenList.Contains(SearchedOpenNode))
                        {
                            //Calculate the H cost of the neighbouring node
                            NeighbourNode.hCost = CalculateManhattanDistance(NeighbourNode, TargetNode);

                            //Set the new G cost to the neighbour node
                            NeighbourNode.gCost = NewGCost;

                            //Set the parent node of the neighbour node to the current node
                            NeighbourNode.ParentNode = CurrentNode;

                            //Add it to the open list
                            OpenList.Add(NeighbourNode);
                        }
                    }
                }
            }
        }
    }

    Node ReturnLowestCostNode()
    {
        //Set lowest cost node as default to the first element in the open list
        Node LowestCostNode = OpenList[0];

        //Set value to infinity to search for the lowest cost node
        float LowestFCost = Mathf.Infinity;

        //Loop through the open list
        foreach (Node OpenNode in OpenList)
        {
            //Calculate the F cost of the node in the open list
            OpenNode.fCost = OpenNode.gCost + (int)OpenNode.hCost;

            //If the node's F cost is lower than the lowest F cost
            if (OpenNode.fCost < LowestFCost)
            {
                //Set the lowest F cost to the node's F cost
                LowestFCost = OpenNode.fCost;

                //Set the lowest cost node to the node from the open list
                LowestCostNode = OpenNode;
            }
        }
        //Return the new current node
        return LowestCostNode;
    }

    float CalculateManhattanDistance(Node CurrentNode, Node TargetNode)
    {
        //Find the absolute values of the X and Z positions of the nodes
        float dX = Mathf.Abs(CurrentNode.Position.x - TargetNode.Position.x);
        float dY = Mathf.Abs(CurrentNode.Position.y - TargetNode.Position.y);
        
        //Return the manhattan distance
        return dX + dY;
    }
    
    public void ResetLists()
    {
        //Resets the lists for the A* pathfinding
        CompletePath.Clear();
        ClosedList.Clear();

        //Reset path index for the pathfinding
        PathIndex = 0;
    }

    //Make the character follow the waypoints
    public void FollowWaypoints()
    {
        //If the distance between itself and the next waypoints are within distance of each other
        if (Vector3.Distance(transform.position, Waypoints[WaypointIndex]) < Mathf.Epsilon)
        {
            //Increment the index
            WaypointIndex++;

            //If the index for the waypoint is more than or equal to the maximum length of the waypoint list
            if (WaypointIndex > Waypoints.Count - 1)
            {
                //Reset the index
                WaypointIndex = 0;
            }

            //Resets the lists used for the A* pathfinding
            ResetLists();

            //Set the new current position for the current node
            CurrentNode.Position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);

            //Set the new next position in the waypoints list for the node waypoint
            NodeWaypoint.Position = Waypoints[WaypointIndex];

            //Find the brand new path
            FindPath(CurrentNode, NodeWaypoint);
        }
    }

    //Sends two positions to the sprite manager script to calculate the current directions
    void ManagePositionsForDirections()
    {
        //If the list has elements
        if (CompletePath.Count > 0 && FiniteStateMachineScript.ReturnCurrentState() != TL_FiniteStateMachine.CharacterState.Attack)
        {
            //Input two positions for calculating the direction of the character
            SpriteManagerScript.SetTwoPositions(CompletePath[PathIndex].Position, transform.position);
        }
    }

    void Update()
    {
        AnimateCharacter();
        ManagePositionsForDirections();
    }

    //Moving the player to the target position
    void AnimateCharacter()
    {
        //If there are elements in the complete path list
        if (CompletePath.Count > 0)
        {
            //If the player approaches the next position in the list
            if (Vector3.Distance(CompletePath[PathIndex].Position, transform.position) > Mathf.Epsilon)
            {
                //Move towards the next position in the list
                transform.position = Vector3.MoveTowards(transform.position, CompletePath[PathIndex].Position, Speed * Time.deltaTime);
            }
            else
            {
                //If the path index has not reached the maximum length of the list
                if (PathIndex < CompletePath.Count - 1)
                {
                    //Increment the path index
                    PathIndex++;
                }
                else
                {
                    //Set the transform position to the last position in the list
                    transform.position = CompletePath[PathIndex].Position;
                }
            }            
        }
    }

}
