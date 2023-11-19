using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Used to control how fast the player moves in the area
    public float MoveSpeed;

    //Used so other script can access PlayerSpeed
    public static float PlayerSpeed;

    //Current bullet that the player has selected
    public GameObject CurrentBullet;

    //Used to store the players' secondary fire
    public GameObject SecondaryFire;

    //So I can access the players position to see if objects are behind it
    public static Vector3 PlayerPosition;

    //Used to detect if main gun button has been held down
    private float BulletCount;

    //Used to calculate the size of the bullet collider
    private float pbulletSize;

    //Used to change the size of the main gun bullet over time
    private float BulletScale;

    //Set it so the bullet doesn't get massive
    private float MaxBulletSize = 1.5f;

    //Used to store default bullet damage
    public int DefaultBulletDamage;

    //Used to store maximum bullet damage
    private int MaxBulletDamage = 400;
    
    //Current player bullet damage
    public static int BulletDamage;

    //Used to trigger whether the player has started the dash
    public bool DashTrigger;

    //Used so the player has to wait until the recharge is done before they
    //can dash again
    public static bool DashRecharge;

    private Material PlayerMaterial;

    private Color DefaultColor = Color.white;

    //Use to measure how long to shake the player for
    [SerializeField]
    public float pShakeTotalTime;

    //Total time the player shakes for
    private float pShakeAmt;

    //Used to see if the player has  hit by an enemy
    public static bool pHit;

    [Header("Player Ability Values")]
    //Used to control how long the player can dash for
    public float DashTime;

    //Used to store value to update the dash bar
    public static float DashBarUpdate;

    //Used to control the player's dash speed
    public float DashSpeed;

    public GameObject Player_Charge;

    private bool PlayerCharging;

    private Color PlayerChargingColor;

    //Ship Size
    private float PlayerWidth;

    private float PlayerHeight;


    /*== ACTION VARIABLES ==*/

    //Is the player rotating
    private bool Rotating;

    //If the player is in the reflect mode
    private bool Reflecting;

    //Make sure the player only rotates 360 degrees
    private int rotatedegrees;

    //Use to control how fast the player rotates during the rotate action
    public float RotateSpeed;
    
    //Used to control how fast the player rotates during the reflect action
    public float ReflectSpeed;

    private Camera MainCamera;

    [Header("Player Health UI values")]
    //Used to calculate enemy health bars. This is the total of the enemies health
    [SerializeField]
    public static float Overall_pHealth;

    //Player Health
    [SerializeField]
    public static float pHealth;

    public Player_Health_Bar pHealthBar;

    private Vector3 pos;

    private Collider2D PlayerCollider;

    private Vector2 playerMovementDirection;

    private Rigidbody2D playerRB;

    private void Awake()
    {

        PlayerCollider = this.GetComponent<Collider2D>();

        MainCamera = Camera.main;

//        PlayerWidth = transform.GetComponent<BoxCollider2D>().bounds.size.x;

        PlayerHeight = transform.GetComponent<BoxCollider2D>().bounds.size.y;

    }

    // Start is called before the first frame update
    void Start()
    {
        Overall_pHealth = 100;

        pHealth = Overall_pHealth;

        playerRB = GetComponent<Rigidbody2D>();

        PlayerSpeed = MoveSpeed;

       // Debug.Log("Move Speed - " + MoveSpeed);
        //Debug.Log("time.deltatime - " + Time.deltaTime);
        //Debug.Log("Player Speed from player script - " + PlayerSpeed);

        DashSpeed = DashSpeed * Time.deltaTime; //deltatime used to normalise speed of movement across all devices
        //Turn on the dash trigger
        DashTrigger = true;

        pHit = false;
        
        DashBarUpdate = 1f;

        PlayerMaterial = GetComponent<Renderer>().material;

        //Set the player health to the maximum
        pHealthBar.SetMaxHealth((int)Overall_pHealth);

        PlayerChargingColor = new Color(1f, 0.38f, 0.46f);

        //Set the default bullet damage
        // DefaultBulletDamage = 100;

        DefaultBulletDamage = CurrentBullet.GetComponent<PlayerBullet>().pBullDamage;

        BulletDamage = DefaultBulletDamage;

        PlayerCharging = false;

        //Set the shake parameters
        pShakeAmt = 0.25f;

        pShakeTotalTime = pShakeAmt;

        pbulletSize = CurrentBullet.GetComponent<SpriteRenderer>().bounds.size.x;
        
    }

    // Update is called once per frame
    void Update()
    {

        playerMovementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Update the player's position to the global position
        PlayerPosition = transform.position;

        /*
        //If the enemy gets hit with a bullet, shake them
        if (pHit)
        {
            if (pShakeTotalTime > 0)
            {
                transform.position = transform.position + Random.insideUnitSphere * 0.25f;
                pShakeTotalTime -= Time.deltaTime;
            }
            if (pShakeTotalTime <= 0)
            {
                //this.transform.position = ePosition;
                pHit = false;
                pShakeTotalTime = pShakeAmt;
            }
        }*/

        //Reset the bullet damage
        if (BulletDamage != DefaultBulletDamage)
        {
            BulletDamage = DefaultBulletDamage;
        }

        //Debug.Log("Rotate Deg = " + rotatedegrees);
        //Debug.Log("Rotation = " + transform.rotation);
        //Debug.Log("H = " + Input.GetAxisRaw("Horizontal"));
        //Debug.Log("V = " + Input.GetAxisRaw("Vertical"));

        //Lock gameobject to Camera view - https://answers.unity.com/questions/799656/how-to-keep-an-object-within-the-camera-view.html

        pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -TestSceneController.SceneDimensions.x + (PlayerWidth / 2), TestSceneController.SceneDimensions.x - (PlayerWidth / 2));
        pos.y = Mathf.Clamp(pos.y, -TestSceneController.SceneDimensions.y + (PlayerHeight / 2), TestSceneController.SceneDimensions.y - (PlayerHeight / 2));
        transform.position = pos;

        //If pressing Q (Keyboard Controls) or square (PS4), perform the dash
        if (Input.GetKey(KeyCode.Q) || Input.GetKey("joystick button 0"))
        {
            //If the player has finished recharging 
            if (DashTime >= 0.1f && DashRecharge)
            {
                DashRecharge = false;
            }

            if (DashTrigger == true)
            {
                DashTrigger = false;
            }

            if (DashTrigger == false && DashTime > 0 && !DashRecharge)
            {
                PlayerCollider.enabled = false;
                transform.position += (Vector3.right * DashSpeed);
                DashBarUpdate -= Time.deltaTime * 10f;
                DashTime -= Time.deltaTime;
            }

            if(DashTime <= 0 && DashTrigger == false)
            {
                DashRecharge = true;
            }
        }

        if (DashTime <= 0.1f && DashTrigger == false)
        {
            PlayerCollider.enabled = true;

            if (DashTime < 0.1f)
            {
                DashTime += (Time.deltaTime / 10);
                DashBarUpdate += (Time.deltaTime / 10) * 10f;
            }         

            if(DashTime >= 0.1f)
            {
                DashTrigger = true;
                DashBarUpdate = 1f;
                if (DashRecharge)
                {
                    DashRecharge = false;
                }
            }
        }


        // //JOYSTICK CONTROLS - X - 0.4 for deadzone
        // if (Input.GetAxisRaw("Horizontal") > 0.7f || Input.GetAxisRaw("Horizontal") < -0.7f)
        // {
        //     transform.position += new Vector3((Input.GetAxisRaw("Horizontal") * MoveSpeed), 0, 0);
        // }

        // //JOYSTICK CONTROLS - Y - 0.4 for deadzone
        // if (Input.GetAxisRaw("Vertical") > 0.7f || Input.GetAxisRaw("Vertical") < -0.7f)
        // {
        //     transform.position += new Vector3(0, (-Input.GetAxisRaw("Vertical") * MoveSpeed), 0);
        // }

    //    Debug.Log(Input.GetAxisRaw("Vertical"));


        //PS4 Controls - "joystick button 1" is X. Go here for full controller config - https://www.reddit.com/r/Unity3D/comments/1syswe/ps4_controller_map_for_unity/
        //------SHOOTING ENEMY-------//
        //Detect if the player has pressed the shoot button.
        //Now based on how long they held the button for, produce a regular size bullet or a super charged bullet!!
        if (Input.GetKey("joystick button 1") || Input.GetKey(KeyCode.Space)) {

            BulletCount += Time.deltaTime;
         //   Debug.Log("BulletCount = " + BulletCount.ToString());
        }

        if (BulletCount > 0.5f && !PlayerCharging) {
            GameObject PC = Instantiate(Player_Charge, transform.localPosition, Quaternion.identity);
            PC.transform.SetParent(this.transform);
            PlayerCharging = true;
        }

        //If the player is charging, start changing the ship colour, increase the bullet damage and size
        if (PlayerCharging)
        {
            PlayerMaterial.color = Color.Lerp(PlayerMaterial.color, PlayerChargingColor, 0.003f);

            if (BulletDamage != MaxBulletDamage)
            {
                BulletDamage = BulletDamage * 6;
            }
                          
            if(CurrentBullet.transform.localScale.x < MaxBulletSize)
            {
                CurrentBullet.transform.localScale = new Vector2(BulletCount * 1.1f, BulletCount * 1.1f); //You'll need to change the '6' when the sprite changes. It needs to be the X and Y size of the bullet GameObject
            }
            
        }

        if (Input.GetKeyUp("joystick button 1") || Input.GetKeyUp(KeyCode.Space)) {
            //Debug.Log("Player Rotation = " + transform.rotation);
            //Calculat were to spawn the bullet and spawn it
            float SpawnPlace = (PlayerWidth / 2) + (pbulletSize / 2) + 0.05f;
            GameObject Bull = Instantiate(CurrentBullet, new Vector3(transform.position.x + SpawnPlace, transform.position.y, transform.position.z), transform.rotation);
            Bull.GetComponent<PlayerBullet>().pBullDamage = BulletDamage;
            BulletCount = 0;

            if (PlayerCharging)
            {
                BulletDamage = DefaultBulletDamage;
                CurrentBullet.transform.localScale = new Vector2(1f, 1f);
                PlayerCharging = false;
                PlayerMaterial.color = DefaultColor;
            }
        }
                 
        }

    private void FixedUpdate()
    {
        playerRB.MovePosition (playerRB.position +  playerMovementDirection * MoveSpeed * Time.fixedDeltaTime);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //if the player is hit by a bullet
        if (collision.gameObject.layer == 10)
        {
            //Damage the player
            pHealthBar.SetHealth(pHealth);
            Destroy(collision.gameObject);

            //Reset the combo counter
            UI_Controller.ComboCounter = 0;
         }

        //if the player is hit by an enemy
        if(collision.gameObject.layer == 8)
        {
            pHit = true;

            pHealth -= 20;
            pHealthBar.SetHealth(pHealth);
            Destroy(collision.gameObject);

            //Reset the combo counter
            UI_Controller.ComboCounter = 0;
        }

        //When the players health is 0, kill the player
        if (pHealth <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    void PlayerShake()
    {

    }





    //OLD REFLECT OR ROTATE CODE
    /*        //PS4 Controls - "joystick button 6" is L2 - Trigger Reflecting
            if (Input.GetKey("joystick button 6"))
            {
                if(Reflecting == false && Rotating == false)
                {
                    Reflecting = true;
                }           
            }

            //PS4 Controls - "joystick button 6" is L2 - Trigger Rotating
            if (Input.GetKey("joystick button 7"))
            {
                if (Rotating == false && Reflecting == false)
                {
                    Rotating = true;
                }
            }

            //REFLECT BULLETS
            if (Reflecting == true)
            {
                if (rotatedegrees < 360)
                {
                    transform.Rotate((new Vector3(0, 0, 1) * ReflectSpeed));
                    rotatedegrees += (int)RotateSpeed;
                    Debug.Log("Should be reflecting");
                }

                if(rotatedegrees >= 360)
                {
                    Reflecting = false;
                    rotatedegrees = 0;
                }
            }

            //Rotate Player Ship
            if (Rotating == true)
            {
                if (rotatedegrees < 360)
                {
                    transform.Rotate((new Vector3(0, 1, 0) * RotateSpeed));
                    rotatedegrees += (int)RotateSpeed;
                    this.GetComponent<Collider2D>().enabled = false;
                }

                if (rotatedegrees >= 360)
                {
                    Rotating = false;
                    rotatedegrees = 0;
                    this.GetComponent<Collider2D>().enabled = true;
                }
            }

            //REFLECT BULLETS
            if (Reflecting == true)
            {
                if (rotatedegrees < 360)
                {
                    transform.Rotate((new Vector3(0, 0, 1) * ReflectSpeed));
                    rotatedegrees += (int)RotateSpeed;
                    Debug.Log("Should be reflecting");
                }

                if (rotatedegrees >= 360)
                {
                    Reflecting = false;
                    rotatedegrees = 0;
                }
            }*/

    /*

     //NEW ACTION CODE - 27/08/2020
     //Trigger the rotation (player dodges bullets)
        if (Input.GetKey("joystick button 6") && !Rotating && !Reflecting) {
            if (!Rotating)
            {
                Rotating = true;
                rotatedegrees = 0;
                StartCoroutine(PlayerAction());
            }
        }
    //Trigger reflection (player reflects bullets)
        if (Input.GetKey("joystick button 7") && !Rotating && !Reflecting) {
            if (!Reflecting)
            {
                Reflecting = true;
                rotatedegrees = 0;
                StartCoroutine(PlayerAction());
            }
        }

    
   // Debug.Log("Reflecting = " + Reflecting);
   */


    //IENUMERATOR FOR Player Actions
    IEnumerator PlayerAction()
    {

        //Rotate the player if they have triggered
        if (Rotating)
        {
            while (rotatedegrees < 360)
            {
                transform.Rotate((new Vector3(0, 0, 1)) * ReflectSpeed);
                rotatedegrees += (int)RotateSpeed;
             //   Debug.Log("Rotate Degrees = " + rotatedegrees);
                yield return null;
            }
        }

        //Reflect bullets if thats the action that's triggered
        if (Reflecting)
        {
            while (rotatedegrees < 360)
            {
                transform.Rotate((new Vector3(0, 1, 0)) * ReflectSpeed);
                rotatedegrees += (int)RotateSpeed;
                //   Debug.Log("Rotate Degrees = " + rotatedegrees);
                yield return null;
            }
        }

        ///Debug.Log("Rotating = " + Rotating + " Time = " + Time.time);

        //Dont let the player yam on the button continously
        yield return new WaitForSeconds(2);

        //Reset whatever the player has triggered
        if (Rotating)
        {
            Rotating = false;
        }

        if (Reflecting)
        {
            Reflecting = false;
        }
    }
}
