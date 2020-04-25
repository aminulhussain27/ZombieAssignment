using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject floorObject;

    public Transform[] zombieSpawnPositions;
    public Transform centrePoint;
    // Start is called before the first frame update
    public float lengthOfFloor;
    public float widthOfFloor;
    public Vector3 centrPoint;
    void Awake()
    {
        Vector3 sizeOfLoor = floorObject.GetComponent<Collider>().bounds.size;
        centrPoint = new Vector3(sizeOfLoor.x / 2, 0, sizeOfLoor.z / 2);
    }

}
