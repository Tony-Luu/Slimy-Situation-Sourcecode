using UnityEngine;

public class TL_MoveCharacter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Vector3 variable to store the next position when the player presses a directional key")]
    private Vector3 NextPosition;

    [SerializeField]
    [Tooltip("Float value for the player's movement speed")]
    private float Speed;

    [SerializeField]
    [Tooltip("Boolean to check whenever the player is moving or not")]
    private bool IsPlayerMoving;

    [SerializeField]
    [Tooltip("Script reference for the level manager script")]
    private TL_LevelManager LevelManagerScript;

    [SerializeField]
    [Tooltip("Script reference for the sprite manager script")]
    private TL_SpriteManager SpriteManagerScript;


    //Returns the next position
    public Vector3 ReturnNextPosition()
    {
        return NextPosition;
    }

    //Check and return if the player is moving or not
    public bool IsCharacterMoving()
    {
        return IsPlayerMoving;
    }

    void Start()
    {
        //Set the next position to the transform position as default
        NextPosition = transform.position;

        //Find the level area and obtain the script
        LevelManagerScript = GameObject.Find("LevelArea").GetComponent<TL_LevelManager>();

        //Obtain the sprite manager script
        SpriteManagerScript = GetComponent<TL_SpriteManager>();
    }

    void MovePlayer()
    {
        //If the player has not reached the next position and the player is not moving
        if (Vector3.Distance(transform.position, NextPosition) < 0.1f && !IsCharacterMoving())
        {
            //If the directional keys are pressed then set the next position depending on the key pressed and set the bool to true
            if (Input.GetKey(KeyCode.A))
            {
                //If the next position does not go out of array bounds and the next position is null in the gameobject array
                if (NextPosition.x - 1f >= 0f && LevelManagerScript.ReturnGameObjectFromPosition((int)(NextPosition.x - 1f), (int)NextPosition.y) == null)
                {
                    NextPosition = new Vector3(NextPosition.x - 1f, NextPosition.y, NextPosition.z);
                    IsPlayerMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //If the next position does not go out of array bounds and the next position is null in the gameobject array
                if (NextPosition.x + 1f < LevelManagerScript.ReturnLevelAreaArray().GetLength(0) && 
                    LevelManagerScript.ReturnGameObjectFromPosition((int)(NextPosition.x + 1f), (int)NextPosition.y) == null)
                {
                    NextPosition = new Vector3(NextPosition.x + 1f, NextPosition.y, NextPosition.z);
                    IsPlayerMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                //If the next position does not go out of array bounds and the next position is null in the gameobject array
                if (NextPosition.y - 1f >= 0f && LevelManagerScript.ReturnGameObjectFromPosition((int)(NextPosition.x), (int)(NextPosition.y - 1f)) == null)
                {
                    NextPosition = new Vector3(NextPosition.x, NextPosition.y - 1f, NextPosition.z);
                    IsPlayerMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                //If the next position does not go out of array bounds and the next position is null in the gameobject array
                if (NextPosition.y + 1f < LevelManagerScript.ReturnLevelAreaArray().GetLength(1) && 
                    LevelManagerScript.ReturnGameObjectFromPosition((int)(NextPosition.x), (int)(NextPosition.y + 1f)) == null)
                {
                    NextPosition = new Vector3(NextPosition.x, NextPosition.y + 1f, NextPosition.z);
                    IsPlayerMoving = true;
                }                
            }
        }
        else
        {
            //Set the bool to false when the player is not moving
            IsPlayerMoving = false;
        }
    }

    void AnimatePlayer()
    {
        //Make the player move towards the next position based on its' speed
        transform.position = Vector3.MoveTowards(transform.position, NextPosition, Speed * Time.deltaTime);
    }

    void Update()
    {
        MovePlayer();
        SpriteManagerScript.SetTwoPositions(NextPosition, new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0f));
    }

    void FixedUpdate()
    {
        AnimatePlayer();
    }
}
