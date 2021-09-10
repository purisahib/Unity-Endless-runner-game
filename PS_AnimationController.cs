using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void Awake() {
        animator = gameObject.GetComponent<Animator>();
    }
    protected void Run_State(float input){
        // Player is run
        animator.SetFloat("RunSpeed", input);
    }
    protected void Jump_State(bool input){
        // Player is Jump
        animator.SetBool("Jump", input);
    }
    protected void Down_State(){
        // Down to player
        animator.SetTrigger("Down");
    }
    private void Death_State(){
        // Player Death State
        animator.SetTrigger("Death");
    }
    private void Heart_State(){
        // Player Heart State
        animator.SetTrigger("Heart");
    }
}
