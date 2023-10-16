using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    //Used to control bullet speed
    [SerializeField]
    private float BulletSpeed;
    
    //Used to define what the bullet shape for the player should be
    public GameObject BulletShape;

    //Player bullet damage
    [SerializeField]
    public int pBullDamage;

    void Awake()
    {
        //Ignore collision between the player and the bullet
        Physics2D.IgnoreLayerCollision(11,9);

        //Ignore the collision between the bullets
        Physics2D.IgnoreLayerCollision(11, 10);

        //Ignore collision between all player bullets
        Physics2D.IgnoreLayerCollision(11, 11);
     }

    private void Start()
    {
        BulletSpeed = (Player.PlayerSpeed + BulletSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.right * BulletSpeed;
        Vector3 movement = transform.rotation * (new Vector3(BulletSpeed, 0, 0) * Time.deltaTime);

        transform.position += movement;

    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Environment")
        {
            Destroy(this.gameObject);
        }
    }
}
