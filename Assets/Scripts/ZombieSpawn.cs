using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    public Rigidbody enemyRb;

    public Transform player;

    private float moveSpeed = 5f;

    public HudController hudController;

    private void Update()
    {
        Vector3 direction = player.position - transform.position;

        //enemyRb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        direction.Normalize();

        enemyRb.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hudController.UpdatePlayerHp(30);

            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            hudController.EnemyDamage();

            Destroy(this.gameObject);
        }
    }
}
