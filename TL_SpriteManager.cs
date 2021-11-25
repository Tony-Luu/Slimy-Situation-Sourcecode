using UnityEngine;

public class TL_SpriteManager : MonoBehaviour
{
    public Sprite WalkingUpSprite;
    public Sprite WalkingLeftSprite;
    public Sprite WalkingDownSprite;
    private SpriteRenderer OriginalSprite;
    private Vector3 FirstPosition;
    private Vector3 SecondPosition;


    void Start()
    {
        OriginalSprite = GetComponent<SpriteRenderer>();
    }

    //Sets a new sprite
    public void SetNewSprite(Sprite NewSprite)
    {
        OriginalSprite.sprite = NewSprite;
    }

    public void SetTwoPositions(Vector3 NewFirstPosition, Vector3 NewSecondPosition)
    {
        //Set the new first position
        FirstPosition = NewFirstPosition;

        //Set the new second position
        SecondPosition = NewSecondPosition;
    }

    void ChangeSpritesBasedOnDirection()
    {
        //Normalize the direction based on two positions
        Vector3 SpriteDirection = (FirstPosition - SecondPosition).normalized;

        //Change the facing of the sprite depending on the currentdirection
        if (SpriteDirection == Vector3.up)
        {
            OriginalSprite.sprite = WalkingUpSprite;
            OriginalSprite.flipX = false;
        }
        else if (SpriteDirection == Vector3.down)
        {
            OriginalSprite.sprite = WalkingDownSprite;
            OriginalSprite.flipX = false;
        }
        else if (SpriteDirection == Vector3.left)
        {
            OriginalSprite.sprite = WalkingLeftSprite;
            OriginalSprite.flipX = false;
        }
        else if (SpriteDirection == Vector3.right)
        {
            OriginalSprite.sprite = WalkingLeftSprite;
            OriginalSprite.flipX = true;
        }
    }

    void Update()
    {
        ChangeSpritesBasedOnDirection();
    }

}
