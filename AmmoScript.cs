using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AmmoScript : MonoBehaviour
{
    private PlayerScript playerScript;
    // Start is called before the first frame update
    void Start() {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
            double dist = Math.Sqrt(Math.Pow(playerScript.transform.position.x - transform.position.x, 2));
        if (other.CompareTag("Player")) {
            if (other.gameObject.CompareTag("Player") && dist < 1) {
                playerScript.current_ammo = playerScript.max_ammo;
                Destroy(gameObject);
            }
        }
    }
}
