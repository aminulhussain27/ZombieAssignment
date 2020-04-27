using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 1400f);
        Destroy(gameObject, 2);//Auto destroy after 3 sec
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
