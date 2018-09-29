using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public enum GunType {Semi,Burst,Auto};
	public GunType gunType;
	public float rpm;

	//components
	public Transform spawn;
	public Transform ShellEjectionPoint;
	public Rigidbody shell;
	private LineRenderer tracer;

	public AudioClip mySound;
	public AudioSource mySource;
	public float myVolume =1.0f;


	//system variables
	private float secondsBetweenShots;
	private float nextPossibleShootTime;

	void Start(){
		secondsBetweenShots = 60 /rpm;
		if(GetComponent<LineRenderer>()){

			tracer = GetComponent<LineRenderer>();
		}
	}

	public void Shoot(){

		if(CanShoot()){
			
			Ray ray = new Ray(spawn.position,spawn.forward);
		    RaycastHit hit;

	    	float shotDistance =20;
			if(Physics.Raycast(ray, out hit,shotDistance)){

			shotDistance = hit.distance;
		}

		nextPossibleShootTime = Time.time + secondsBetweenShots;
		mySource.PlayOneShot(mySound,myVolume);

		if(tracer){
			StartCoroutine("RenderTracer",ray.direction * shotDistance);
		}
		Rigidbody newShell = Instantiate(shell,ShellEjectionPoint.position,Quaternion.identity) as Rigidbody;
		newShell.AddForce(ShellEjectionPoint.forward * Random.Range(150f,200f) + spawn.forward * Random.Range(-10f,10f));
	}
	
  }		
        

	public void ShootContinuous(){
		if(gunType == GunType.Auto){
			Shoot();
		}
	}


	public bool CanShoot(){
		bool CanShoot = true;

		if(Time.time < nextPossibleShootTime){
			CanShoot = false;
		}

		return CanShoot;
	}

	IEnumerator RenderTracer(Vector3 hitPoint){
			
		tracer.enabled = true;
		tracer.SetPosition(0,spawn.position);
		tracer.SetPosition(1,spawn.position + hitPoint);


		yield return null;
		tracer.enabled = false;
	}
}


