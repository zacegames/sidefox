using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //Bullet speed
    [SerializeField]
    public float bulletSpeed;

    //Damage of the object
    [SerializeField]
    public float bulletDamage;

    public float bullRotation;

    public bool eRotationStatus;

    public Quaternion bulletRotation;

    private Rigidbody2D eBullRigid;
    
    
    
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
        //Debug.Log ("Bullet Rigidbody = " + eBullRigid.velocity);

        eBullMvmt(eRotationStatus);
    }

    public void eBullMvmt(bool eRotStatus)
    {
        
        //eBullRigid = GetComponent<Rigidbody2D>();
        
        if (!eRotStatus)
        {
            // eBullRigid.velocity = Vector2.left * bulletSpeed;

            eBullRigid.velocity = -transform.right * bulletSpeed;
                 
            
        }
        
        if (eRotStatus)
        {

            //This make the bullet go straight in the direction that their rotated in.
            /* NOTE! - This is dependant of the rotation of the gameobject and sprite !! */
            eBullRigid.velocity = -transform.right * bulletSpeed;
                 
        
        /*
        //The below code makes the bullet aim at the players position 
        Vector3 bulletTarget = Player.PlayerPosition - transform.position;
        // eBullRigid.velocity = new Vector2(bulletTarget.x, bulletTarget.y).normalized * bulletSpeed;

        //Not sure if this is needed?? - TEST (set in bullet spawn point in enemy script)
        // transform.rotation = bulletRotation;
        */

        }
    }

//If the enemy should be having the bullet aim at the player, we do the rotation of the bullet ONC
    
            /*
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
                */


    
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
            //Player.pHealth -= eBulletDamage;
        }
    }
}


