using UnityEngine;
using System.Collections;
//using UnityStandardAssets.ImageEffects;

public enum GunStyles
{
    nonautomatic, automatic
}
public class GunScript : MonoBehaviour
{
    public GunStyles currentStyle;
    public MouseLookScript mls;
    public int walkingSpeed = 3;
    public float bulletsIHave = 20;
    public float bulletsInTheGun = 5;
    public float amountOfBulletsPerLoad = 5;
    private Transform player;
    private Camera cameraComponent;
    private PlayerMovement pmS;

    /*
	 * Collection the variables upon awake that we need.
	 */
    void Awake()
    {
        mls = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLookScript>();
        player = mls.transform;
        mainCamera = mls.myCamera;
        secondCamera = GameObject.FindGameObjectWithTag("SecondCamera").GetComponent<Camera>();
        cameraComponent = mainCamera.GetComponent<Camera>();
        pmS = player.GetComponent<PlayerMovement>();
        rotationLastY = mls.currentYRotation;
        rotationLastX = mls.currentCameraXRotation;

    }

    public Vector3 currentGunPosition;
    public Vector3 restPlacePosition;
    public Vector3 aimPlacePosition;
    public float gunAimTime = 0.1f;
    public bool reloading;
    private Vector3 gunPosVelocity;
    private float cameraZoomVelocity;
    private float secondCameraZoomVelocity;

    void Update()
    {
        Animations();

        GiveCameraScriptMySensitvity();

        PositionGun();

        Shooting();
    }

    void FixedUpdate()
    {
        RotationGun();

        gunPrecision = gunPrecision_notAiming;
        recoilAmount_x = recoilAmount_x_non;
        recoilAmount_y = recoilAmount_y_non;
        recoilAmount_z = recoilAmount_z_non;
        currentGunPosition = Vector3.SmoothDamp(currentGunPosition, restPlacePosition, ref gunPosVelocity, gunAimTime);
        cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_notAiming, ref cameraZoomVelocity, gunAimTime);
        secondCamera.fieldOfView = Mathf.SmoothDamp(secondCamera.fieldOfView, secondCameraZoomRatio_notAiming, ref secondCameraZoomVelocity, gunAimTime);
        //}

    }

    [Header("Sensitvity of the gun")]
    [Tooltip("Sensitvity of this gun while not aiming.")]
    public float mouseSensitvity_notAiming = 10;
    //[HideInInspector]
    [Tooltip("Sensitvity of this gun while aiming.")]
    public float mouseSensitvity_aiming = 5;
    //[HideInInspector]
    [Tooltip("Sensitvity of this gun while running.")]
    public float mouseSensitvity_running = 4;
    /*
	 * Used to give our main camera different sensivity options for each gun.
	 */
    void GiveCameraScriptMySensitvity()
    {
        mls.mouseSensitvity_notAiming = mouseSensitvity_notAiming;
        mls.mouseSensitvity_aiming = mouseSensitvity_aiming;
    }

    private Vector3 velV;
    public Transform mainCamera;
    private Camera secondCamera;
    /*
	 * Calculatin the weapon position accordingly to the player position and rotation.
	 * After calculation the recoil amount are decreased to 0.
	 */
    void PositionGun()
    {
        transform.position = Vector3.SmoothDamp(transform.position,
            mainCamera.transform.position -
            (mainCamera.transform.right * (currentGunPosition.x + currentRecoilXPos)) +
            (mainCamera.transform.up * (currentGunPosition.y + currentRecoilYPos)) +
            (mainCamera.transform.forward * (currentGunPosition.z + currentRecoilZPos)), ref velV, 0);

        pmS.cameraPosition = new Vector3(currentRecoilXPos, currentRecoilYPos, 0);

        currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref velocity_z_recoil, recoilOverTime_z);
        currentRecoilXPos = Mathf.SmoothDamp(currentRecoilXPos, 0, ref velocity_x_recoil, recoilOverTime_x);
        currentRecoilYPos = Mathf.SmoothDamp(currentRecoilYPos, 0, ref velocity_y_recoil, recoilOverTime_y);
    }


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
    /*
	* Rotatin the weapon according to mouse look rotation.
	* Calculating the forawrd rotation like in Call Of Duty weapon weight
	*/
    void RotationGun()
    {
        rotationDeltaY = mls.currentYRotation - rotationLastY;
        rotationDeltaX = mls.currentCameraXRotation - rotationLastX;

        rotationLastY = mls.currentYRotation;
        rotationLastX = mls.currentCameraXRotation;

        angularVelocityY = Mathf.Lerp(angularVelocityY, rotationDeltaY, Time.deltaTime * 5);
        angularVelocityX = Mathf.Lerp(angularVelocityX, rotationDeltaX, Time.deltaTime * 5);

        gunWeightX = Mathf.SmoothDamp(gunWeightX, mls.currentCameraXRotation, ref velocityGunRotate.x, rotationLagTime);
        gunWeightY = Mathf.SmoothDamp(gunWeightY, mls.currentYRotation, ref velocityGunRotate.y, rotationLagTime);

        transform.rotation = Quaternion.Euler(gunWeightX + (angularVelocityX * forwardRotationAmount.x), gunWeightY + (angularVelocityY * forwardRotationAmount.y), 0);
    }

    private float currentRecoilZPos;
    private float currentRecoilXPos;
    private float currentRecoilYPos;
    /*
	 * Called from ShootMethod();, upon shooting the recoil amount will increase.
	 */
    public void RecoilMath()
    {
        currentRecoilZPos -= recoilAmount_z;
        currentRecoilXPos -= (Random.value - 0.5f) * recoilAmount_x;
        currentRecoilYPos -= (Random.value - 0.5f) * recoilAmount_y;
        mls.wantedCameraXRotation -= Mathf.Abs(currentRecoilYPos * gunPrecision);
        mls.wantedYRotation -= (currentRecoilXPos * gunPrecision);

        //expandValues_crosshair += new Vector2(6, 12);

    }

    [Header("Shooting setup - MUSTDO")]
    public GameObject bulletSpawnPlace;
    [Tooltip("Bullet prefab that this waepon will shoot.")]
    public GameObject bullet;
    [Tooltip("Rounds per second if weapon is set to automatic rafal.")]
    public float roundsPerSecond;
    private float waitTillNextFire;
    /*
	 * Checking if the gun is automatic or nonautomatic and accordingly runs the ShootMethod();.
	 */
    void Shooting()
    {
        if (currentStyle == GunStyles.nonautomatic)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootMethod();
            }
        }
        if (currentStyle == GunStyles.automatic)
        {
            if (Input.GetButton("Fire1"))
            {
                ShootMethod();
            }
        }
        waitTillNextFire -= roundsPerSecond * Time.deltaTime;
    }


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

    public float secondCameraZoomRatio_aiming = 40;

    public float gunPrecision;


    private void ShootMethod()
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
    /*
	* Fetching if any current animation is running.
	* Setting the reload animation upon pressing R.
	*/
    void Animations()
    {

        if (handsAnimator)
        {

            reloading = handsAnimator.GetCurrentAnimatorStateInfo(0).IsName(reloadAnimationName);

            handsAnimator.SetFloat("walkSpeed", pmS.currentSpeed);
            handsAnimator.SetBool("aiming", Input.GetButton("Fire2"));
            handsAnimator.SetInteger("maxSpeed", pmS.maxSpeed);
            if (Input.GetKeyDown(KeyCode.R) && pmS.maxSpeed < 5 && !reloading)
            {
                StartCoroutine("Reload_Animation");
            }
        }

    }
    public string reloadAnimationName = "Player_Reload";
    public string aimingAnimationName = "Player_AImpose";
    public string meeleAnimationName = "Character_Malee";
}
