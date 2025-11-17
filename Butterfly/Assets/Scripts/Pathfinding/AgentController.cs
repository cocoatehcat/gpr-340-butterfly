using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AgentController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float turnSpeed = 10f;
    public float arriveThreshold = 0.3f;

    private List<Vector3> path = new List<Vector3>();
    private int currentIndex = 0;
    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.height = 1.6f;
            controller.radius = 0.3f;
        }
    }

    //void Update()
    //{
    //    if (path == null || path.Count == 0) return;
    //    Vector3 target = path[currentIndex];
    //    Vector3 dir = (target - transform.position);
    //    dir.y = 0;
    //    float dist = dir.magnitude;
    //    if (dist < arriveThreshold)
    //    {
    //        currentIndex++;
    //        if (currentIndex >= path.Count)
    //        {
    //            path.Clear();
    //            return;
    //        }
    //        target = path[currentIndex];
    //        dir = (target - transform.position);
    //        dir.y = 0;
    //    }
    //    if (dir.sqrMagnitude > 0.001f)
    //    {
    //        Quaternion toRot = Quaternion.LookRotation(dir.normalized);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, toRot, Time.deltaTime * turnSpeed);
    //    }
    //    Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
    //    controller.Move(move);
    //}
    void Update()
    {
        if (path == null || path.Count == 0) return;

        Vector3 target = path[currentIndex];
        //Vector3 dir = (target - transform.position);
        Vector3 dir = (target - transform.position);
        dir.x = 0;
        float dist = dir.magnitude;

        // Reached a waypoint
        if (dist < arriveThreshold)
        {
            currentIndex++;
            if (currentIndex >= path.Count)
            {
                path.Clear();
                return;
            }
            target = path[currentIndex];
            dir = (target - transform.position);
        }

        // Rotate toward movement
        // if (dir.sqrMagnitude > 0.001f)
        if (dir.sqrMagnitude > 0.01f)
        {
            dir = dir.normalized;

            // rotate toward target
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRot, turnSpeed * Time.deltaTime);

            // move in the direction facing
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            Quaternion desiredRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * turnSpeed);
        }

        // Move forward
        Vector3 move = transform.forward * moveSpeed * Time.deltaTime;

        // Apply gravity so it stays on terrain
        move.y += -9.81f * Time.deltaTime;

        controller.Move(move);
    }


    public void SetPath(List<Vector3> worldWaypoints)
    {
        path = worldWaypoints;
        currentIndex = 0;
    }
}