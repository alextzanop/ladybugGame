using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletHandler : MonoBehaviour
{
    public float speed;//
    public float damage;
    //public float dd;
    
    [HideInInspector]
    public Vector3 direction = new Vector3(0,1F,0);
    // Start is called before the first frame update
    void Start()
    {
        //direction = Vector3.up;   
    }

    // Update is called once per frame
    void Update()
    {
        //move bullet
        transform.position +=  direction * speed ;
        
        if (this.damage <= 0)
            this.damage = 0;
		//destroy bullet if it goes off bounds
        if (outOfBounds())
            Destroy(this.gameObject);
    }

	//check if bullet is out of bounds
    bool outOfBounds()
    {
        if(this.transform.position.x >= SceneHandler.instance.xBounds[0] || this.transform.position[0] <= SceneHandler.instance.xBounds[1])
            return true;
        if (this.transform.position.y >= SceneHandler.instance.yBounds[0] || this.transform.position.y <= SceneHandler.instance.yBounds[1])
            return true;
        return false;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Enemy"))
        {
			//destroy bullet on contact with enemy
            Destroy(this.gameObject);
			
			//deal damage depending on travel distance
            other.gameObject.GetComponent<EnemyUnit>().takeDamage(damage/this.transform.position.magnitude*10);
        }
    }
}
