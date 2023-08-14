using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideColliderScript : MonoBehaviour
{
    private EnemyAIScript enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            getEnemyAIScript(other).setAIRunning(false);
        }
    }

    EnemyAIScript getEnemyAIScript(Collision2D other) {
        return other.gameObject.GetComponent<EnemyAIScript>();
    }
}
