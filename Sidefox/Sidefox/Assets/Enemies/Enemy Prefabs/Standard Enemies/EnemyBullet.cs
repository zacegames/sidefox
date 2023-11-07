using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //Bullet speed
    [SerializeField]
    public float bullSpeed;

    //Damage of the object
    [SerializeField]
    public float bullDamage;

    public float bullRotation;

    public bool eRotationStatus;

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
            eBullRigid.velocity = Vector2.left * bullSpeed;
            
        }
        
        if (eRotStatus)
        {
                 
        Vector3 bulletTarget = Player.PlayerPosition - transform.position;
        eBullRigid.velocity = new Vector2(bulletTarget.x, bulletTarget.y).normalized * bullSpeed;       
        float rot = Mathf.Atan2(-bulletTarget.y,-bulletTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot);

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
           // Player.pHealth -= eBulletDamage;
        }
    }
}


