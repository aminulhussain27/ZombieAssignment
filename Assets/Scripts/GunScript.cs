using UnityEngine;
using System.Collections;

public enum GunStyles
{
    semi, automatic
}
public class GunScript : MonoBehaviour
{
    public GunStyles currentStyle;
    public int walkingSpeed = 3;
    public float bulletsIHave = 20;
    public float bulletsInTheGun = 5;
    public float amountOfBulletsPerLoad = 5;
    private Transform player;
    private Camera cameraComponent;
    private PlayerMovement playerMovement;

    public Vector3 currentGunPosition;
    public Vector3 restPlacePosition;

    public float gunAimTime = 0.1f;
    public bool reloading;
    private Vector3 gunPosVelocity;
    private float cameraZoomVelocity;
    private float secondCameraZoomVelocity;

    private Vector2 velocityGunRotate;
    private float gunWeightX, gunWeightY;
    public float rotationLagTime = 0f;
    private float rotationLastY;
    private float rotationDeltaY;
    private float angularVelocityY;
    private float rotationLastX;
    private float rotationDeltaX;
    private float angularVelocityX;
    public Vector2 forwardRotationAmount = Vector2.one;

    private float currentRecoilZPos;
    private float currentRecoilXPos;
    private float currentRecoilYPos;

    [Header("Sensitvity of the gun")]
    public float mouseSensitvity_notAiming = 10;
    public float mouseSensitvity_aiming = 5;

    private Vector3 velV;
    public Transform mainCamera;
    private Camera secondCamera;

    public GameObject bulletSpawnPlace;
    public GameObject bullet;
    public float roundsPerSecond;
    private float waitTillNextFire;

    public float recoilAmount_z = 0.5f;
    public float recoilAmount_x = 0.5f;
    public float recoilAmount_y = 0.5f;

    public float recoilAmount_z_non = 0.5f;

    public float recoilAmount_x_non = 0.5f;

    public float recoilAmount_y_non = 0.5f;

    public float velocity_z_recoil, velocity_x_recoil, velocity_y_recoil;

    public float recoilOverTime_z = 0.5f;

    public float recoilOverTime_x = 0.5f;

    public float recoilOverTime_y = 0.5f;

    public float gunPrecision_notAiming = 200.0f;

    public float cameraZoomRatio_notAiming = 60;

    public float secondCameraZoomRatio_notAiming = 60;

    public float gunPrecision;

    public string reloadAnimationName = "Player_Reload";

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().transform;//mouseLook.transform;
        playerMovement = player.GetComponent<PlayerMovement>();
        mainCamera = playerMovement.cameraMain;//mouseLook.myCamera;
        secondCamera = GameObject.FindGameObjectWithTag("SecondCamera").GetComponent<Camera>();
        cameraComponent = mainCamera.GetComponent<Camera>();
        rotationLastY = playerMovement.currentYRotation;//mouseLook.currentYRotation;
        rotationLastX = playerMovement.currentCameraXRotation;//mouseLook.currentCameraXRotation;

    }

    void Update()
    {
        Animations();
        UpdateGunPosition();
        Shooting();
    }

    void FixedUpdate()
    {
        UpdateGunRotation();

        gunPrecision = gunPrecision_notAiming;
        recoilAmount_x = recoilAmount_x_non;
        recoilAmount_y = recoilAmount_y_non;
        recoilAmount_z = recoilAmount_z_non;
        currentGunPosition = Vector3.SmoothDamp(currentGunPosition, restPlacePosition, ref gunPosVelocity, gunAimTime);
        cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_notAiming, ref cameraZoomVelocity, gunAimTime);
        secondCamera.fieldOfView = Mathf.SmoothDamp(secondCamera.fieldOfView, secondCameraZoomRatio_notAiming, ref secondCameraZoomVelocity, gunAimTime);
    }

    void UpdateGunPosition()
    {
        transform.position = Vector3.SmoothDamp(transform.position,
            mainCamera.transform.position -
            (mainCamera.transform.right * (currentGunPosition.x + currentRecoilXPos)) +
            (mainCamera.transform.up * (currentGunPosition.y + currentRecoilYPos)) +
            (mainCamera.transform.forward * (currentGunPosition.z + currentRecoilZPos)), ref velV, 0);

        playerMovement.cameraPosition = new Vector3(currentRecoilXPos, currentRecoilYPos, 0);

        currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref velocity_z_recoil, recoilOverTime_z);
        currentRecoilXPos = Mathf.SmoothDamp(currentRecoilXPos, 0, ref velocity_x_recoil, recoilOverTime_x);
        currentRecoilYPos = Mathf.SmoothDamp(currentRecoilYPos, 0, ref velocity_y_recoil, recoilOverTime_y);
    }

    void UpdateGunRotation()
    {
        rotationDeltaY = playerMovement.currentYRotation - rotationLastY;
        rotationDeltaX = playerMovement.currentCameraXRotation - rotationLastX;

        rotationLastY = playerMovement.currentYRotation;
        rotationLastX = playerMovement.currentCameraXRotation;

        angularVelocityY = Mathf.Lerp(angularVelocityY, rotationDeltaY, Time.deltaTime * 5);
        angularVelocityX = Mathf.Lerp(angularVelocityX, rotationDeltaX, Time.deltaTime * 5);

        gunWeightX = Mathf.SmoothDamp(gunWeightX, playerMovement.currentCameraXRotation, ref velocityGunRotate.x, rotationLagTime);
        gunWeightY = Mathf.SmoothDamp(gunWeightY, playerMovement.currentYRotation, ref velocityGunRotate.y, rotationLagTime);

        transform.rotation = Quaternion.Euler(gunWeightX + (angularVelocityX * forwardRotationAmount.x), gunWeightY + (angularVelocityY * forwardRotationAmount.y), 0);
    }

    public void RecoilMath()
    {
        currentRecoilZPos -= recoilAmount_z;
        currentRecoilXPos -= (Random.value - 0.5f) * recoilAmount_x;
        currentRecoilYPos -= (Random.value - 0.5f) * recoilAmount_y;
        playerMovement.wantedCameraXRotation -= Mathf.Abs(currentRecoilYPos * gunPrecision);
        playerMovement.wantedYRotation -= (currentRecoilXPos * gunPrecision);
    }

    void Shooting()
    {
        if (currentStyle == GunStyles.semi)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GetBullet();
            }
        }
        if (currentStyle == GunStyles.automatic)
        {
            if (Input.GetButton("Fire1"))
            {
                GetBullet();
            }
        }
        waitTillNextFire -= roundsPerSecond * Time.deltaTime;
    }

    private void GetBullet()
    {
        if (waitTillNextFire <= 0 && !reloading)
        {

            if (bulletsInTheGun > 0)
            {
                if (bullet)
                    Instantiate(bullet, bulletSpawnPlace.transform.position, bulletSpawnPlace.transform.rotation);
                else
                    print("Missing the bullet prefab");

                RecoilMath();

                waitTillNextFire = 1;
                bulletsInTheGun -= 1;
            }
            else
            {
                StartCoroutine(Reload_Animation());
            }
        }
    }

    public float reloadChangeBulletsTime;
    IEnumerator Reload_Animation()
    {
        if (bulletsIHave > 0 && bulletsInTheGun < amountOfBulletsPerLoad && !reloading/* && !aiming*/)
        {
            handsAnimator.SetBool("reloading", true);
            yield return new WaitForSeconds(0.5f);
            handsAnimator.SetBool("reloading", false);

            yield return new WaitForSeconds(reloadChangeBulletsTime - 0.5f);//minus ovo vrijeme cekanja na yield

            if (bulletsIHave - amountOfBulletsPerLoad >= 0)
            {
                bulletsIHave -= amountOfBulletsPerLoad - bulletsInTheGun;
                bulletsInTheGun = amountOfBulletsPerLoad;
            }
            else if (bulletsIHave - amountOfBulletsPerLoad < 0)
            {
                float valueForBoth = amountOfBulletsPerLoad - bulletsInTheGun;
                if (bulletsIHave - valueForBoth < 0)
                {
                    bulletsInTheGun += bulletsIHave;
                    bulletsIHave = 0;
                }
                else
                {
                    bulletsIHave -= valueForBoth;
                    bulletsInTheGun += valueForBoth;
                }
            }
        }
    }
    public Animator handsAnimator;
    void Animations()
    {
        if (handsAnimator)
        {
            reloading = handsAnimator.GetCurrentAnimatorStateInfo(0).IsName(reloadAnimationName);

            handsAnimator.SetFloat("walkSpeed", playerMovement.currentSpeed);
            handsAnimator.SetInteger("maxSpeed", playerMovement.maxSpeed);
            if (Input.GetKeyDown(KeyCode.R) && !reloading)
            {
                StartCoroutine("Reload_Animation");
            }
        }
    }
}
