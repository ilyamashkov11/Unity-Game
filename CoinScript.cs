using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private PlayerScript player;

    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update() {
        // just a counter to make the ocin bob up and down
        if (!gameObject.CompareTag("ScreenCoin") && counter <= 2100) { counter++; }
        else { counter = 0; }
        coinBob();
    }

    void OnTriggerEnter2D(Collider2D other) {
        double dist = Math.Sqrt(Math.Pow(player.transform.position.x - transform.position.x, 2));
        if (other.gameObject.CompareTag("Player") && !gameObject.CompareTag("ScreenCoin") 
            && dist < 1) {
            player.coinCount++;
            player.increaseScoreOnHUD();
            Destroy(gameObject);
        }
    }

    void coinBob() {
        if (counter == 1550) transform.position += new Vector3(0f, 0.5f, 0f);
        else if (counter == 2000) transform.position += new Vector3(0f, -0.5f, 0f);
    }
}
