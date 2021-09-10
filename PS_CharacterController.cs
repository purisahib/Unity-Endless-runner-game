using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_CharacterController : PS_AnimationController
{
    [SerializeField] private LayerMask groundLayers;
    [SerializeField, Range(0,10)] private float runSpeed = 3f;
    [SerializeField, Range(0,5)] private float jumpHeight = 2f;
    [SerializeField] private bool is_Jump = false;

    private float gravity = -50f;
    private CharacterController characterController;
    private Vector3 velocity;
    [SerializeField] private bool is_Grounded;
    private float horizontalInput;

    [SerializeField]
    private bool is_death = false;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = 1;

        // Face Forward
        //transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);
    }
    private void FixedUpdate() {
        if(!is_death){
            PlayerJump(); // JumpFun
            PlayerRun();  // RunFun
            ConvertLan(); //LanFun
        }
       
    }
    //-------------------------------------------------------------//
    private void PlayerJump(){
        // is Grounded
        is_Grounded = Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);

        //---------------------***--------------------------//
        if(is_Grounded && velocity.y < 0){
            velocity.y = 0;
        }else{
            // Add Gravity
            velocity.y += gravity * Time.deltaTime;
        }    

        //---------------------***--------------------------// 
        if(is_Grounded && is_Jump){/* Input.GetButtonDown("Jump")){*/
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            is_Jump = false;
        }
        //Vertical Velocity
        characterController.Move(velocity * Time.deltaTime);

        //---------------------***--------------------------//
        Jump_State(!is_Grounded);//Animation
    }
    //-------------------------------------------------------------//
    private void PlayerRun(){
        // RunState
        characterController.Move(new Vector3(0, 0, horizontalInput * runSpeed) * Time.deltaTime);

        //---------------------***--------------------------//
        Run_State(runSpeed);//Animation
    }
    //-------------------------------------------------------------//
    private void ConvertLan(){
        // LanConvert
        transform.position = new Vector3(m_CurrentLane, transform.position.y, transform.position.z);
        real_tean = transform.position;
    }
    //-------------------------------------------------------------//
    // tranfrorm the input manager
    // -------------------------------------------------------------//
    public void Jump_State(){
        // Jump State
        is_Jump = true;
    }
    //-------------------------------------------------------------//
    public void Slide_State(){
        Down_State();
        StartCoroutine(Set_Player_Height());
    }
    IEnumerator Set_Player_Height(){
        characterController.height = 1f;
        yield return new WaitForSeconds(0.7f);
        characterController.height = 2f;
    }
    //-------------------------------------------------------------//
    [SerializeField]
    private int m_CurrentLane =0;
    [SerializeField]
    private Vector3 real_tean;
    public void ChangeLane(int direction){
        // if (!m_IsRunning)
		// 	return;

        int targetLane = m_CurrentLane + direction;

        if (targetLane < -1 || targetLane > 1)
            // Ignore, we are on the borders.
            return;

        m_CurrentLane = targetLane;
    }
    //-------------------------------------------------------------//
    public void SetSpeed(float modifier){
        runSpeed = 3.0f + modifier;
    }

    // It is being called every timre out capsule hits spmething
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.point.z > transform.position.z + characterController.radius){
            Death();
        }
    }
    private void Death(){
        Debug.Log("Dead");
        GetComponent<PS_Score>().OnDeath();
    }
}
