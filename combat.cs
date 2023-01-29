using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{
    private Movement movement;
    private Rigidbody2D rgb;
    public Animator animator;

    // for combat system
    public bool attack_pressed = false;
    public bool is_Attack;
    public float attack_cooldown = 0.9f;
    public float timeBTtwAttack = 0;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public float attackRange;
    public LayerMask enemies;
    public int damage;

    // for animation
    private string currentState;
    private string attack1 = "attack1";
    private string fight_pos = "fight_pos";
    private string attack1_trans = "attack1_trans";

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        rgb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.IsGround();

        if (attack_cooldown <= 0)  // prevent player spamming attack
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                movement.idle_time = 5;
                attack_pressed = true;
                attack_cooldown = 0.9f;
            }
        }
        else
        {
            attack_cooldown -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected() // for hitbox
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private void FixedUpdate()
    {
        if (attack_pressed)
        {
            attack_pressed = false;
            if (!is_Attack)
            {
                is_Attack = true;
                movement.ChangeAnimationState(attack1);
                Collider2D[] hitbox = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemies);
                foreach (Collider2D enemy in hitbox)
                {
                    enemy.GetComponent<enemy_test>().take_damage(damage);
                }
            }
            float attackDelay = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke("resetAttack", attackDelay);
        }
    }

    void resetAttack()
    {
        is_Attack = false;
        movement.ChangeAnimationState(attack1_trans);
    }
}
