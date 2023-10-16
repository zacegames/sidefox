using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    //Object for the bullet
    //public GameObject eBulletObj;

    //Bullet speed
    public float eHoriBulletSpeed;

    public float eVertBulletSpeed;

    //Bullet firing pattern
    public int eBulletPattern;

    //Speed of the enemy that spawned the bullet
    public float eSpawnSpeed;

    //Damage of the object
    public float eBulletDamage;

    //See if the enemy is a rotating one
    public bool eRotationStatus;

    //store the rotation values of the enemy
    public Quaternion enemyRotationValues;

    //Enemy transform - used for rotating enemies to make sure the bullet goes forward
    //from their rotation
    public Vector3 eTransform;

    private Rigidbody2D eBullRigid;

    private Vector2 eBulletTargetLocation;

    //Calling this class to set the player health damage
    //  public Player_Health_Bar pHealthBar;

    //Rigidbody2D rigidbody;

    //    private Vector3 movement;

    void Awake()
    {
        //Ignore collision between the player and the bullet
        Physics2D.IgnoreLayerCollision(10, 8);

        //Ignore the collision between the bullets
        Physics2D.IgnoreLayerCollision(10, 11);

        //Ignore collision betweeen all enemy bullets
        Physics2D.IgnoreLayerCollision(10, 10);
    }

    private void Start()
    {
        eBullRigid = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void FixedUpdate()        
    {
        eBullMvmt(eRotationStatus, eTransform);
    }

    public void eSetBulletValues(float eHoriMvmt, float eVertMvmt, int eBullPatt, int eBullDam, float eSpSpeed, bool eRot, Quaternion eRotValues)
    {
        eHoriBulletSpeed = eHoriMvmt * Time.deltaTime;

        eVertBulletSpeed = eVertMvmt * Time.deltaTime;

        eBulletPattern = eBullPatt;

        eBulletDamage = eBullDam;

        eSpawnSpeed = eSpSpeed;
        
        eRotationStatus = eRot;

        enemyRotationValues = eRotValues;
    }

    private void eBullMvmt(bool eRotStatus, Vector3 eTrans)
    {
        

        switch (eBulletPattern)
        {
            case 1:
                if (!eRotStatus)
                {
                    transform.position += Vector3.left * eHoriBulletSpeed;
                }
                if (eRotStatus)
                {

                    // eBullRigid.velocity


                    transform.rotation = enemyRotationValues;

                    Vector2 eBulletTargetLocation = (Vector2)Player.PlayerPosition - eBullRigid.position;

                    eBulletTargetLocation.Normalize();

                    
                    //transform.position += -transform.right * eHoriBulletSpeed;
                    //transform.position -= new Vector3(eHoriBulletSpeed, 0, 0f);
                    transform.position += new Vector3(eBulletTargetLocation.x, eBulletTargetLocation.y,0) * 10f * Time.deltaTime;
                }
                break;
            //Used for boss bullets
            case 2:

                //Vector3 DiaMvmt = new Vector3();

                //DiaMvmt = new Vector3(eHoriBulletSpeed, eVertBulletSpeed, 0f);

                transform.position += new Vector3((eHoriBulletSpeed * -1f), eVertBulletSpeed, 0f);
                break;
            case 3:

                //Vector3 NegDiaMvmt = new Vector3();

                //NegDiaMvmt = new Vector3(eHoriBulletSpeed, -eVertBulletSpeed, 0f);

                transform.position -= new Vector3(eHoriBulletSpeed, eVertBulletSpeed, 0f);
                break;
            default:
                transform.position += Vector3.left *  eHoriBulletSpeed;
                break;
        }
    }

    
    
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Environment")
        {
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<Player>().pHealth -= eBulletDamage;
            Player.pHealth -= eBulletDamage;
        }
    }
}


