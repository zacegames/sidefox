using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Missile tutorial here - https://www.youtube.com/watch?v=0v_H3oOR0aU

//Targeting information - https://forum.unity.com/threads/find-and-delete-closest-gameobject-with-tag-solved.419205/

public class PlayerHomingMissile : MonoBehaviour
{
    //Target for the missile
    private GameObject MissileTarget;

    //used to store targets the missiles could hit
    private GameObject[] EnemyArray;

    public Rigidbody2D missileRidigbody;

    //Turing Speed
    public float turn;

    //Missile Speed
    public float missileVelocity;

    public int pSecondaryDamage;

    private void Awake()
    {
        //Ignore collision with player
        Physics2D.IgnoreLayerCollision(12, 9);

        //Ignore collision with other missiles 
        Physics2D.IgnoreLayerCollision(12, 12);

        //Ignore collision with the standard player bullets
        Physics2D.IgnoreLayerCollision(12, 11);
        
        //Ignore collision with other enemy bullets
        Physics2D.IgnoreLayerCollision(12, 10);


    }

    private void Start()
    {
        missileRidigbody = GetComponent<Rigidbody2D>();

        //find all the enemies, and target the closest
        EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        float curDist = 0f;

        foreach (GameObject Targ in EnemyArray)
        {

            //Debug.Log("Collider Status = " + Targ.GetComponent<PolygonCollider2D>().enabled);
            if (Targ.GetComponent<PolygonCollider2D>().enabled == true)
            {
                float dist = Vector3.Distance(transform.position, Targ.transform.position);

              //  Debug.Log("Enemy Distance = " + dist);

                if (dist > curDist)
                {
                    curDist = dist;

                    MissileTarget = Targ;
                }
            }
        }
    } 

 
    private void FixedUpdate()
    {

        Debug.Log("Missile Target - " + MissileTarget);

        if (MissileTarget)
        {
            Vector2 missileTargetLocation = (Vector2)MissileTarget.transform.position - missileRidigbody.position;

            //Debug.Log(missileTargetLocation);

            missileTargetLocation.Normalize();

            float RotateAmount = Vector3.Cross(missileTargetLocation, transform.up).z;

            missileRidigbody.angularVelocity = -RotateAmount * turn;

            missileRidigbody.velocity = transform.up * missileVelocity;
        }
        
        else if(missileRidigbody.angularVelocity != 0 && !MissileTarget)
        {
            missileRidigbody.angularVelocity = 0;
        }
        
        else
        {
            missileRidigbody.velocity = transform.up * missileVelocity;
        }
    }

    //If the missile goes off screen, kill it
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
