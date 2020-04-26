using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnHandler : MonoBehaviour
{
    public GameObject zombiePrefab;
    // Start is called before the first frame update
    void Start()
    {

    }
    float timeGap = 5;
    // Update is called once per frame
    void Update()
    {
        timeGap -= Time.deltaTime;
        if (timeGap <= 0)
        {
            timeGap = 5f;
            GetZombieObject();
        }
    }

    void GetZombieObject()
    {
        if (zombiePrefab)
            Instantiate(zombiePrefab, transform, true);
        else
            print("Missing the bullet prefab");
    }
}
