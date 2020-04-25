using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;


    public float currentSpeed;

    public float deaccelerationSpeed = 15.0f;

    public float accelerationSpeed = 50000.0f;

    public int maxSpeed = 5;

    private Vector2 horizontalMovement;

    public Vector3 cameraPosition;

    private Vector3 slowdownV;

    public Environment environment;

    public Transform cameraMain;

    public Transform bulletSpawn; //from here we shoot a ray to check where we hit him;

    /*
	 * Getting the Players rigidbody component.
	 * And grabbing the mainCamera from Players child transform.
	 */
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraMain = transform.Find("Main Camera").transform;
        bulletSpawn = cameraMain.Find("BulletSpawn").transform;
    }
    private void Start()
    {
        transform.position = new Vector3(0, 10, 0);
    }


    void FixedUpdate()
    {
        PlayerMovementLogic();
    }
    /*
	* Accordingly to input adds force and if magnitude is bigger it will clamp it.
	* If player leaves keys it will deaccelerate
	*/
    void PlayerMovementLogic()
    {
        currentSpeed = rb.velocity.magnitude;
        horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);
        if (horizontalMovement.magnitude > maxSpeed)
        {
            horizontalMovement = horizontalMovement.normalized;
            horizontalMovement *= maxSpeed;
        }
        rb.velocity = new Vector3(
            horizontalMovement.x,
            rb.velocity.y,
            horizontalMovement.y
        );

        rb.velocity = Vector3.SmoothDamp(rb.velocity,
            new Vector3(0, rb.velocity.y, 0),
            ref slowdownV,
            deaccelerationSpeed);


        rb.AddRelativeForce(Input.GetAxis("Horizontal") * accelerationSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * accelerationSpeed * Time.deltaTime);

        rb.AddRelativeForce(Input.GetAxis("Horizontal") * accelerationSpeed / 2 * Time.deltaTime, 0, Input.GetAxis("Vertical") * accelerationSpeed / 2 * Time.deltaTime);
        /*
		 * Slippery issues fixed here
		 */
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            deaccelerationSpeed = 0.5f;
        }
        else
        {
            deaccelerationSpeed = 0.1f;
        }
    }

    [Tooltip("Sound while player makes when successfully reloads weapon.")]
    public AudioSource _freakingZombiesSound;
    [Tooltip("Sound Bullet makes when hits target.")]
    public AudioSource _hitSound;
    [Tooltip("Walk sound player makes.")]
    public AudioSource _walkSound;

}

