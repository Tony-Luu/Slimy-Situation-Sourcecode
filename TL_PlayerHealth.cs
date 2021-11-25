using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class TL_PlayerHealth : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Value for player's health")]
    private float MaxHealth;

    [SerializeField]
    [Tooltip("Value for player's max health")]
    private float CurrentHealth;

    [SerializeField]
    [Tooltip("UI for Health Bar")]
    private Image HealthBar;

    [SerializeField]
    [Tooltip("Script reference for the sprite manager")]
    private TL_SpriteManager SpriteManagerScript;

    [SerializeField]
    [Tooltip("Sound for slime death")]
    private AudioClip SlimeDeathSound;

    [SerializeField]
    [Tooltip("Audio source to play sounds")]
    private AudioSource SoundSource;

    [SerializeField]
    [Tooltip("Particle effect for the slime's death")]
    private ParticleSystem SlimeExplosion;



    void Start()
    {
        //Obtain the sprite manager
        SpriteManagerScript = GetComponent<TL_SpriteManager>();

        //Obtain the particle system
        SlimeExplosion = GetComponent<ParticleSystem>();

        //Obtain the audio source
        SoundSource = GetComponent<AudioSource>();

        //Find the camera and obtain the image
        HealthBar = GameObject.Find("Camera").GetComponentInChildren<Image>();

        //Set current health to max health as default
        CurrentHealth = MaxHealth;
    }

    //Returns the current health
    public float ReturnCurrentHealth()
    {
        return CurrentHealth;
    }

    void UpdateHealthBar()
    {
        //Divide the current health with the max health to get the percentage
        float HealthBarValue = CurrentHealth / MaxHealth;

        //Set the fill amount as the health bar value
        HealthBar.fillAmount = HealthBarValue;
    }

    void Update()
    {
        UpdateHealthBar();
    }

    //Play the explosion particles when the particles are not playing
    IEnumerator PlayExplosionParticles()
    {
        if (!SlimeExplosion.isPlaying)
        {
            SoundSource.clip = SlimeDeathSound;
            SoundSource.Play();
            SlimeExplosion.Play();
            yield return new WaitForSeconds(SlimeExplosion.main.duration);
            SlimeExplosion.Stop();
            SoundSource.Stop();
        }
    }

    void OnParticleSystemStopped()
    {
        //Destroy the gameobject
        Destroy(gameObject);

        //Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Sets the current health from the parameter
    public void SetCurrentHealth(float Health)
    {
        //Subtract current health with the value from the parameter
        CurrentHealth += Health;

        //If the current health is more than maximum health
        if (CurrentHealth > MaxHealth)
        {
            //Set the current health to max health to prevent current health from overflowing
            CurrentHealth = MaxHealth;
        }
        else if (CurrentHealth <= 0)       //If the current health is less than or equal to 0
        {
            //Set current health to 0
            CurrentHealth = 0;

            //Obtain the component and disable it
            GetComponent<Animator>().enabled = false;

            //Obtain the box collder
            BoxCollider2D PlayerCollider = GetComponent<BoxCollider2D>();

            //Disable the box collider
            PlayerCollider.enabled = false;

            //Obtain the slime trail properties script
            TL_SlimeTrailProperties SlimeTrailScript = GetComponent<TL_SlimeTrailProperties>();

            //Disable the slime trail script
            SlimeTrailScript.enabled = false;

            //Obtain the move character script
            TL_MoveCharacter MoveCharacterScript = GetComponent<TL_MoveCharacter>();

            //Disable the move character script
            MoveCharacterScript.enabled = false;

            //Set the sprite to null
            SpriteManagerScript.SetNewSprite(null);

            //Disable the sprite manager
            SpriteManagerScript.enabled = false;

            //Play the explosion particles
            StartCoroutine(PlayExplosionParticles());
        }
    }

}
