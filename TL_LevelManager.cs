using UnityEngine.SceneManagement;
using UnityEngine;

public class TL_LevelManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("2D gameobject array for the level area to check for collisions")]
    private GameObject[,] LevelArea = new GameObject[9, 9];

    [SerializeField]
    [Tooltip("2D level layout of the area")]
    private int[,] LevelLayout;

    [SerializeField]
    [Tooltip("Player Character")]
    private GameObject Slime;

    [SerializeField]
    [Tooltip("Adventurer NPC")]
    private GameObject Adventurer;

    [SerializeField]
    [Tooltip("Rock gameobject")]
    private GameObject Rock;

    [SerializeField]
    [Tooltip("Rock 2 gameobject")]
    private GameObject Rock2;

    [SerializeField]
    [Tooltip("Cave Floor gameobject")]
    private GameObject CaveFloor;

    [SerializeField]
    [Tooltip("Cave Floor 2 gameobject")]
    private GameObject CaveFloor2;

    [SerializeField]
    [Tooltip("Cave Floor 3 gameobject")]
    private GameObject CaveFloor3;

    [SerializeField]
    [Tooltip("Puddle")]
    private GameObject Puddle;

    [SerializeField]
    [Tooltip("Contains the current scene")]
    private Scene CurrentScene;

    [SerializeField]
    [Tooltip("Contains the previous scene")]
    private Scene PreviousScene;


    //Returns level area array
    public GameObject[,] ReturnLevelAreaArray()
    {
        return LevelArea;
    }

    //Returns a gameobject from the level area array based on X and Y positions
    public GameObject ReturnGameObjectFromPosition(int x, int y)
    {
        return LevelArea[x, y];
    }

    //Set a gameobject in the level area
    public void SetGameObjectInLevelArea(int x, int y, GameObject Object)
    {
        LevelArea[x, y] = Object;
    }

    //Generate objects in the scene based on the integer in the integer 2D array
    void GenerateObjectsInScene(int[,] GridLayout)
    {
        for (int x = 0; x < GridLayout.GetLength(0); x++)
        {
            for (int y = 0; y < GridLayout.GetLength(1); y++)
            {
                switch (GridLayout[x, y])
                {
                    case -1:
                        Instantiate(Puddle, new Vector3(x, y, 0f), Quaternion.identity);
                        break;

                    case 0:
                        LevelArea[x, y] = null;
                        break;

                    case 1:
                        LevelArea[x, y] = Instantiate(Rock, new Vector3(x, y, 0f), Quaternion.identity);
                        break;

                    case 2:
                        LevelArea[x, y] = Instantiate(Rock2, new Vector3(x, y, 0f), Quaternion.identity);
                        break;

                    case 3:
                        Instantiate(CaveFloor, new Vector3(x, y, 0f), Quaternion.identity);
                        break;

                    case 4:
                        Instantiate(CaveFloor2, new Vector3(x, y, 0f), Quaternion.identity);
                        break;

                    case 5:
                        Instantiate(CaveFloor3, new Vector3(x, y, 0f), Quaternion.identity);
                        break;

                    case 6:
                        Instantiate(Slime, new Vector3(x, y, 0f), Quaternion.identity);
                        break;

                    case 7:
                        Instantiate(Adventurer, new Vector3(x, y, 0f), Quaternion.identity);
                        break;
                }
            }
        }
    }

    void Awake()
    {
        //Obtain the current scene
        CurrentScene = SceneManager.GetActiveScene();
    }

    void InitializeTutorialLevel()
    {
        //Top-left corner is X = 0 and Y = 0 in the scene
        LevelLayout = new int[,] 
        {
            { 1, 1, 1, 0, 6, 4, 1, 1, 2 },
            { 1, 1, 5, 0, 0, 0, 5, 1, 2 },
            { 1, 0, 0, 0, 3, 0, 0, 0, 2 },
            { 0, 0, 4, -1, 0, 0, 0, 5, 0 },
            { 3, 0, 0, 2, 2, -1, 1, 0, 3 },
            { 0, 5, -1, 0, 5, 0, 0, 0, 4 },
            { 0, -1, 1, 4, 0, -1, 2, 4, 0 },
            { 0, -1, 1, 0, 0, 3, 0, 0, 3 },
            { 5, 0, 3, 0, 4, 5, 0, 0, 7 },
        };
        GenerateObjectsInScene(LevelLayout);
    }

    void InitializeLevel1()
    {
        LevelLayout = new int[,]
        {
            { 5, 1, 1, 1, 0, 0, 5, 0, 6 },
            { 0, 4, 0, 0, 4, 0, -1, 2, 5 },
            { 0, 0, 0, -1, 0, 3, 0, 2, 0 },
            { 1, 0, 3, 2, 1, 0, 0, -1, 0 },
            { 2, 0, 0, 2, 2, 1, -1, 1, 0 },
            { -1, 0, -1, 1, 2, 0, 4, 2, 0 },
            { 2, -1, 0, 3, 0, -1, 0, 2, -1 },
            { 1, 4, 0, 0, 5, 0, 0, 0, 0 },
            { 0, 0, 5, -1, 2, 4, -1, 1, 5 },
        };
        GenerateObjectsInScene(LevelLayout);
    }

    void InitializeLevel2()
    {
        LevelLayout = new int[,]
        {
            { 1, 1, 2, 5, -1, 1, 2, 1, 1 },
            { 2, 0, 4, 0, 0, 0, 0, 0, 3 },
            { 2, 0, 1, -1, 2, 2, -1, 0, 0 },
            { 0, 3, -1, 0, 5, 1, 2, 1, -1 },
            { 0, -1, 1, 1, 6, 0, 0, 0, 4 },
            { 1, 5, 0, -1, 1, 2, -1, 2, 0 },
            { 1, 1, 2, 0, 0, 0, 3, 1, 0 },
            { 0, 3, 2, 1, 4, 0, 0, 0, 0 },
            { 5, -1, 0, 0, -1, 2, 1, 1, 5 },
        };
        GenerateObjectsInScene(LevelLayout);
    }

    void InitializeLevel3()
    {
        LevelLayout = new int[,]
        {
            { 2, 5, 0, 4, 2, 1, 1, 2, 2 },
            { 4, -1, 1, -1, 3, 0, 0, 6, 1 },
            { 5, 1, 5, 0, 0, -1, 5, 1, 2 },
            { 3, 0, -1, 2, 0, 2, 1, 5, 3 },
            { 1, 5, 1, 1, -1, 4, 0, 0, 0 },
            { 1, 0, -1, 2, 1, -1, 2, 2, 0 },
            { -1, 4, 0, 2, 5, 0, 5, 0, 4 },
            { 0, 2, 3, 0, -1, 1, 5, -1, 1 },
            { 5, 1, 0, 4, 1, 2, 1, 4, 2 },
        };
        GenerateObjectsInScene(LevelLayout);
    }

    //Load the level based on scene name
    void LoadLevelRespectiveOfSceneName()
    {
        //Obtain the active scene
        CurrentScene = SceneManager.GetActiveScene();

        //If the previous scene is not the same as the current scene
        if (PreviousScene != CurrentScene)
        {
            //If the scene name corresponds with the loaded level, initialize the level based on the scene
            if (CurrentScene.name == "TutorialLevel")
            {
                InitializeTutorialLevel();
            }
            else if (CurrentScene.name == "Level1")
            {
                InitializeLevel1();
            }
            else if (CurrentScene.name == "Level2")
            {
                InitializeLevel2();
            }
            else if (CurrentScene.name == "Level3")
            {
                InitializeLevel3();
            }
            else if (CurrentScene.name == "VictoryScreen")
            {
                //Destroy this script and gameobject
                Destroy(GetComponent<TL_DestroyDuplicateObject>());
                Destroy(gameObject);
            }
            //Set the previous scene as the current scene
            PreviousScene = CurrentScene;
        }
    }

    void Update()
    {
        LoadLevelRespectiveOfSceneName();
    }

}
