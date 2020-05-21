using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretFire : MonoBehaviour
{
    public Animator animator;
    public GameObject bullet; //bullet to shoot
    public GameObject spawn;  //shoot point
    
    public float fireRate; //seconds
    public float rotateSpeed; 
    public float attackRadius;
    public GameObject attackRadiusIMG; //(might not use)


    private bool canShoot; //true if can shoot, false otherwise
    private bool onCD; //true if on shoot cooldown, false otherwise
    private int enemiesInRange; //(probably wont use)
    private Vector3 spawnPoint; //spawn vector
    private GameObject enemy; //unused
    [HideInInspector]
    public bool shoot;
    [HideInInspector]
    public bool rotate;

    // Start is called before the first frame update
    private void Awake()
    {
        makeAttackRadius();
    }

    void Start()
    {
        shoot = true;
        rotate = true;
        canShoot = false;
        onCD = false;
        enemiesInRange = 0;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
            //makeAttackRadius();
            spawnPoint = spawn.transform.position;

            if (!onCD && shoot)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetBool("shooting", true);
                    Shoot();
                }
            }
            else
                animator.SetBool("shooting", false);
            if(rotate)
                this.transform.Rotate(new Vector3(0f, 0f, -Input.GetAxis("Horizontal") * rotateSpeed));

        
    }

	//build the attack radius(probably wont use)
    void makeAttackRadius()
    {
        this.GetComponent<CircleCollider2D>().radius = attackRadius;
        attackRadiusIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(2.25f*attackRadius, 2.25f*attackRadius);
    }

	//shoot function
    void Shoot()
    {
        //create  bullet
        GameObject newBullet = Instantiate(bullet, spawnPoint, Quaternion.identity) as GameObject;
		
		//calculate rotation angle
        float angle = -this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
		
		//move bullet on a direction based on the rotation angle
        newBullet.GetComponent<bulletHandler>().direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        
        StartCoroutine(shootCooldown());
    }


    //not used
    void followEnemy()
    {
        if(!enemy)
            enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy)
        {
            
            float angle = Mathf.Acos(Vector3.Dot(enemy.transform.position,new Vector3(0,1F,0)) /Vector3.Magnitude(enemy.transform.position));
            this.transform.rotation = Quaternion.Euler(new Vector3(0,0,-Mathf.Sign(enemy.transform.position.x)*Mathf.Rad2Deg*angle));
            attackRadiusIMG.transform.rotation = Quaternion.Euler(0, 0, 0);
        }


    }

	//not used
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            
            
            enemiesInRange += 1;
        }
    }

	//not used
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            enemiesInRange -= 1;
    }


	//cooldown for next shot
    IEnumerator shootCooldown()
    {
        
        onCD = true;
        yield return new WaitForSeconds(fireRate);
        
        onCD = false;
    }

   
}
