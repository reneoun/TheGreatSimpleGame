using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    private float MovementSpeed;
    public float WalkingSpeed = 3;
    public float RunningSpeed = 4.5f;
    public float JumpForce = 5;
    public Animator animator;
    
    public Vector3 RespawnPoint = new Vector3(-12,-3,0);

    private Rigidbody2D _rigidbody;
    private Vector3 characterScale;
    private bool isNeutral = true;

    private bool doubleTap = false;
    private float doubleTapTime;

    private bool onGround = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public int extraJumps;

    public HealthBar healthBar;
    public int curHealth = 100;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        characterScale = transform.localScale;
        MovementSpeed = WalkingSpeed;

        healthBar.SetMaxHealth(100);

    }


    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        
        isNeutral = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ? false : true;
        if (!(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            // animator.SetBool("isRunning", false);
            MovementSpeed = WalkingSpeed;
        }
        animator.SetBool("neutral", isNeutral);

        if (onGround)
        {
            extraJumps = 1;
            animator.SetBool("isJumping", false);
        }

        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        animator.SetFloat("speed", Mathf.Abs(movement * MovementSpeed));
        //Debug.Log(Mathf.Abs(movement*MovementSpeed));
        // if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            _rigidbody.AddForce(new Vector2(0,JumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            animator.SetBool("isRunning", false);
            extraJumps--;
            Debug.Log("Jumping");
        }


        DoubleTapKey(KeyCode.LeftArrow);
        DoubleTapKey(KeyCode.RightArrow);
        
        if(Input.GetKeyDown(KeyCode.Z)) {
            animator.SetTrigger("attack");
        }

        characterScale.x = Input.GetAxis("Horizontal") < 0 ? -1 : characterScale.x;
        characterScale.x = Input.GetAxis("Horizontal") > 0 ? 1 : characterScale.x;
        transform.localScale = characterScale;

        if (transform.position.y < -4.5f) {
            transform.position =  RespawnPoint;
            takeDamage();
        }
    }

    private void DoubleTapKey(KeyCode key)
    {
        if (Input.GetKeyDown(key) && doubleTap)
        {
            if (Time.time - doubleTapTime < 0.5f)
            {
                Debug.Log("Double-tapped");
                // animator.SetBool("isRunning", true);
                MovementSpeed = RunningSpeed;
                doubleTapTime = 0f;
            }
            doubleTap = false;
        }

        if (Input.GetKeyDown(key) && !doubleTap)
        {
            doubleTap = true;
            doubleTapTime = Time.time;
        }


    }

    private void takeDamage() {
        curHealth -= 10;
        curHealth = curHealth < 0 ? 100 : curHealth;
        healthBar.SetHealth(curHealth);
    }
}
