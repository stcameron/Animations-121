using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightController : MonoBehaviour
{
    GameObject thedoor;

    public GameObject pickupEffect;

    public Text countText;

	public float speed = 4;
	public float rotSpeed = 80;
	public float rot =0f;
	public float gravity = 8;

    private int count;

	Vector3 moveDir = Vector3.zero;

	CharacterController controller;
	Animator anim;

    // Start is called before the first frame update
    void Start()
    {
    	controller = GetComponent<CharacterController> ();
    	anim = GetComponent<Animator> ();
        count = 0;
        SetCountText();
        
    }

    // Update is called once per frame
    void Update()
    {
    	Movement();
    	GetInput();
    }

    void Movement()
    {
    	if(controller.isGrounded) {
    		if(Input.GetKey (KeyCode.W)) {
    			if(anim.GetBool ("attacking") == true) {
    				return;
    			} else if(anim.GetBool("attacking") == false) {
    			anim.SetBool ("running", true);
    			anim.SetInteger("condition", 1);
    			moveDir = new Vector3 (0, 0, 1);
    			moveDir *= speed;
    			moveDir = transform.TransformDirection (moveDir);
    			}	
    		}	

    		if(Input.GetKeyUp (KeyCode.W)) {
    			anim.SetBool ("running", false);
    			anim.SetInteger ("condition", 0);
    			moveDir = new Vector3 (0, 0, 0);
    		}

            if (Input.GetKey(KeyCode.S))
            {
                if (anim.GetBool("attacking") == true)
                {
                    return;
                }
                else if (anim.GetBool("attacking") == false)
                {
                    anim.SetBool("running", true);
                    anim.SetInteger("condition", 1);
                    moveDir = new Vector3(0, 0, -1);
                    moveDir *= speed;
                    moveDir = transform.TransformDirection(moveDir);
                }
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                anim.SetBool("running", false);
                anim.SetInteger("condition", 0);
                moveDir = new Vector3(0, 0, 0);
            }
        }
        rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3 (0, rot, 0);   

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move (moveDir * Time.deltaTime); 	
    }

    void GetInput() 
    {
    	if(controller.isGrounded) {
    		if(Input.GetMouseButtonDown (0)) {
    			if(anim.GetBool("running") == true) {
    				anim.SetBool ("running" , false);
    				anim.SetInteger("condition", 0);
    			}  
    			if(anim.GetBool ("running") == false) {
    				Attacking();
    			}
    		}
    	}

    }

    void OnTriggerEnter(Collider other)

    {

        if (other.gameObject.CompareTag("Pickup"))
        {
            Instantiate(pickupEffect, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            count += 1;
            SetCountText();

            if (count >= 5)
            {
                thedoor = GameObject.FindWithTag("SF_Door");
                thedoor.GetComponent<Animation>().Play("open");
            }
        }
    }

    void SetCountText()
    {
        countText.text = "Gems: " + count.ToString();
    }

    void Attacking() 
    {

    	StartCoroutine (AttackRoutine());

    }

    IEnumerator AttackRoutine()
    {
    	anim.SetBool ("attacking", true);
    	anim.SetInteger("condition", 2);
    	yield return new WaitForSeconds (1);
    	anim.SetInteger("condition", 0);
    	anim.SetBool ("attacking", false);
    }
}
