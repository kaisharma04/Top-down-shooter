using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {Idle,Chasing,Attacking};

	Material skinMaterial;
	Color originalColor;
	State currentState;

	NavMeshAgent pathfinder;
	Transform target;
	float attackDistanceThreshold = .75f;
	float timeBetweenAttacks = 1f;
	float nextAttackTime;

	float myCollisionRadius;
	float targetCollisionRadius;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		pathfinder = GetComponent<NavMeshAgent>();
		currentState = State.Chasing;
		target = GameObject.FindGameObjectWithTag("Player").transform;

		myCollisionRadius = GetComponent<CapsuleCollider>().radius;
		targetCollisionRadius = GetComponent<CapsuleCollider>().radius;

		skinMaterial = GetComponent<Renderer>().material;
		originalColor = skinMaterial.color;
		StartCoroutine(UpdatePath());
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > nextAttackTime){
			float sqrtDistanceToTarget = (target.position - transform.position).sqrMagnitude;
			if(sqrtDistanceToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius,2)){
				nextAttackTime = Time.time + timeBetweenAttacks;
				StartCoroutine(Attack());
			}
		}
	}

	IEnumerator Attack(){

		currentState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 attackPosition = target.position;

		skinMaterial.color = Color.red;

		float percent = 0;
		float attackSpeed = 3;
		while(percent <= 1){

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
			transform.position = Vector3.Lerp(originalPosition,attackPosition,interpolation);

			yield return null;
		}

		skinMaterial.color = originalColor;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}


	IEnumerator UpdatePath(){
		float refreshRate = .25f;
		while(target != null){
			if(currentState == State.Chasing){
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
				pathfinder.SetDestination(targetPosition);
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}
