using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    private PlayerScript playerScript;

    private Vector3 CameraPos; // a field to store the camera position
    private float maxX = 97, maxY = 39, minX = -22, minY = -27; // the constraints for where the camera can go

    void Start() {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();    
    }

    void LateUpdate() {
        float posX = playerScript.transform.position.x;
        float posY = playerScript.transform.position.y;
        if (posX >= minX && posY >= minY && posX <= maxX && posY <= maxY) { 
                moveInXDir();
                moveInYDir(); 
        } else if (posX >= minX && posX <= maxX && (posY < minY || posY > maxY)) {
                moveInXDir();
        } else if (posY >= minY && posY <= maxY && (posX < minX || posX > maxX)) {
                moveInYDir();
        }
    }

    void moveInXDir() {
        CameraPos = transform.position;
        CameraPos.x = playerScript.transform.position.x;
        transform.position = CameraPos;
    }

    void moveInYDir() {
        CameraPos = transform.position;
        CameraPos.y = playerScript.transform.position.y;
        transform.position = CameraPos;
    } 
}
