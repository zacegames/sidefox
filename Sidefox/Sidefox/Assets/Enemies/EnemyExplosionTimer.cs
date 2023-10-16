using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionTimer : MonoBehaviour
{

    //For the particle system
    private ParticleSystem parts;

    // Start is called before the first frame update
    void Start()
    {
        parts = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //TIP - Adding time to the end of Destroy delays the deletion! - https://forum.unity.com/threads/solved-particle-instance-finished-playing-so-can-i-destroy-it.412234/
        Destroy(this.gameObject, parts.main.duration);
    }
}
