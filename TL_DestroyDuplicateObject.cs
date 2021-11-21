using UnityEngine;

public class TL_DestroyDuplicateObject : MonoBehaviour
{
    void Start()
    {
        //Don't destroy this gameobject on load
        DontDestroyOnLoad(this);

        //If there is more than 1 type of object that exists
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            //Destroy the duplicate
            Destroy(gameObject);
        }

    }

}
