using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogueManager : MonoBehaviour
{
    public static dialogueManager dM;
    public static Canvas statCanv; //static canvas copy


    public Canvas canvas;
    public TextAsset scriptText; //dialogue script
    public Text npcName; //npc name to display
    public Text text; //text to display
    
    private string[] lines; //array of text file lines
    private int currIndex; //current index of line array

    [HideInInspector]
    public static bool cut; //true to cut the dialog, false otherwise

    private void Awake()
    {
        dM = this;
        statCanv = canvas;
    }
    // Start is called before the first frame update
    void Start()
    {

        cut = false;
        currIndex = 0;
        lines = scriptText.text.Split('\n'); //split text on newline
        readNextLine();
        

    }

    // Update is called once per frame
    void Update()
    {
        // hide/unhide canvas by pressing P
        if (Input.GetKeyDown(KeyCode.P) && !cut)
            statCanv.enabled = !statCanv.enabled;


        if (statCanv.enabled)
        {
			//go to next line by pressing enter
            if (Input.GetKeyDown(KeyCode.Return) && !cut)
                readNextLine();
        }
            
    }

	//read the next line from the lines array
	/*
	
	**Unique texts in text file**
	
	'~' 	       :signifies new npc name to be displayed
	"rotate\r"     :tells the SceneHandler to enable player rotation
	"enableLB\r"   :tells the SceneHandler to enable labybug health bars
	"enableShoot\r":tells the SceneHandler to enable player shoot and rotation
	*/
    void readNextLine()
    {
        if (currIndex < lines.Length)
        {
			//new NPC name
            if (lines[currIndex][0] == '~')
            {
                updateName(lines[currIndex].Substring(1));currIndex++;
                
            }
           
			//enable rotation
            if (string.Equals(lines[currIndex], "rotate\r")) {
                
                statCanv.enabled = false;
                SceneHandler.playerRotateTutorial();
                cut = true;
                currIndex++;
                
            }
			//enable healthbars
            else if(string.Equals(lines[currIndex], "enableLB\r"))
            {
                SceneHandler.setEnemiesActive(true);
                currIndex++;

            }
			//enable player shoot and rotation controls
            else if (string.Equals(lines[currIndex], "enableShoot\r"))
            {
                
                statCanv.enabled = false;
                SceneHandler.setPlayerRotation(true);
                SceneHandler.setPlayerShoot(true);
                cut = true;
                currIndex++;
            }

            
            updateText(lines[currIndex]);
            currIndex++;

        }
        
    }

	//update the text on the canvas
    void updateText(string s)
    {
        text.text = s;
    }

	//update NPC Name on canvas
    void updateName(string s)
    {
        npcName.text = s;
    }
}
