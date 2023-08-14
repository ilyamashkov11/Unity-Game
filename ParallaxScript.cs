using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{

    [SerializeField][Range(0f, 1f)] private float parallax;
    private Vector3 previousCameraPos;
    private Vector3 targetPos;
    private Transform _camera;

    void Awake() {
        _camera = Camera.main.transform;
        previousCameraPos = _camera.position;
    }

    // Start is called before the first frame update
    void LateUpdate() {
        Vector3 movement = cameraMovement;
        if (movement == Vector3.zero) return;
        targetPos = new Vector3(transform.position.x - movement.x * parallax, transform.position.y - movement.y * parallax, transform.position.z); 
        transform.position = targetPos;
        
    }

    private Vector3 cameraMovement {
        get {
            Vector3 movement = _camera.position - previousCameraPos;
            previousCameraPos = _camera.position;
            return movement;
        }
    } 
}
