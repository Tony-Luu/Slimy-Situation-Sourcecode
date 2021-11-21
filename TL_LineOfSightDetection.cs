using System.Collections.Generic;
using UnityEngine;

public class TL_LineOfSightDetection : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of 2D colliders to detect which collider is the closest")]
    private List<Collider2D> DetectedColliders = new List<Collider2D>();

    [SerializeField]
    [Tooltip("Stores the closest 2D collider")]
    private Collider2D Closest2DCollider;

    [SerializeField]
    [Tooltip("Script reference for the finite state machine")]
    private TL_FiniteStateMachine FiniteStateMachineScript;

    [SerializeField]
    [Tooltip("Script reference for the pathfinder")]
    private TL_Pathfinder PathfinderScript;

    [SerializeField]
    [Tooltip("Float value for offsetting the collider")]
    private float ColliderOffset = 5f;


    void Start()
    {
        FiniteStateMachineScript = transform.parent.GetComponent<TL_FiniteStateMachine>();
        PathfinderScript = transform.parent.GetComponent<TL_Pathfinder>();
    }

    void ChangeLineOfSightDirection()
    {
        if (PathfinderScript.CompletePath.Count > 0)
        {
            Vector3 Direction = (PathfinderScript.CompletePath[PathfinderScript.ReturnPathIndex()].Position - transform.parent.position).normalized;

            if (Direction == Vector3.right)
            {
                transform.localPosition = Vector3.right * ColliderOffset;
                transform.localEulerAngles = Vector3.zero;
            }
            else if (Direction == Vector3.left)
            {
                transform.localPosition = Vector3.left;
                transform.localEulerAngles = Vector3.zero;
            }
            else if (Direction == Vector3.up)
            {
                transform.localPosition = Vector3.up * ColliderOffset;
                transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (Direction == Vector3.down)
            {                
                transform.localPosition = Vector3.down;
                transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            }
        }
    }

    void Update()
    {
        ChangeLineOfSightDirection();
        CalculateNearestCollider(DetectedColliders);
    }

    Collider2D CalculateNearestCollider(List<Collider2D> ListColliders)
    {
        //Set default values
        Closest2DCollider = null;

        //Set closest distance to infinity as default
        float ClosestDistance = Mathf.Infinity;

        //Obtain the current position from the parent
        Vector3 CurrentPos = transform.parent.position;

        //Loop through the list of colliders
        foreach (Collider2D Col in ListColliders)
        {
            //Calculate the direction from the collider's position with the current position
            Vector3 Direction = Col.transform.position - CurrentPos;

            //Square the direction
            float DistanceSquared = Direction.sqrMagnitude;

            //If the distance squared is less than the closest distance
            if (DistanceSquared < ClosestDistance)
            {
                //Set the closest distance to distance squared
                ClosestDistance = DistanceSquared;

                //Set the closest collider from the list of colliders
                Closest2DCollider = Col;
            }
        }

        //If the detected collider still exists
        if (Closest2DCollider != null)
        {
            //If the closest collider is the player
            if (Closest2DCollider.tag == "Player")
            {
                //Set new state to Chase
                FiniteStateMachineScript.SetNewState(TL_FiniteStateMachine.CharacterState.Chase);
            }
        }
        //Return the closest collider
        return Closest2DCollider;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (DetectedColliders.Find(x => x == collision) == null)
        {
            if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Wall"))
            {
                DetectedColliders.Add(collision);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        DetectedColliders.Remove(collision);
    }

}
