using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //[SerializeField] Transform groundcheckcollider;
    [SerializeField] LayerMask ground;
    private Rigidbody2D rgb;
    private combat combat;
    private CircleCollider2D groundChecker;
    public Animator animator;

    // for movement
    public float idle_time = 5;
    public float Movementspeed = 0;
    public float jumpForce = 1000;
    public float Speed = 10;
    public bool grounded = false;
    private bool facingRight = true;
    public bool isJump = false;
    public bool falling = false;

    // for animation
    private string currentState;
    private string idle = "idle";
    private string jump = "jump";
    private string run = "run";
    private string fight_pos = "fight_pos";
    private string fighttoidle = "FighttoIdle";
    private string down = "down";

    // for status
    public bool can_Move = true;

    // Start is called before the first frame update
    void Start()
    {
        combat = GetComponent<combat>();
        rgb = transform.GetComponent<Rigidbody2D>();
        groundChecker = transform.GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void IsGround()
    {
        grounded = false;

        Collider2D collider = Physics2D.OverlapBox(groundChecker.bounds.center, groundChecker.bounds.size, 0f, ground);
        if (collider != null)
        {
            grounded = true;
        }
    }

    public void IsFall()
    {
        falling = false;

        if (rgb.velocity.y < 0)
        {
            falling = true;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;

        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    // Update is called once per frame
    void Update()
    {
        //IsGround();

        Movementspeed = Input.GetAxisRaw("Horizontal") * Speed;
        idle_time -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            isJump = true;
        }
    }

    private void FixedUpdate()
    {
        IsGround();
        IsFall();

        if (Movementspeed != 0 && !combat.is_Attack) 
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);
            idle_time = 5;
            transform.position += new Vector3(Movementspeed * Time.fixedDeltaTime, 0, 0);
        }
        else if (Movementspeed == 0 && grounded)
        {
            animator.SetBool("isRunning", false);
            if (idle_time < 0)
            {
                animator.SetBool("isIdle", true);
            }
        }

        if (facingRight == false && Movementspeed > 0 && !combat.is_Attack)
        {
            Flip();
        }
        else if (facingRight == true && Movementspeed < 0 && !combat.is_Attack)
        {
            Flip();
        }

        if (isJump && grounded && !combat.is_Attack)
        {
            ChangeAnimationState(jump);
            idle_time = 5;
            rgb.AddForce(new Vector2(0, jumpForce));
        }
        isJump = false;

        if (rgb.velocity.y > 0 && !grounded)
        {
            ChangeAnimationState(jump);
        }
        if (falling && !grounded)
        {
            ChangeAnimationState(down);
        }
        else if (falling && grounded)
        {
            ChangeAnimationState(fight_pos);
        }
        //if (rgb.velocity.y == 0 && !combat.is_Attack)
        //{
        //    ChangeAnimationState(fight_pos);
        //} 
    }

    public void Status()
    {
        if (can_Move == false)
        {
            Movementspeed = 0;
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
