using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnHandler : MonoBehaviour
{
    public GameObject zombiePrefab;

    public Transform[] spawnPoints;

    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GetZombieObject", 0.5f, 5f);
    }

    //float timeGap = 5;
    // Update is called once per frame
    //void Update()
    //{
    //    timeGap -= Time.deltaTime;
    //    if (timeGap <= 0)
    //    {
    //        timeGap = 5f;
    //        GetZombieObject();
    //    }
    //}

    void GetZombieObject()
    {
        if (zombiePrefab)
        {
            Transform pos = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject obj = Instantiate(zombiePrefab, pos.position, pos.rotation);

            obj.SetActive(true);

            obj.transform.parent = parent;

            //obj.transform.rotation = parent.localRotation;
        }
        else
            print("Missing the bullet prefab");
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
