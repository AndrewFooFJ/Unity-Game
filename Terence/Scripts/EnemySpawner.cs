using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject[] enemyTypes; // Your prefabs go here.
    public int minimumSpawns, maximumSpawns;

    public Vector2 minimumPosition, maximumPosition;

    public GameObject GetRandomPrefab() {
        return enemyTypes[UnityEngine.Random.Range(0,enemyTypes.Length)];
    }

}
