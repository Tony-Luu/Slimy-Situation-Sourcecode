using UnityEngine;

public class TL_FiniteStateMachine : MonoBehaviour
{
    private float AttackingDistance = 1f;
    private float MaximumDistance = 6f;
    private TL_AttackPlayer AttackPlayerScript;
    private TL_Pathfinder PathfinderScript;
    private TL_SpriteManager SpriteManagerScript;

    public enum CharacterState
    {
        Patrol, Chase, FollowTrail, Attack
    }

    [SerializeField]
    [Tooltip ("Enumerable variable for the current character state")]
    private CharacterState CurrentState = CharacterState.Patrol;

    [SerializeField]
    [Tooltip("Enumerable variable for the previous character state")]
    private CharacterState PreviousState;


    //Sets a new character state
    public void SetNewState(CharacterState NewState)
    {
        CurrentState = NewState;
    }

    //Return current state
    public CharacterState ReturnCurrentState()
    {
        return CurrentState;
    }

    void Start()
    {
        AttackPlayerScript = GetComponent<TL_AttackPlayer>();
        PathfinderScript = GetComponent<TL_Pathfinder>();
        SpriteManagerScript = GetComponent<TL_SpriteManager>();
        PreviousState = CurrentState;
    }

    void ResetPatrolState()
    {
        //Reset the lists for the pathfinding
        PathfinderScript.ResetLists();

        //Create a new current node
        Node CurrentNode = new Node();

        //Store the current position for the node
        CurrentNode.Position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);

        //Recalculate the path to the next waypoint
        PathfinderScript.FindPath(CurrentNode, PathfinderScript.ReturnNodeWaypoint());

        //Set new state to Patrol
        SetNewState(CharacterState.Patrol);
    }

    //Updates the previous state and resets the lists for the pathfinding
    void UpdatePreviousState()
    {
        //If the previous state is not the same as the current state
        if (PreviousState != CurrentState)
        {
            //Reset the lists for the pathfinding
            PathfinderScript.ResetLists();

            //Set the previous state as the current state
            PreviousState = CurrentState;
        }
    }

    void ManageStates()
    {
        //Find the player
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        //If the player is still alive
        if (Player != null)
        {
            //Obtain the player position
            Vector3 PlayerPosition = new Vector3(Mathf.RoundToInt(Player.transform.position.x), Mathf.RoundToInt(Player.transform.position.y), 0f);

            switch (CurrentState)
            {
                case CharacterState.Patrol:
                    //Make the character follow waypoints
                    PathfinderScript.FollowWaypoints();
                    break;

                case CharacterState.Chase:
                    //Update the previous state
                    UpdatePreviousState();

                    //Chase the target
                    PathfinderScript.ChaseTarget();

                    //If the character is outside maximum distance
                    if (Vector3.Distance(PlayerPosition, transform.position) > MaximumDistance)
                    {
                        //Reset the lists for the pathfinding
                        PathfinderScript.ResetLists();

                        //Create a new current node
                        Node CurrentNode = new Node();

                        //Store the current position for the node
                        CurrentNode.Position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);

                        //Recalculate the path to the next waypoint
                        PathfinderScript.FindPath(CurrentNode, PathfinderScript.ReturnNodeWaypoint());

                        //Set new state to Patrol
                        SetNewState(CharacterState.Patrol);
                    }

                    //If the character is within attacking distance
                    if (Vector3.Distance(Player.transform.position, transform.position) <= AttackingDistance)
                    {
                        //Round up to the nearest integer on the X and Y positions
                        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);

                        //Set both player and the character's positions
                        SpriteManagerScript.SetTwoPositions(PlayerPosition, transform.position);

                        //Set new state to Attack
                        SetNewState(CharacterState.Attack);

                        //Updates the previous states
                        UpdatePreviousState();
                    }
                    break;

                case CharacterState.Attack:
                    //Round up to the nearest integer on the X and Y positions
                    transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f);

                    //Set both player and the character's positions
                    SpriteManagerScript.SetTwoPositions(PlayerPosition, transform.position);

                    //Attack the player
                    AttackPlayerScript.AttackPlayer();

                    //If the character is outside attacking distance
                    if (Vector3.Distance(PlayerPosition, transform.position) > AttackingDistance)
                    {
                        //Set new state to Chase
                        SetNewState(CharacterState.Chase);

                        //Updates the previous states
                        UpdatePreviousState();
                    }
                    break;
            }
        }
    }

    void Update()
    {
        ManageStates();
    }

}
