using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region components

    [HideInInspector] public GameManager gameManager;

    [HideInInspector] public PlayerInput playerInput;
    #region input actions

    [HideInInspector] public InputAction 
        movement, 
        look,
        jump, 
        sprint, 
        grapple, 
        pause, 
        slowMotionAim;

    #endregion

    [HideInInspector] public Rigidbody body;
    [HideInInspector] public CapsuleCollider capsule;

    #endregion

    #region inspector
    
    public MovementPhysicsObj movementPhyObj;
    public GrapplingPhysicsObj grapplingPhysicsObj;

    public Transform grapplingHandObj;
    
    public PlayerPreferencesObj playerPreferencesObj;
    public UserInterfaceObj userInterfaceObj;

    #endregion

    #region variables

    //states
    [HideInInspector] public bool 
        actionStarted = false, 
        isGrounded, 
        doubleJumpReady, 
        isSprinting, 
        jumpCooldown, 
        isSlowed;
    
    [HideInInspector] public float slowT;

    //grappling
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpringJoint grappleJoint;
    [HideInInspector] public Transform grappleHookObj;
    [HideInInspector] public LineRenderer grappleLineRenderer;

    [HideInInspector] public float grappleScale, grappleScaleSpeed;

    //air movement
    [HideInInspector] public Vector3 airStrafeSpeed;

    //ground
    [HideInInspector] public Vector3 groundNormal;
    [HideInInspector] public Collider groundCollider;
    [HideInInspector] public string groundTypeTag;

    //camera
    [HideInInspector] public float camFollowVelocity;
    [HideInInspector] public Transform camFollowTargetTransform;
    [HideInInspector] public Image reticle;

    [HideInInspector] public Vector3 respawnPoint;

    #endregion


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        #region input actions

        movement = playerInput.actions["Movement"];

        look = playerInput.actions["Look"];

        pause = playerInput.actions["Pause"];
        pause.started += obj => gameManager.TogglePause();

        jump = playerInput.actions["Jump"];
        jump.started += obj => MovementPhysicsController.Jump(this);

        sprint = playerInput.actions["Sprint"];
        sprint.started += obj => MovementPhysicsController.SprintPerformed(this);

        grapple = playerInput.actions["Grapple"];
        grapple.started += obj => GrapplingPhysicsController.StartGrapple(this);
        grapple.canceled += obj => GrapplingPhysicsController.CancelGrapple(this);

        slowMotionAim = playerInput.actions["SlowMotionAim"];
        slowMotionAim.started += obj => SlowMotionController.StartSlowMotionCoroutine(this);
        slowMotionAim.canceled += obj => SlowMotionController.StopSlowMotionCoroutine(this);
        
        #endregion

        gameManager = FindObjectOfType<GameManager>();

        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        grappleLineRenderer = GetComponent<LineRenderer>();

        camFollowTargetTransform = transform.Find("Follow Target").transform;

        reticle = gameManager.transform.Find("Canvas").Find("New Reticle Obj").Find("Base").Find("Addapting Shape").GetComponent<Image>();

        respawnPoint = transform.position;

        SlowMotionController.SetUpVolumeVignette(this);
    }

    private void Update()
    {
        float camFollowTargetY = Mathf.SmoothDamp(camFollowTargetTransform.localPosition.y, body.velocity.y / 50, ref camFollowVelocity, 0.02f);
        camFollowTargetTransform.localPosition = new Vector3(0, camFollowTargetY, 0);

        MovementPhysicsController.Movement(this);

        UserInterfaceController.UpdateReticle(this);

        if (transform.position.y <= gameManager.minLevelYKillLevel || transform.position.y >= gameManager.maxLevelYKillLevel) Die();
    }

    private void LateUpdate()
    {
        camFollowTargetTransform.rotation = Camera.main.transform.rotation;
        GrapplingPhysicsController.DrawLine(this);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isGrounded && !jumpCooldown)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y >= 0.5f)
                {
                    isGrounded = true;
                    doubleJumpReady = true;
                    PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimTriggerStates.Landed, animator);
                }
            }
        }

        if (collision.contacts[0].normal.y >= 0.5f)
        {
            groundNormal = collision.contacts[0].normal;
            groundTypeTag = collision.contacts[0].otherCollider.tag;
            groundCollider = collision.contacts[0].otherCollider;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isGrounded && !jumpCooldown)
        {
            StartCoroutine(CoyoteTime());
            IEnumerator CoyoteTime()
            {
                yield return new WaitForSeconds(movementPhyObj.coyoteTime);

                RaycastHit hit;

                if (!Physics.Raycast(transform.position, -transform.up, out hit, capsule.height / 1.75f) || !(hit.normal.y >= 0.5f))
                {
                    isGrounded = false;
                    groundCollider = null;
                }
            }
        }

        if (transform.parent != gameManager.transform)
        {
            transform.parent = gameManager.transform;
        }
    }

    public RaycastHit ReticleTarget()
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit cameraHit);
        if (!cameraHit.collider) return new RaycastHit();
        
        Physics.Raycast(transform.position, cameraHit.point - body.position, out RaycastHit grappleTarget);

        Debug.DrawLine(Camera.main.transform.position, cameraHit.point, Color.cyan);
        Debug.DrawLine(transform.position, grappleTarget.point, Color.yellow);

        return grappleTarget;
    }

    public void Die()
    {
        gameManager.deathCount++;
        transform.position = respawnPoint;
        body.velocity = Vector3.zero;
    }
}