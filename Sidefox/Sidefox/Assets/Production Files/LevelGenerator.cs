using UnityEngine;

/* == COLOUR KEY (with RGB values, alpha on all of these is 255) == 
 * 
 * Black (0,0,0) - Standard enemy
 *
 * Red (255,0,0) - Diagonal enemy
 *
 * Poo Brown (102,102,0) Diagonal enemy, rotate towards player
 *
 * Blue (0,255,0) - sin wave enemy
 *
 * Magenta (255,0,255) - swooping in enemy 
 *
 *  ============== */



//Level Editor tutorial - https://www.youtube.com/watch?v=B_Xp9pt8nRY&list=LLPXPVuQ5QrkRW2jg2keeVYQ&index=2
public class LevelGenerator : MonoBehaviour
{
    public Texture2D LevelMap;

    public LG_ColourToPrefab[] ColourMapping;

    private Color pixelColour;

    //Used to measure the world size
    public static Vector3 WorldSize;

    //Used to get the screen size so we can know when to start the enemy movement

    private int x;

    private int y;

    private float EnemyYPos;

    //All of the below are used for the pattern 4 slerp movement. Information here -
    // https://docs.unity3d.com/ScriptReference/Vector3.Slerp.html
    public Vector3 pat4startpos;

    public Vector3 pat4endpos;

    //Used to control the start time of the pattern 4 slerp angle. This needs to be set where ever we want to begin our SLERP movement
    private float startTime;


    void Awake()
    {
        WorldSize = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
    }

    // Start is called before the first frame update
    void Start()
    {
        //Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Camera.main.pixelHeight, 10f));

        //Debug.Log("WORLD POS = " + Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10f)));

         Debug.Log("WorldSize = " + WorldSize);

        GenerateLevel();
   }

    void GenerateLevel()
    {
        for (x = 0; x < LevelMap.width; x++)
        {
            for (y = 0; y < LevelMap.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        pixelColour = LevelMap.GetPixel(x, y);

      //  Debug.Log("Pixel Colour = " + pixelColour);
        foreach (LG_ColourToPrefab ColMap in ColourMapping)
        {
            if(ColMap.Colour.Equals(pixelColour))
            {
                GameObject Enemy = ColMap.EnemyPrefab;

                Transform HealthBar = Enemy.transform.GetChild(0);

                //  Debug.Log("Enemy Size = " + Enemy.transform.localScale.x);

                //    Debug.Log("UI Size = " + (HealthBar.GetComponent<RectTransform>().sizeDelta.y * (HealthBar.transform.localScale.y * 2)));

                //  float Spacing = (Enemy.transform.localScale.y + (HealthBar.GetComponent<RectTransform>().sizeDelta.y * (HealthBar.transform.localScale.y * 2)));

                //Calculate Y pos based on level editor size by world units - THIS IS DEPENDANT ON THE POSITIVE SCREEN HEIGHT BEING THE SAME AS THE NEGATIVE SCREEN HEIGHT.
                /*This is currently used for the following enemy pattern types -
                 * Enemy 1
                 * Enemy 3
                 * Enemy 4
                 * 
                 * Enemy 2 uses a different pattern
                 * */

                //First, need to figure out what half the LevelMap Height is 
                float LMHeight = LevelMap.height / 2;
           //     Debug.Log("LMHeight - " + LMHeight);

                //then we devide this by the screen height
                float Y_Spacing = WorldSize.y / LMHeight;
              //  Debug.Log("Y_Spacing - " + Y_Spacing);

              //  Debug.Log("y - " + y);

                //finally, we get where we should be putting this at. NOTE that need to take away location on pixelmap from LMheight then multiply. Example for top position
                //LMHeight - 1 * Y_Spacing
                if (y < LMHeight && y != 0)
                {
                    EnemyYPos = (LMHeight - y) * Y_Spacing;
                }
                else if(y == LMHeight)
                {
                    EnemyYPos = Y_Spacing;
                }
                else if (y > LMHeight && y < LevelMap.height)
                {
                    EnemyYPos = (y - LMHeight) * Y_Spacing;  
                }
                else if (y == 0)
                {
                    EnemyYPos = -WorldSize.y + Y_Spacing;
                }

                //Debug.Log("EnemyYPos = " + EnemyYPos);
                

                Vector3 position  = new Vector3((WorldSize.x + (float)x + Enemy.transform.localScale.x), EnemyYPos, 0f); 
                
                GameObject E1 = Instantiate(Enemy, position, Quaternion.identity);

                if (pixelColour.a == 0)
                {
                    //Set all variables to 0 so no previous settings are held over

                    E1.GetComponent<Enemy_1>().EnemyMovementPattern = 0;
                    E1.GetComponent<Enemy_1>().eHealth = 100f;
                    E1.GetComponent<Enemy_1>().eBaseBulletTimer = 3f;
                    E1.GetComponent<Enemy_1>().RotateTowardsPlayer = false;
                    Enemy.GetComponent<Enemy_1>().RotateTowardsPlayer = false;
                    //Pixel is transparent. Ignore the pixel
                    return;
                }

                //Change the flight pattern based on the 
                if (pixelColour == Color.black)
                {
                    E1.GetComponent<Enemy_1>().EnemyMovementPattern = 0;
                    E1.GetComponent<Enemy_1>().eHealth = 100f;
                    E1.GetComponent<Enemy_1>().eBaseBulletTimer = 3f;
                    E1.GetComponent<Enemy_1>().RotateTowardsPlayer = false;
                }

                if (pixelColour == Color.red)
                {
                    E1.GetComponent<Enemy_1>().EnemyMovementPattern = 1;
                    E1.GetComponent<Enemy_1>().eHealth = 200f;
                    E1.GetComponent<Enemy_1>().eBaseBulletTimer = 3f;
                    E1.GetComponent<Enemy_1>().RotateTowardsPlayer = false;
                    E1.transform.position = new Vector3(E1.transform.position.x, WorldSize.y, E1.transform.position.z);
                }


                if (pixelColour == new Color(0.4f, 0.4f, 0, 1))
                {
                    E1.GetComponent<Enemy_1>().EnemyMovementPattern = 2;
                    E1.GetComponent<Enemy_1>().eHealth = 200f;
                    E1.GetComponent<Enemy_1>().eBaseBulletTimer = 3f;
                    E1.GetComponent<Enemy_1>().RotateTowardsPlayer = true;
                    E1.transform.position = new Vector3(E1.transform.position.x, -WorldSize.y, E1.transform.position.z);
                }


                if (pixelColour == Color.blue)
                {
                    E1.GetComponent<Enemy_1>().EnemyMovementPattern = 3;
                    E1.GetComponent<Enemy_1>().eHealth = 200f;
                    E1.GetComponent<Enemy_1>().eBaseBulletTimer = 3f;
                    E1.GetComponent<Enemy_1>().RotateTowardsPlayer = false;
                }

                if (pixelColour == Color.magenta)
                {
                    //Set the start and end positions for the enemy to move along. They need to be static for correct movement
                    E1.GetComponent<Enemy_1>().EnemyMovementPattern = 4;
                    E1.GetComponent<Enemy_1>().eHealth = 200f;
                    E1.GetComponent<Enemy_1>().eBaseBulletTimer = 3f;
                    E1.GetComponent<Enemy_1>().RotateTowardsPlayer = false;
                }

                /*
                switch (E1.GetComponent<Enemy_1>().EnemyMovementPattern)
                {
                    //Diagonal movement, bottom to top
                    case 1:
                        //choose whether to spawn the enemy at the top or bottom of the screen
                        Debug.Log("diagonal set - " + new Vector3(transform.position.x, WorldSize.y, transform.position.z));
                        
                        break;

                    case 2:
                        
                        break;

                    case 4:
                        break;

                    default:

                        break;
                }
                */


            }
        } 
    }

}
