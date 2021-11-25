using UnityEngine;

public class TL_AttackPlayer : MonoBehaviour
{
    [SerializeField]
    [Tooltip ("Integer value for how much damage this character does")]
    private float AttackDamage = 1f;

    [SerializeField]
    [Tooltip("Float value for how often the character can attack")]
    private float AttackInterval = 1f;

    [SerializeField]
    [Tooltip("Float value for stroing the cooldown of the character's attack")]
    private float AttackCooldown;

    [SerializeField]
    [Tooltip("Sound effect for attacking the player")]
    private AudioSource AttackSound;


    void Start()
    {
        AttackSound = GetComponent<AudioSource>();
    }

    //Make the character attack the player
    public void AttackPlayer()
    {
        //Find the player
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        //If the player is still alive
        if (Player != null)
        {
            //Obtain the script from the player
            TL_PlayerHealth PlayerHealthScript = Player.GetComponent<TL_PlayerHealth>();

            //If the attack cooldown is less than the real time start up
            if (AttackCooldown < Time.realtimeSinceStartup && PlayerHealthScript.ReturnCurrentHealth() > 0f)
            {
                //Play the attacking sound
                AttackSound.Play();

                //Subtract the player's health with the character's attack damage
                PlayerHealthScript.SetCurrentHealth(-AttackDamage);

                //Add the cooldown
                AttackCooldown = AttackInterval + Time.realtimeSinceStartup;
            }
        }
    }

}
