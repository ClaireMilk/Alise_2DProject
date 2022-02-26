using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetToFollow;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothfactor;
    private void FixedUpdate()
    {
        FollowThePlayer();
    }
    void FollowThePlayer()
    {
        Vector3 targetposition = targetToFollow.position + offset;
        Vector3 smoothposition = Vector3.Lerp(transform.position, targetposition, smoothfactor * Time.fixedDeltaTime);
        transform.position = smoothposition;
    }
}
