using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_CharacterInputController : MonoBehaviour
{
    [Header("PS_CharacterController"), SerializeField]
    private PS_CharacterController pS_CharacterController;

	[Header("Sounds")]
	public AudioClip slideSound;
	public AudioClip powerUpUseSound;
	public AudioSource powerupSource;
    private void Awake() {
        pS_CharacterController = gameObject.GetComponent<PS_CharacterController>();
    }
    
#if !UNITY_STANDALONE
    protected Vector2 m_StartingTouch;
	protected bool m_IsSwiping = false;
#endif

    protected void Update ()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // Use key input in editor or standalone
        // disabled if it's tutorial and not thecurrent right tutorial level (see func TutorialMoveCheck)

        if (Input.GetKeyDown(KeyCode.LeftArrow) )//&& TutorialMoveCheck(0))
        {
            pS_CharacterController.ChangeLane(-1);
            //ChangeLane(-1);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) )//&& TutorialMoveCheck(0))
        {
            pS_CharacterController.ChangeLane(1);
            //ChangeLane(1);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) )//&& TutorialMoveCheck(1))
        {
            pS_CharacterController.Jump_State();
            //Jump();
        }
		else if (Input.GetKeyDown(KeyCode.DownArrow) )//&& TutorialMoveCheck(2))
		{
            pS_CharacterController.Slide_State();
			//if(!m_Sliding)
				//Slide();
		}
#else
        // Use touch input on mobile
        if (Input.touchCount == 1)
        {
			if(m_IsSwiping)
			{
				Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;

				// Put difference in Screen ratio, but using only width, so the ratio is the same on both
                // axes (otherwise we would have to swipe more vertically...)
				diff = new Vector2(diff.x/Screen.width, diff.y/Screen.width);

				if(diff.magnitude > 0.01f) //we set the swip distance to trigger movement to 1% of the screen width
				{
					if(Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
					{
						if(TutorialMoveCheck(2) && diff.y < 0)
						{
                            pS_CharacterController.Slide_State();
							//Slide();
						}
						else if(TutorialMoveCheck(1))
						{
                            pS_CharacterController.Jump_State();
							//Jump();
						}
					}
					else if(TutorialMoveCheck(0))
					{
						if(diff.x < 0)
						{
                            pS_CharacterController.ChangeLane(-1);
							//ChangeLane(-1);
						}
						else
						{
                            pS_CharacterController.ChangeLane(1);
							//ChangeLane(1);
						}
					}
						
					m_IsSwiping = false;
				}
            }

        	// Input check is AFTER the swip test, that way if TouchPhase.Ended happen a single frame after the Began Phase
			// a swipe can still be registered (otherwise, m_IsSwiping will be set to false and the test wouldn't happen for that began-Ended pair)
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				m_StartingTouch = Input.GetTouch(0).position;
				m_IsSwiping = true;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				m_IsSwiping = false;
			}
        }
#endif
    }

}/*
        Vector3 verticalTargetPosition = m_TargetPosition;

		if (m_Sliding)
		{
            // Slide time isn't constant but the slide length is (even if slightly modified by speed, to slide slightly further when faster).
            // This is for gameplay reason, we don't want the character to drasticly slide farther when at max speed.
			float correctSlideLength = slideLength * (1.0f + trackManager.speedRatio);
			float ratio = (trackManager.worldDistance - m_SlideStart) / correctSlideLength;
			if (ratio >= 1.0f)
			{
                // We slid to (or past) the required length, go back to running
				StopSliding();
			}
		}

        if(m_Jumping)
        {
			if (trackManager.isMoving)
			{
                // Same as with the sliding, we want a fixed jump LENGTH not fixed jump TIME. Also, just as with sliding,
                // we slightly modify length with speed to make it more playable.
				float correctJumpLength = jumpLength * (1.0f + trackManager.speedRatio);
				float ratio = (trackManager.worldDistance - m_JumpStart) / correctJumpLength;
				if (ratio >= 1.0f)
				{
					m_Jumping = false;
					character.animator.SetBool(s_JumpingHash, false);
				}
				else
				{
					verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
				}
			}
			else if(!AudioListener.pause)//use AudioListener.pause as it is an easily accessible singleton & it is set when the app is in pause too
			{
			    verticalTargetPosition.y = Mathf.MoveTowards (verticalTargetPosition.y, 0, k_GroundingSpeed * Time.deltaTime);
				if (Mathf.Approximately(verticalTargetPosition.y, 0f))
				{
					character.animator.SetBool(s_JumpingHash, false);
					m_Jumping = false;
				}
			}
        }

        characterCollider.transform.localPosition = Vector3.MoveTowards(characterCollider.transform.localPosition, verticalTargetPosition, laneChangeSpeed * Time.deltaTime);

        // Put blob shadow under the character.
        RaycastHit hit;
        if(Physics.Raycast(characterCollider.transform.position + Vector3.up, Vector3.down, out hit, k_ShadowRaycastDistance, m_ObstacleLayer))
        {
            blobShadow.transform.position = hit.point + Vector3.up * k_ShadowGroundOffset;
        }
        else
        {
            Vector3 shadowPosition = characterCollider.transform.position;
            shadowPosition.y = k_ShadowGroundOffset;
            blobShadow.transform.position = shadowPosition;
        }
	}

    public void Jump()
    {
	    if (!m_IsRunning)
		    return;
	    
        if (!m_Jumping)
        {
			if (m_Sliding)
				StopSliding();

			float correctJumpLength = jumpLength * (1.0f + trackManager.speedRatio);
			m_JumpStart = trackManager.worldDistance;
            float animSpeed = k_TrackSpeedToJumpAnimSpeedRatio * (trackManager.speed / correctJumpLength);

            character.animator.SetFloat(s_JumpingSpeedHash, animSpeed);
            character.animator.SetBool(s_JumpingHash, true);
			m_Audio.PlayOneShot(character.jumpSound);
			m_Jumping = true;
        }
    }

    public void StopJumping()
    {
        if (m_Jumping)
        {
            character.animator.SetBool(s_JumpingHash, false);
            m_Jumping = false;
        }
    }

    public void Slide()
	{
		if (!m_IsRunning)
			return;
		
		if (!m_Sliding)
		{

		    if (m_Jumping)
		        StopJumping();

            float correctSlideLength = slideLength * (1.0f + trackManager.speedRatio); 
			m_SlideStart = trackManager.worldDistance;
            float animSpeed = k_TrackSpeedToJumpAnimSpeedRatio * (trackManager.speed / correctSlideLength);

			character.animator.SetFloat(s_JumpingSpeedHash, animSpeed);
			character.animator.SetBool(s_SlidingHash, true);
			m_Audio.PlayOneShot(slideSound);
			m_Sliding = true;

			characterCollider.Slide(true);
		}
	}

    public void StopSliding()
	{
		if (m_Sliding)
		{
			character.animator.SetBool(s_SlidingHash, false);
			m_Sliding = false;

			characterCollider.Slide(false);
		}
	}

    public void ChangeLane(int direction)
    {
		if (!m_IsRunning)
			return;

        int targetLane = m_CurrentLane + direction;

        if (targetLane < 0 || targetLane > 2)
            // Ignore, we are on the borders.
            return;

        m_CurrentLane = targetLane;
        m_TargetPosition = new Vector3((m_CurrentLane - 1) * trackManager.laneOffset, 0, 0);
    }

}*/
