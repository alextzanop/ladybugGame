using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Facing { UP, DOWN, LEFT, RIGHT };
public class EnemyUnit : MonoBehaviour
{
    public float speed; //movement speed
    public int moveBlocks; //used for tile movement(might not use)
    public float life; //health points
	
    public GameObject healthBar; //
    public float y_offset; //healthbar y-offset
    public Transform waypoint; //unit to move towards for tile movement(might not use)
    public TextAsset textfile; //file of movement directions
    public Animator animator;
    public Facing face;
    

    private string[] directionText; //array of movement directions derived from the text file
    private int[][] directions; //final array of directions. contains a [x,y] set for each direction. 0 to not move, 1 to move up or to the right, -1 to move down or to the left  
    private bool canBlink; //for blinking cooldown for when taking damage(might not use)
    private float maxLife; //initial life
    private int currentDir; //current direction
    private bool canMove; //for move cooldown(not used in intro)
    
    // Start is called before the first frame update
    void Start()
    {
        
        initUnit();
        setFace(face);
    }

    // Update is called once per frame
    void Update()
    {
        //move towards waypoint
        MoveToWaypoint();
		
		//kill unit when life <= 0 
        if (this.life <= 0)// || transform.position.x > 5)
            Destroy(this.gameObject);
       
    }

	//initialize life, healthbar, directions etc
    void initUnit()
    {
        maxLife = life;
        canBlink = true;
        setHealthBar();
        waypoint.parent = null;
        makeDirections();
        canMove = true;
    }

	/*
	The direction file contains directions in the from of x,y,az...
	where x,y,z are on of the following values [s,n,w,e] (for south, north, west, east)
	and a is an integer determining how many times the following direction should be executed
	
	For example: e,s,2w -> move one unit to east, one to south and 2 to west.
	position   :   [0,0]->[-1,-1]
	*/
    void makeDirections()
    {
        
        int finalSize = 0;
        currentDir = 0;
		//split directions in ','
        directionText = textfile.text.Split(',');
        

        //determine final size
        for(int i = 0; i < directionText.Length; i++)
        {
            if (directionText[i].Length == 1)
                finalSize++;
            else
            {
                
                finalSize += int.Parse(directionText[i].Substring(0, directionText[i].Length - 1));
                
            }
        }
		
		
        string[] finalDirText = new string[finalSize];
        int tempI = 0;
        for(int i = 0; i < directionText.Length; i++)
        {
            if (directionText[i].Length == 1)
                finalDirText[tempI++] = directionText[i];
            else
            {
				//how many times to execute the direction
                int times = int.Parse(directionText[i].Substring(0, directionText[i].Length - 1));
                for(int j = 0; j < times; j++){
                    finalDirText[tempI++] = directionText[i][directionText[i].Length - 1].ToString();
                }
            }
            
        }

        directions = new int[finalDirText.Length][];
        for (int j = 0; j < finalDirText.Length;  j++)
        {
            directions[j] = new int[2];
            
        }
        
		//set directions numbers per direction
		/*
		s:south
		n:north
		w:west
		e:east
		*/
        for (int i=0; i<finalDirText.Length; i++)
        {
            
            if (string.Equals(finalDirText[i], "s"))
            {
               
                directions[i][0] = 0;
                directions[i][1] = -1;
                
            }
            else if (string.Equals(finalDirText[i], "n"))
            {
                directions[i][0] = 0;
                directions[i][1] = 1;
            }
            else if (string.Equals(finalDirText[i], "w"))
            {
                
                directions[i][0] = -1;
                directions[i][1] = 0;
                
            }
            else if (string.Equals(finalDirText[i], "e"))
            {
                
                directions[i][0] = 1;
                directions[i][1] = 0;
            }
            
        }
        
    }

	//set sprite depending on face direction
    void setFace(Facing face)
    {
        Debug.Log(this.face==Facing.DOWN);
        this.face = face;
        if (this.face == Facing.UP)
            this.transform.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Sprites/ladybug")[0] as Sprite;
        else if (this.face == Facing.DOWN)
            this.transform.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Sprites/ladybug")[2] as Sprite;
        else if (this.face == Facing.LEFT)
            this.transform.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Sprites/ladybug")[1] as Sprite;
        else if (this.face == Facing.RIGHT)
            this.transform.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Sprites/ladybug")[3] as Sprite;
        Debug.Log(this.transform.GetComponent<SpriteRenderer>().sprite);
    }
    
	//set health bar position based on y_offset
    void setHealthBar()
    {
        healthBar.transform.position = this.transform.position + new Vector3(0,y_offset,0);
    }

	//deal damage to unit
    public void takeDamage(float dmg)
    {
		//blink(might not use)
		if(canBlink)
            StartCoroutine(blink());
        this.life -= dmg;
		//update healthbar slider
        healthBar.GetComponent<Slider>().value -= (float)dmg / maxLife;
    }

	//move unit to waypoint(might not use)
	//moves waypoint first and then the unit follows to create the "tile movement"
    public void MoveToWaypoint()
    {
        if (canMove)
        {
            //determine position
            transform.position = Vector3.MoveTowards(transform.position, waypoint.position, speed * Time.deltaTime);
            //animator.SetBool("moving", true);

        }
        //else
           // animator.SetBool("moving", false);
	   
	    
        if (Vector3.Distance(this.transform.position, waypoint.position) < .005f && canMove)
        {
           //waypoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f) * moveBlocks;
            StartCoroutine(pauseMove());
			//move waypoint
            waypoint.position += new Vector3(directions[currentDir][0], directions[currentDir][1], 0f) * moveBlocks;

            currentDir++;
            if (currentDir >= directions.Length)
                currentDir = 0;
            
        }
         
       
    }
	
	//pause movement
    IEnumerator pauseMove()
    {
        canMove = false;
        yield return new WaitForSeconds(1);
        canMove = true;
    }
	
	//blink unit
    IEnumerator blink()
    {
        canBlink = false;
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.01f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
        canBlink = true;

    }
}
