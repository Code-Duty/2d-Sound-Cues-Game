using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject AttackSphere;
    public Transform ShootingPoint;
    public float ShootingForce = 1000f;
    public float ShootingTime = 0.5f;
    public void ShootProjectile()
    {
        if (AttackSphere && ShootingPoint)
        {

            GameObject my_projetile = Instantiate(AttackSphere, ShootingPoint.position, ShootingPoint.rotation);
            Rigidbody2D my_body = my_projetile.GetComponent<Rigidbody2D>();
            if (my_body)
            {
                //Debug.LogError(ShootingPoint.right);
                my_body.AddForce(ShootingPoint.right * ShootingForce);
            }
            else
            {
                Debug.LogError("NOT WORKING");
            }
            Destroy(my_projetile, ShootingTime);

        }
    }


}
