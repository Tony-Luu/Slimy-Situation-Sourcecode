using UnityEngine;

public class TL_LineOfSightDetection : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Vector3 variable for calculating the direction")]
    private Vector3 Direction;

    [SerializeField]
    [Tooltip("Collider 2D variable to store the detected collider")]
    private Collider2D DetectedCollider;

    [SerializeField]
    [Tooltip("Script reference for the pathfinder")]
    private TL_Pathfinder PathfinderScript;

    [SerializeField]
    [Tooltip("Script reference for the finite state machine")]
    private TL_FiniteStateMachine FiniteStateMachineScript;


    void Start()
    {
        FiniteStateMachineScript = transform.parent.GetComponent<TL_FiniteStateMachine>();
        PathfinderScript = transform.parent.GetComponent<TL_Pathfinder>();
    }

    void CalculateDirection()
    {
        //Find the player
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        //If the player is still alive
        if (Player != null)
        {
            //Calculate the direction between itself and the Player
            Direction = Player.transform.position - transform.position;
        }
    }

    //Change the line of sight detection based on the next direction of the character
    void ChangeLineOfSightDirection()
    {
        //If the complete path list has elements
        if (PathfinderScript.CompletePath.Count > 0)
        {
            //Calculate the direction of the character is moving in
            Vector3 Direction = (PathfinderScript.CompletePath[PathfinderScript.ReturnPathIndex()].Position - transform.parent.position).normalized;

            //Rotate the collider respectively of the character's direction
            if (Direction == Vector3.right)
            {
                transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (Direction == Vector3.left)
            {
                transform.localEulerAngles = new Vector3(0f, 0f, -90f);
            }
            else if (Direction == Vector3.up)
            {
                transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (Direction == Vector3.down)
            {
                transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
    }

    void Update()
    {
        CalculateDirection();
        ChangeLineOfSightDirection();
    }

    Collider2D ReturnClosestCollider(RaycastHit2D[] RaycastHitArray)
    {
        //Set collider 2D variable to null as default
        Collider2D ClosestCollider = null;

        //Set closest distance to infinity as default
        float ClosestDistance = Mathf.Infinity;

        //Loop through the raycast hit 2D array
        foreach (RaycastHit2D RayHit in RaycastHitArray)
        {
            //Calculate the direction from the collider's position with the current position
            Vector3 Direction = RayHit.collider.transform.position - transform.position;

            //Square the direction
            float DistSquared = Direction.sqrMagnitude;

            //If the distance squared is less than the closest distance
            if (DistSquared < ClosestDistance)
            {
                //Set the closest distance to distance squared
                ClosestDistance = DistSquared;

                //Set the closest collider from the list of colliders
                ClosestCollider = RayHit.collider;
            }
        }
        return ClosestCollider;
    }

    void RaycastDetection()
    {
        //If the detected collider still exists
        if (DetectedCollider != null)
        {
            //Detect and return all of the 2D colliders by the raycast hit array
            RaycastHit2D[] RaycastCollide2D = Physics2D.RaycastAll(transform.position, Direction, 4f);

            //Calculate and return the closest 2D collider from the raycast hit array
            Collider2D ReturnedCollider2D = ReturnClosestCollider(RaycastCollide2D);

            Debug.DrawRay(transform.position, Direction, Color.red);

            //If the returned collider is the player and the character isn't attack
            if (ReturnedCollider2D != null && ReturnedCollider2D.CompareTag("Player") &&
                FiniteStateMachineScript.ReturnCurrentState() != TL_FiniteStateMachine.CharacterState.Attack)
            {
                //Set new state to Chase
                FiniteStateMachineScript.SetNewState(TL_FiniteStateMachine.CharacterState.Chase);
            }
        }
    }

    void FixedUpdate()
    {
        RaycastDetection();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DetectedCollider = collision;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DetectedCollider = null;
        }
    }

}
