using System;
using UnityEngine;

public class FollowPlayerBehavior : MonoBehaviour
{
    public Transform playerTransform;
    Transform cameraTransform;
    Vector3 target;
    float lastUpdateTime;

    void Start(){
        lastUpdateTime = Time.time;
        cameraTransform = GetComponent<Transform>();
        target = new Vector3(playerTransform.position.x, playerTransform.position.y, cameraTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        var currTime = Time.time;
        cameraTransform.position = Vector3.Lerp(transform.position, target, (currTime - lastUpdateTime) / .05f);

        if((currTime - lastUpdateTime) < .05f)
            return;
        
        Debug.Log("Updating target");
        target = new Vector3(playerTransform.position.x, playerTransform.position.y, cameraTransform.position.z);
        lastUpdateTime = currTime;
    }
}
