using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public Enemy enemy;

	Wave currentWave;
	int currentWaveCount;
	int enemiesRemainingToSpawn;
	int enemiesRamainingAlive;
	float nextSpawnTime;

	[System.Serializable]
	public class Wave{
		public int enemyCount;
		public int timeBetweenSpawns;
	}

	// Use this for initialization
	void Start () {
		NextWave();
	}
	
	// Update is called once per frame
	void Update () {
		if(enemiesRemainingToSpawn > 0 &&  Time.time > nextSpawnTime){
			enemiesRemainingToSpawn--;
			nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

			Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
			spawnedEnemy.OnDeath += OnEnemyDeath;
		}
	}

	void OnEnemyDeath(){
		enemiesRamainingAlive--;
		if(enemiesRamainingAlive == 0){
			NextWave();
		}
	}

	void NextWave(){
		currentWaveCount++;
		if(currentWaveCount - 1 < waves.Length){
			currentWave = waves[currentWaveCount -1];
			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesRamainingAlive = enemiesRemainingToSpawn;
		}
	}
}
