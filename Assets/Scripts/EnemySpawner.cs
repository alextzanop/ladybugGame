using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//unused in intro level
public class EnemySpawner : MonoBehaviour
{
    public GameObject unit; //enemy unit to spawn
    public GameObject spawnPoint; //enemy spawn point
    public float spawnCD; //enemy spawn cooldown

    private bool canSpawn; //true if can span new enemy, false otherwise
    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawn)
            Spawn();   
    }

	//spawn unit on spawnPoint
    void Spawn()
    {
        Instantiate(unit, spawnPoint.transform.position, Quaternion.identity);
        StartCoroutine(spawnCooldown());
    }
    
	//wait to spawn new enemy
    IEnumerator spawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnCD);
        canSpawn = true;
    }
}
