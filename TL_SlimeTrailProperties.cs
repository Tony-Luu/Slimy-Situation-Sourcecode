using UnityEngine;

public class TL_SlimeTrailProperties : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Gameobject to leave behind slime trails when the player moves")]
    private GameObject SlimeTrail;

    [SerializeField]
    [Tooltip("Script reference for moving the player")]
    private TL_MoveCharacter MoveCharacterScript;

    [SerializeField]
    [Tooltip("Script reference for player health")]
    private TL_PlayerHealth PlayerHealthScript;


    void Start()
    {
        MoveCharacterScript = GetComponent<TL_MoveCharacter>();
        PlayerHealthScript = GetComponent<TL_PlayerHealth>();
    }

    //Does the terrain tile exist on the next position?
    bool IsTerrainTileOnNextPosition(string TerrainTileTag)
    {
        //Find all of the terrain tiles in the scene
        GameObject[] TerrainTiles = GameObject.FindGameObjectsWithTag(TerrainTileTag);

        //Loop through all of the terrain tiles
        foreach (GameObject Trails in TerrainTiles)
        {
            //If a terrain trail exists on the next position
            if (Trails.transform.position == MoveCharacterScript.ReturnNextPosition())
            {
                //Return true
                return true;
            }
        }
        //Otherwise return false
        return false;
    }

    //Creates slime trails from previous position when the player moves
    void CreateSlimeTrail()
    {
        //If the player is moving and the next position does not have a smile trail
        if (MoveCharacterScript.IsCharacterMoving() && !IsTerrainTileOnNextPosition("Trail"))
        {
            //Create a slime trail
            Instantiate(SlimeTrail, new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0f), Quaternion.identity);
        }
    }

    //Reduce health when the player moves and their next position is not on a slime trail
    void ReduceHealth()
    {
        //If the player is moving
        if (MoveCharacterScript.IsCharacterMoving())
        {
            //If the slime trail is not on the next position or if the puddle is on the next position
            if (!IsTerrainTileOnNextPosition("Trail") && !IsTerrainTileOnNextPosition("Puddle"))
            {
                PlayerHealthScript.SetCurrentHealth(-1f);
            }
        }
    }

    void Update()
    {
        ReduceHealth();
        CreateSlimeTrail();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        //If the player touches the slime trail
        if (collider.gameObject.CompareTag("Trail") && Vector3.Distance(transform.position, collider.transform.position) < 0.25f)
        {
            //Increase current health by 1
            PlayerHealthScript.SetCurrentHealth(1f);

            //Destroy the slime trail
            Destroy(collider.gameObject);
        }
    }

}
