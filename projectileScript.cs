using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    [SerializeField] private float bullet_velocity;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Rigidbody2D rb;
    private PlayerScript playerScript;
    private Transform worldCenter;
    private int dir;

    // Start is called before the first frame update
    void Start()
    {
        projectile.transform.localScale = new Vector3(0.15f, 0.15f, 0);
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        dir = playerScript.directionFacing == "Left" ? -1 : 1; 
        worldCenter = GameObject.FindGameObjectWithTag("World Center").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fly();
    }

    private void fly() {
        Vector3 placeholder = this.transform.position;
        placeholder.x += bullet_velocity * dir * Time.deltaTime;
        this.transform.position = placeholder;
    }

    public void createProjectile(GameObject player) {
        Instantiate(projectile, player.transform.position + new Vector3(0.167f * dir, 0.471f, 0), new Quaternion(0,0,0,0), worldCenter);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(this.gameObject);
        if (other.gameObject.CompareTag("Enemy")) {
            other.gameObject.GetComponent<EnemyAIScript>().takeDamage(50);
        }
    }
}
