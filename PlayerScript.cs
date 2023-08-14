using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    /**
        References
    */
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody; // a reference to the rigidBody component
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private projectileScript projectileScript;
    private Text text;
    private Text ammoText;

    /**
        Public (and serializable) Fields 
    */
    public int coinCount;
    public int current_ammo = 0;
    public int max_ammo = 25;
    public String directionFacing = "Left";
    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpForce = 5f;
    

    /**
        Private Fields
    */
    private bool onGround;
    private bool isAttackingBool;
    private int collisions;
    private int _health = 100;
    private float Move;
    private float airTime;
    private float hitRadius = 0.25f;
    private float attackRate = 2f;
    private float nextAttackTime = 0f;
    private float jumpRate = 2f;
    private float nextJumpTime = 0f;
    private String isJumping = "isJumping";
    private String isRunning = "isRunning";
    private String isAttacking = "isAttacking";

#region Initialising Methods
    void Awake() {
        coinCount = 0;
        current_ammo = 0;
        rigidBody.mass = 5;
        rigidBody.gravityScale = 2;
        rigidBody.angularVelocity = 0;
        rigidBody.freezeRotation = true;
    }

    // Start is called before the first frame update
    void Start() {
        text = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        ammoText = GameObject.FindGameObjectWithTag("Max Ammo Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        playerMove();
        playerAttack();
        animatePlayer();
        checkCollisions();
        setAmmoOnHUD();
    }

    void FixedUpdate() {
        airTime++;
        playerJump();
    }
#endregion

#region Movement
    /**
        A method the moves the player based on input
    */
    void playerMove() {
        // gets the input from the horizontal axis\ (-1 = left, 0 = no input, 1 = right) 
        Move = Input.GetAxisRaw("Horizontal"); 
        transform.position += new Vector3(Move * speed, rigidBody.velocity.y, 0f) * Time.deltaTime;
    }

    /**
        The method responsible for making a player jump
    */
    void playerJump() {
        // making sure you can only jump 2 times per second max
        if (Time.time >= nextJumpTime){ 
            if ((Input.GetButtonDown("Jump") || Input.GetButton("Jump")) && onGround) {
                rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                nextJumpTime = Time.time + 1f / jumpRate;
                airTime = 0;
            } 
        }
        if (!onGround && Input.GetButton("Jump") && airTime < 5) {
            rigidBody.AddForce(new Vector2(0f, jumpForce/1.5f), ForceMode2D.Impulse);
        }
    }
#endregion

#region Animations
    /**
        The method resposible for player animation switches
    */
    void animatePlayer() {
        setAttackPointSide();
        // for switching between running and idle animation
        if (Move > 0) {
            animator.SetBool(isRunning, true);
            spriteRenderer.flipX = true;
            directionFacing = "Right";
        } else if (Move < 0) { 
            animator.SetBool(isRunning, true); 
            spriteRenderer.flipX = false;
            directionFacing = "Left";
        } else {
            animator.SetBool(isRunning, false);
        }

        // for jumping animation
        if (!onGround) { animator.SetBool(isJumping, true); }
        else { animator.SetBool(isJumping, false); }
    }

    // for attack animation
    void animateAttack() {
        if (isAttackingBool) { 
            animator.SetBool(isAttacking, true); 
            //attack(); 
        }
        else { animator.SetBool(isAttacking, false); }
    }
#endregion

#region Collisions
    void OnCollisionEnter2D(Collision2D collision) {
        Vector2 contact = collision.GetContact(0).normal;
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("World Border Bottom"))) { // if the player has hit the ground
            //! make it detect only the top of the collider using the normal vector
            collisions++;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("World Border Bottom")) { // if the player has hit the ground
            collisions--;
            //Debug.Log("left ground: " + collisions);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            getEnemyAIScript(other).moveToPlayer();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            getEnemyAIScript(other).setAIRunning(false);
        }
    }
#endregion

#region Attack
    void playerAttack() {
        // ensuring the player cant attack more than 2 times per second
        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.X)) { 
                isAttackingBool = true; 
                animateAttack();
                nextAttackTime = Time.time + 1f / attackRate; 
            } 
            else { 
                isAttackingBool = false; 
                animateAttack();
            }
            if (Input.GetKeyDown(KeyCode.Z)) {
                if (current_ammo > 0) {
                    projectileScript.createProjectile(this.gameObject);
                    nextAttackTime = Time.time + 1f / attackRate; 
                    current_ammo -= 1;
                }
            }
        }
    }

    // only caled during atack animation
    void attack() {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, hitRadius, enemyLayers);

        foreach (Collider2D enemyHit in enemiesHit) {
            getEnemyAIScript(enemyHit).takeDamage(50);
        }
    }

    void setAttackPointSide() {
        if (directionFacing.Equals("Left")) {
            attackPoint.position = new Vector3(transform.position.x - 0.95f, transform.position.y + 0.25f, transform.position.z);
        } else if (directionFacing.Equals("Right")) {
            attackPoint.position = new Vector3(transform.position.x + 0.7f, transform.position.y + 0.25f, transform.position.z);
        }
    }
    #endregion

#region Helper
    void checkCollisions() {
        if (collisions == 1) { onGround = true; }
        else if (collisions == 0) { onGround = false; } 
    }

    public void increaseScoreOnHUD() {
        text.text = "" + coinCount;
    }

    public void setAmmoOnHUD() {
        ammoText.text = current_ammo + "/ 25";
    }

    EnemyAIScript getEnemyAIScript(Collider2D other) {
        return other.gameObject.GetComponent<EnemyAIScript>();
    }

    public void takeDamage(int damage) {
        _health -= damage;
    }
    #endregion
}
