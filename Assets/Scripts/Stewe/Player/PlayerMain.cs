using UnityEngine;
using UnityEngine.Events;


public class PlayerMain : MonoBehaviour,IHealth
{
    //Player parameters
    [SerializeField]
    internal int healthPoints;
    internal int maxHp;

    public float speed;
    public float playerHeight;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float waterMultiplier;
    public float rotationSpeed;

    bool isReadyToJump = true;
    bool isSwimming = false;
    bool isGrounded;

    public LayerMask groundLayer;

    public Transform gun;
    public Transform cam;
    public Transform waterSurface;

    public Rigidbody rb;

    float horizontalInput;
    float verticalInput;

    Vector3 direction;
    Quaternion rotDir;
    Vector3 forward;
    Vector3 right;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        rb = this.gameObject.GetComponent<Rigidbody>();
        maxHp = healthPoints;
    }

    private void Update()
    {
        PlayerInput();
        LimitSpeed();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, groundLayer);
        if (Input.GetKey(KeyCode.M))
        {
            this.transform.Translate(Vector3.up * speed * Time.deltaTime* 10);
        }
        if (Input.GetKey(KeyCode.N))
        {
            this.transform.Translate(Vector3.down * speed * Time.deltaTime * 10);
        }
        if (transform.position.y <= waterSurface.position.y)
        {
            isSwimming = true;
            rb.useGravity = false;
        }
        else
        {
            isSwimming = false;
            rb.useGravity = true;
        }

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space) && isReadyToJump && isGrounded)
        {
            isReadyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
            Jump();
        }
    }

    private void Move()
    {
        forward = cam.forward;
        right = cam.right;

        forward.Normalize();
        right.Normalize();

        //player rotation
        if (!isSwimming)
        {

            forward.y = 0;
            right.y = 0;

            direction = forward * verticalInput + right * horizontalInput;

            if (isGrounded)
                rb.AddForce(direction.normalized * speed, ForceMode.Force);
            else if (!isGrounded)
                rb.AddForce(direction.normalized * speed * airMultiplier, ForceMode.Force);


            rotDir = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        }
        else
        {
            direction = forward * verticalInput + right * horizontalInput;

            rb.AddForce(direction.normalized * speed * waterMultiplier, ForceMode.Force);

            rotDir = Quaternion.Euler(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);
            rb.velocity *= direction.normalized.magnitude;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, rotationSpeed);

        //gun rotation
        Quaternion gunRotDir = Quaternion.Euler(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);
        gun.rotation = Quaternion.Slerp(gun.rotation,gunRotDir,rotationSpeed);


    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    private void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVelocity.magnitude > speed)
        {
            Vector3 LimitedVelocity = flatVelocity.normalized * speed;
            rb.velocity = new Vector3(LimitedVelocity.x, rb.velocity.y, LimitedVelocity.z);
        }
    }

    public void OnRecoverHealth(int hp)
    {
        healthPoints = (int)Mathf.Clamp((float)(healthPoints + hp),1f,(float)maxHp);    // adds hp within the declared limits (from 1 to MaxHp)
    }

    public void OnGetDamage(int dmg)
    {
        // animation, sounds and lots of further improvements have to be made

        if(healthPoints - dmg <= 0)
        {
            OnHealthOver();
        }
        else
        {
            healthPoints -= dmg;
            Debug.Log("player damaged : " + healthPoints);
        }
    }

    public void OnHealthOver()
    {
        Debug.Log("player dead !");
        /* basically Game Over so anything related that concerns the player
           and maybe even a unity event that comunicates with the game controller must be added here*/
    }
}
