using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject floorObject;

    public Transform centrePoint;
    // Start is called before the first frame update
    public float lengthOfFloor;
    public float widthOfFloor;
    public Vector3 centrPoint;

    public Material[] materials;
    Material currentMaterial;
    void Awake()
    {
        Vector3 sizeOfLoor = floorObject.GetComponent<Collider>().bounds.size;

        centrPoint = new Vector3(sizeOfLoor.x / 2, 0, sizeOfLoor.z / 2);

        int randomNumber = Random.Range(0, 10);
        if (randomNumber > 5)
        {
            currentMaterial = materials[0];
        }
        else
        {
            currentMaterial = materials[1];
        }
        floorObject.GetComponent<Renderer>().material = currentMaterial;
    }

}
