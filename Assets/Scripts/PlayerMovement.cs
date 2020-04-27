using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region PlayerMovementVariables
    private Rigidbody playerRigidBody;

    private Vector3 slowdownV;

    private Vector2 horizontalMovement;

    public int maxSpeed = 5;

    public float currentSpeed;

    public Vector3 cameraPosition;

    public Environment environment;

    public Transform cameraMain;

    [HideInInspector] public float deaccelerationSpeed = 15.0f;

    [HideInInspector] public float accelerationSpeed = 50000.0f;
    #endregion

    #region MouseMovementVariables
    private float rotationYVelocity, cameraXVelocity;
    private float topAngleView = 45;
    private float bottomAngleView = -30;
    public float yRotationSpeed, xCameraSpeed;
    public float wantedYRotation;
    public float currentYRotation;
    public float wantedCameraXRotation;
    public float currentCameraXRotation;
    public float zRotation;
    [HideInInspector] public float mouseSensitvity = 3;
    #endregion

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        transform.position = new Vector3(0, 10, 0);
    }

    private void Update()
    {
        GetMouseInput();
    }

    private void FixedUpdate()
    {
        PlayerMovementLogic();

        MouseAimMovement();
    }
    
	/* Accordingly to input adds force and if magnitude is bigger it will clamp it. If player leaves keys it will deaccelerate */
    void PlayerMovementLogic()
    {
        currentSpeed = playerRigidBody.velocity.magnitude;
        horizontalMovement = new Vector2(playerRigidBody.velocity.x, playerRigidBody.velocity.z);
        if (horizontalMovement.magnitude > maxSpeed)
        {
            horizontalMovement = horizontalMovement.normalized;
            horizontalMovement *= maxSpeed;
        }
        playerRigidBody.velocity = new Vector3(
            horizontalMovement.x,
            playerRigidBody.velocity.y,
            horizontalMovement.y
        );

        playerRigidBody.velocity = Vector3.SmoothDamp(playerRigidBody.velocity,
            new Vector3(0, playerRigidBody.velocity.y, 0),
            ref slowdownV,
            deaccelerationSpeed);


        playerRigidBody.AddRelativeForce(Input.GetAxis("Horizontal") * accelerationSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * accelerationSpeed * Time.deltaTime);

        playerRigidBody.AddRelativeForce(Input.GetAxis("Horizontal") * accelerationSpeed / 2 * Time.deltaTime, 0, Input.GetAxis("Vertical") * accelerationSpeed / 2 * Time.deltaTime);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            deaccelerationSpeed = 0.5f;
        }
        else
        {
            deaccelerationSpeed = 0.1f;
        }
    }

    #region MouseMovement

    void GetMouseInput()
    {
        wantedYRotation += Input.GetAxis("Mouse X") * mouseSensitvity;
        wantedCameraXRotation -= Input.GetAxis("Mouse Y") * mouseSensitvity;
        wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);
    }

    void MouseAimMovement()
    {
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);
        //WeaponRotation();
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
        cameraMain.localRotation = Quaternion.Euler(currentCameraXRotation, 0, zRotation);
    }
    #endregion

    [Tooltip("Sound while player makes when successfully reloads weapon.")]
    public AudioSource _freakingZombiesSound;
    [Tooltip("Sound Bullet makes when hits target.")]
    public AudioSource _hitSound;
    [Tooltip("Walk sound player makes.")]
    public AudioSource _walkSound;

}

