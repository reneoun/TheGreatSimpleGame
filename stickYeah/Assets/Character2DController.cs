using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    public float MovementSpeed = 1;
    public float JumpForce = 1;
    public Animator animator;

    private Rigidbody2D _rigidbody;
    private Vector3 characterScale;
    private bool lookright = true;

    private bool onGround = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public int extraJumps;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        characterScale = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        extraJumps = onGround ? 1 : extraJumps;

        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        animator.SetFloat("speed", Mathf.Abs(movement));
        // if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            _rigidbody.AddForce(new Vector2(0,JumpForce), ForceMode2D.Impulse);
            extraJumps--;
        }
        

        characterScale.x = Input.GetAxis("Horizontal") < 0 ? -1 : characterScale.x;
        characterScale.x = Input.GetAxis("Horizontal") > 0 ? 1 : characterScale.x;
        transform.localScale = characterScale;
    }
}
