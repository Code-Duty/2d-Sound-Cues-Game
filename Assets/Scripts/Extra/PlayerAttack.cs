using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject AttackSphere;
    public Transform ShootingPoint;
    public float ShootingForce = 1000f;
    public float ShootingTime = 0.5f;
<<<<<<< HEAD

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
=======
    public void ShootProjectile()
>>>>>>> dbae7a5dc882037a8e63599298dd7964bf128d8e
    {
        if (AttackSphere && ShootingPoint)
        {

            GameObject my_projetile = Instantiate(AttackSphere, ShootingPoint.position, ShootingPoint.rotation);
            Rigidbody2D my_body = my_projetile.GetComponent<Rigidbody2D>();
            if (my_body)
            {
<<<<<<< HEAD
                Debug.LogError(ShootingPoint.right);
=======
                //Debug.LogError(ShootingPoint.right);
>>>>>>> dbae7a5dc882037a8e63599298dd7964bf128d8e
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
