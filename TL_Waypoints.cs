using UnityEngine;

public class TL_Waypoints : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Target position for the character's destination")]
    private Vector3 TargetPosition;

    [SerializeField]
    [Tooltip("Next position for the character's future position")]
    private Vector3 NextPosition;

    [SerializeField]
    [Tooltip("")]
    private Vector3 DirectionToTarget;

    [SerializeField]
    [Tooltip("Value for the character's movement speed")]
    private float CharacterSpeed;

    [SerializeField]
    [Tooltip("Bool for checking if the character is moving or not")]
    private bool IsCharacterMoving;


    private TL_LevelManager LevelManagerScript;


    private TL_MoveCharacter MoveCharacterScript;

    //
    void Start()
    {
        LevelManagerScript = GameObject.Find("LevelArea").GetComponent<TL_LevelManager>();
        MoveCharacterScript = GameObject.FindGameObjectWithTag("Player").GetComponent<TL_MoveCharacter>();
        TargetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        NextPosition = TargetPosition;
    }

    //
    void FourDirectionalMovement()
    {
        //
        DirectionToTarget = TargetPosition - transform.position;

        //
        if (Mathf.Abs(DirectionToTarget.x) < Mathf.Abs(DirectionToTarget.y) && Mathf.Abs(DirectionToTarget.x) > Mathf.Epsilon)
        {
            DirectionToTarget.y = 0f;
        }
        else if (Mathf.Abs(DirectionToTarget.y) > Mathf.Epsilon)
        {
            DirectionToTarget.x = 0f;
        }
    }

    void Update()
    {
        //FollowWaypoints();
        //TargetPosition = MoveCharacterScript.ReturnPreviousPosition();
        //FourDirectionalMovement();
    }

    void AnimateCharacter()
    {
        //Make the player move towards the next position based on its' speed
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + DirectionToTarget, CharacterSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, NextPosition, CharacterSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        AnimateCharacter();
    }


}
