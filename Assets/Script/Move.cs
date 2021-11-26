using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class Move : NetworkBehaviour
{
    public Rigidbody rb;
    public GameObject cameraRoot;

    public static float turnSpeed = 1.0f;
    public float moveSpeed = 3.0f;

    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;
    private float rotX;

    private Joystick joystickMove;
    private Joystick joystickAim;

    public float verticalMove;
    public float horizontalMove;

    public float verticalAim;
    public float horizontalAim;

    [Client]
    private void Start()
    {
        if (isLocalPlayer)
        {
            joystickMove = GameObject.FindGameObjectWithTag("LeftJS").GetComponent<Joystick>();
            joystickAim = GameObject.FindGameObjectWithTag("RightJS").GetComponent<Joystick>();
            var cam = GameObject.FindGameObjectWithTag("CinemachineTarget").GetComponent<CinemachineVirtualCamera>();
            cam.Follow = cameraRoot.transform;
        }
    }



    [Client]
    void Update()
    {
        if (!hasAuthority) return;

        verticalMove = joystickMove.Vertical;
        horizontalMove = joystickMove.Horizontal;

        verticalAim = joystickAim.Vertical;
        horizontalAim = joystickAim.Horizontal;

        MouseAiming();
        KeyboardMovement();
    }

    void MouseAiming()
    {
        // get the mouse inputs
        float y = horizontalAim * turnSpeed;
        rotX += verticalAim * turnSpeed;

        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

        // rotate the camera
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y + y, 0.0f);
        cameraRoot.transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y, 0.0f);
    }

    void KeyboardMovement()
    {
        Vector3 dir = new Vector3(0, 0, 0);

        dir.x = horizontalMove;
        dir.z = verticalMove;

        transform.Translate(dir * moveSpeed * Time.deltaTime);
        //rb.AddForce(dir * moveSpeed * Time.deltaTime);
    }
}