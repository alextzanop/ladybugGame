using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    public float[] xBounds; //scene bounds x
    public float[] yBounds; //scene bounds y
    public static SceneHandler instance;
    public GameObject player;
    public Transform enemies; //transform containing all enemies
    


    private bool shooting;
    private bool rotating;
   
    
    void Awake()
    {
        instance = this;   
    }

    private void Start()
    {
        rotating = false;
        shooting = false;
        setPlayerRotation(false);
        setPlayerShoot(false);
        setEnemiesActive(false);
        
        
    }

    private void Update()
    {
		
        if (instance.player.transform.rotation.z != 0 && rotating)
        {
			//wait for 2 seconds since the player starts rotating
            instance.StartCoroutine(SceneHandler.wait(2));
            rotating = false;
        }
		
        if (instance.shooting)
        {
            //when all enemies are dead resume dialogue
            if (enemies.childCount == 0)
            {
                
                shooting = false;
                setPlayerShoot(false);
                setPlayerRotation(false);
                dialogueManager.cut = false;
                dialogueManager.statCanv.enabled = true;
                
            }

        }
    }


	//activate rotation for the tutorial
    static public void playerRotateTutorial()
    {
        instance.rotating = true;
        setPlayerRotation(true);
        
    }

	// activates/deactivates enemies' healthbar(first child)
    static public void setEnemiesActive(bool active)
    {
        foreach (Transform t in instance.enemies)
        {
            t.GetChild(0).gameObject.SetActive(active);
        }
    }

	// enables/disables player shoot controls
    static public void setPlayerShoot(bool shoot)
    {
        instance.player.GetComponent<turretFire>().shoot = shoot;
        
        instance.shooting = shoot;
    }

	// enables/disables player rotation
    static public void setPlayerRotation(bool rotate)
    {
        instance.player.GetComponent<turretFire>().rotate = rotate;
        

    }

    static public IEnumerator wait(float sec)
    {
        
        yield return new WaitForSeconds(sec);
        dialogueManager.cut = false;
        dialogueManager.statCanv.enabled = true;
        setPlayerRotation(false);
       
    }
    
}
