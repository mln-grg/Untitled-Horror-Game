﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 4f;
    [SerializeField] private float groundDistance = 0.4f;

    public Light flashlight;
    public GameObject Beam;
    public Spawn_LittleGirl s;
    private float gravity = -9.8f;
    private Vector3 velocity;
    private bool isGrounded;

    public bool isMoving = false;

    private bool flashlightEnabled = false;
    public AudioSource click;
    public bool checkForGirl = false;
    private void Update()
    {
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        if (Mathf.Abs(X) + Mathf.Abs(Z) > 0)
            isMoving = true;
        else
            isMoving = false;

        Vector3 move = transform.right * X + transform.forward * Z;
        float speed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed = sprintSpeed;
        else
            speed = walkSpeed;
        controller.Move(move * speed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(X) + Mathf.Abs(Z));

        //Gravity Simulation
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //Debug.Log(controller.transform.position);

        //flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            click.Play();
            flashlightEnabled = !flashlightEnabled;
            flashlight.enabled = flashlightEnabled;
            var vlb = Beam.GetComponent<VLB.VolumetricLightBeam>();
            vlb.enabled = flashlightEnabled;
            if (checkForGirl)
            {
                s.done = true; 
               checkForGirl = false;
            }
        }
    }

}
