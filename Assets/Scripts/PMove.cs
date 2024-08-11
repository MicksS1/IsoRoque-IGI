using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PMove : MonoBehaviour
{
    [Header("Refs")]
    public Rigidbody player;
    public Animator anim;

    [Header("Movement")]
    public float moveSpeed;
    //public float moveSpeedMax;
    //public float moveSpeedMin;
    public float rotSpeed;
    public float dashSpeed;
    public float dashCD;
    public float acc;

    [Header("Speed")]
    [SerializeField]private float speedX;
    [SerializeField]private float speedZ;

    public bool isDashing;
    private bool CD;

    private Vector3 move;
    private Vector3 isoMove;
    private Vector3 dashDir;
    //private Vector3 orVel;
    private Vector3 targPos;
    private Vector3 currVel;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();
        isDashing = false;
        CD = false;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            anim.SetTrigger("run");
        else
            anim.SetTrigger("idle");
            
        // dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !CD)
        {
            isDashing = true;
            moveSpeed = moveSpeed * dashSpeed;
            Invoke("dashReset", 0.2f);

            CD = true;
            Invoke("coolDown", dashCD + 0.2f);
        }

        speedX = player.velocity.x;
        speedZ = player.velocity.z;
    }

    private void FixedUpdate()
    {
        // isometric movement
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        isoMove = matrix.MultiplyPoint3x4(move);

        // character movement
        //player.velocity = new Vector3(isoMove.x * moveSpeed, player.velocity.y, isoMove.z * moveSpeed);

        currVel = player.velocity;
        targPos = new Vector3(isoMove.x * moveSpeed, player.velocity.y, isoMove.z * moveSpeed);

        player.velocity = Vector3.SmoothDamp(player.velocity, targPos, ref currVel, acc);

        // rotation
        if (move != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(isoMove, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
        
        } 
        //else if (move == Vector3.zero)
        //    player.velocity = Vector3.zero;
    }

    private void dashReset()
    {
        moveSpeed = moveSpeed / dashSpeed;
        isDashing = false;
    }

    private void coolDown()
    {
        CD = false;
    }
 }
