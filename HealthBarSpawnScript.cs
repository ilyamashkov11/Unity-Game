using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarSpawnScript : MonoBehaviour
{
    public GameObject healthBar;

    public GameObject instantiateHealthBar(GameObject enemy) {
        return Instantiate(healthBar, enemy.transform.position, new Quaternion(0, 0, 0, 0), GameObject.FindGameObjectWithTag("Canvas").transform);
    }
}
