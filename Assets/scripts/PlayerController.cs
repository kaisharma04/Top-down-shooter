using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {
     //handling
	public float rotationSpeed = 450;
	public float walkSpeed = 5;
	public float runSpeed = 8;
	private float acceleration = 5;
	public Gun gun;
	private Camera cam;
	//System
    private Quaternion targetRotation;
	private Vector3 currentVelocityMod;

	//components
	private CharacterController controller;
	void Start () {
		controller = GetComponent<CharacterController>();
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () { 
	
		ControlMouse();
		//ControlWASD();	

		if (Input.GetButtonDown("Shoot")){
			gun.Shoot();
		}
		else if(Input.GetButton("Shoot")){
			gun.ShootContinuous();
		}
	}

	void ControlMouse(){

		Vector3 mousePos =Input.mousePosition;
		mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x , mousePos.y,cam.transform.position.y-transform.position.y));
		targetRotation = Quaternion.LookRotation(mousePos- new Vector3(transform.position.x,0,transform.position.z));
		 transform.eulerAngles = Vector3.up  * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,rotationSpeed * Time.deltaTime);

		Vector3 input =new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
			currentVelocityMod =Vector3.MoveTowards(currentVelocityMod,input,acceleration * Time.deltaTime);
			Vector3 motion = currentVelocityMod;
			motion *=(Mathf.Abs(input.x)==1 && Mathf.Abs(input.z)==1)?.7f:1;
			motion *=(Input.GetButton("Run"))?runSpeed:walkSpeed;
			motion +=Vector3.up * -8;

			controller.Move(motion * Time.deltaTime);
	}
	void ControlWASD(){
		Vector3 input =new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
	   
	   if(input != Vector3.zero)
	   {
		   targetRotation = Quaternion.LookRotation(input);
		   transform.eulerAngles = Vector3.up  * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,rotationSpeed * Time.deltaTime);
	   }

			currentVelocityMod =Vector3.MoveTowards(currentVelocityMod,input,acceleration * Time.deltaTime);
			Vector3 motion = currentVelocityMod;
			motion *=(Mathf.Abs(input.x)==1 && Mathf.Abs(input.z)==1)?.7f:1;
			motion *=(Input.GetButton("Run"))?runSpeed:walkSpeed;
			motion +=Vector3.up * -8;

			controller.Move(motion * Time.deltaTime);
	}

}
