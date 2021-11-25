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
    [Tooltip("Sound for slime moving")]
    private AudioClip SlimeMovingSound;

    [SerializeField]
    [Tooltip("Audio source to play sounds")]
    private AudioSource SoundSource;

    [SerializeField]
    [Tooltip("Script reference for the level manager script")]
    private TL_LevelManager LevelManagerScript;

    [SerializeField]
    [Tooltip("Script reference for the sprite manager script")]
    private TL_SpriteManager SpriteManagerScript;

    [SerializeField]
    private LayerMask IgnoreLayer;


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

        //Obtain the audio source
        SoundSource = GetComponent<AudioSource>();

        //Find the level area and obtain the script
        LevelManagerScript = GameObject.Find("LevelArea").GetComponent<TL_LevelManager>();

        //Obtain the sprite manager script
        SpriteManagerScript = GetComponent<TL_SpriteManager>();
    }

    //Uses a 2D box cast and checks in a direction if it collides with anything or not
    bool IsRaycastDetectingCollisionInNextDirection(Vector3 NextDirection)
    {
        //Set the distance for detection
        float DetectionDistance = 1f;

        //Set the box size of the box cast
        Vector2 BoxSize = new Vector2(0.95f, 0.95f);

        //Create a 2D box cast
        RaycastHit2D RaycastDetect2D = Physics2D.BoxCast(transform.position, BoxSize, 90f, NextDirection, DetectionDistance, ~IgnoreLayer);        

        //Return the result of the 2D box cast
        return RaycastDetect2D;
    }

    void MovePlayer()
    {
        //Checks and returns if the next position in the scene is obstructed or not
        bool IsUpPositionObstructed = IsRaycastDetectingCollisionInNextDirection(Vector2.up);
        bool IsDownPositionObstructed = IsRaycastDetectingCollisionInNextDirection(Vector2.down);
        bool IsLeftPositionObstructed = IsRaycastDetectingCollisionInNextDirection(Vector2.left);
        bool IsRightPositionObstructed = IsRaycastDetectingCollisionInNextDirection(Vector2.right);

        //If the player has not reached the next position and the player is not moving
        if (Vector3.Distance(transform.position, NextPosition) < Mathf.Epsilon && !IsCharacterMoving())
        {
            //Set the audio clip as the slime moving sound
            SoundSource.clip = SlimeMovingSound;

            //If the directional keys are pressed then set the next position depending on the key pressed and
            //the next position is not obstructed
            if (Input.GetKey(KeyCode.A) && !IsLeftPositionObstructed)
            {
                //If the next position does not go out of array bounds
                if (NextPosition.x - 1f >= 0f)
                {
                    SoundSource.Play();
                    NextPosition = new Vector3(NextPosition.x - 1f, NextPosition.y, NextPosition.z);
                    IsPlayerMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.D) && !IsRightPositionObstructed)
            {
                //If the next position does not go out of array bounds
                if (NextPosition.x + 1f < LevelManagerScript.ReturnLevelAreaArray().GetLength(0))
                {
                    SoundSource.Play();
                    NextPosition = new Vector3(NextPosition.x + 1f, NextPosition.y, NextPosition.z);
                    IsPlayerMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.S) && !IsDownPositionObstructed)
            {
                //If the next position does not go out of array bounds
                if (NextPosition.y - 1f >= 0f)
                {
                    SoundSource.Play();
                    NextPosition = new Vector3(NextPosition.x, NextPosition.y - 1f, NextPosition.z);
                    IsPlayerMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.W) && !IsUpPositionObstructed)
            {
                //If the next position does not go out of array bounds
                if (NextPosition.y + 1f < LevelManagerScript.ReturnLevelAreaArray().GetLength(1))
                {
                    SoundSource.Play();
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
