using UnityEngine;

public class TL_ExitLevel : MonoBehaviour
{
    public string NextSceneName;
    private TL_LoadScene LoadSceneScript;


    void Start()
    {
        LoadSceneScript = GetComponent<TL_LoadScene>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player touches the exit then load the next scene
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadSceneScript.LoadNextScene(NextSceneName);
        }
    }

}
