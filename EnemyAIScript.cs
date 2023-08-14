using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAIScript : MonoBehaviour
{

    /**
        References
    */
    /**
        Public / Serialized fields
    */

    /**
        Private fields
    */
    private PlayerScript player;
    private Slider slider;
    private Image healthBarFill;
    private GameObject healthBar;
    public Gradient gradient;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [SerializeField] private float enemySpeed = 2f;
    private Vector3 healthBarOffset = new Vector3(0f, 1f, 0f);
    private Vector3 playerPos; // position of the player
    private string isRunning = "isRunning";
    private bool dead = false;
    private int health = 200;
    int layer;

    // Start is called before the first frame update
    void Start() {
        rb.freezeRotation = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        layer = LayerMask.NameToLayer("Dead Enemy");
        
        healthBar = GameObject.FindGameObjectWithTag("HealthBar Spawner").GetComponent<HealthBarSpawnScript>().instantiateHealthBar(gameObject);
        slider = healthBar.GetComponent<Slider>();
        //healthBarFill = GameObject.FindGameObjectWithTag("fill image").GetComponent<Image>();
        healthBarFill = healthBar.GetComponentInChildren<Image>();

        slider.value = health;
        healthBarFill.color = gradient.Evaluate(slider.normalizedValue);
    }

    // Update is called once per frame
    void LateUpdate() {
        playerPos = player.transform.position;
        healthBar.transform.position = gameObject.transform.position + new Vector3(0f, 1.25f, 0f);
        //healthBar.transform.position = transform.position + healthBarOffset;
        if (health <= 0) { 
            dead = true; 
            die(); 
        }
    }

    public void moveToPlayer() {
        if (!dead) {
        int dir = directionToPlayer();
        if (dir == -1) spriteRenderer.flipX = false; 
        else spriteRenderer.flipX = true;
        animator.SetBool(isRunning, true);
        transform.position += new Vector3(dir * enemySpeed * 1.5f, 0f, 0f) * Time.deltaTime;     
        }  
    }

    int directionToPlayer() {
        float diff = playerPos.x - transform.position.x;
        if (diff < 0) return -1;
        else return 1;
    }

    public void setAIRunning(bool status) {
        animator.SetBool(isRunning, status);
    }

    public void takeDamage(int damage) {
        setAIRunning(false);
        animator.SetTrigger("Hurt");
        health -= damage;
        slider.value = health;
        healthBarFill.color = gradient.Evaluate(slider.normalizedValue);
    }

    void die() {
        Destroy(healthBar);
        animator.SetTrigger("Dead");
        gameObject.layer = layer;
        Vector3 offset = new Vector3(0f, 0.15f, 0f);
        GetComponent<BoxCollider2D>().offset = offset;
        this.enabled = false;
    } 
}
 