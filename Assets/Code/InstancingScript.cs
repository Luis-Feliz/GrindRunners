using UnityEngine;
using TMPro;

public class InstancingScript : MonoBehaviour
{
    public GameObject obsticles;   // Game Object being manipulated

    float points;                               // Value to determine the current score in Float
    int i_points;                               // Value to determine the current score in Interger
    int h_score;                                // Value to determine the current high score of the run
    public TextMeshProUGUI score;               // Public Text value to view the score in game
    public TextMeshProUGUI highScore;           // Public Text value to view the high score in game

    GameObject clone1;                          // Game objects holding one of the many game obsticles
    GameObject clone2;                          // Game objects holding one of the many game obsticles
    GameObject clone3;                          // Game objects holding one of the many game obsticles
    GameObject[] objArray = new GameObject[4];  // Game object array to store the many game obsticles

    Vector3 start = new Vector3(0, 0, 95);      // Loctaion where Value will be moved to
    Quaternion rotation;                        // Default Quanternion to add when instantiating
    
    void Start()
    {
        ObjAssign();                            // Function assigning all game objects into the array

        //Begining instantiation followed by a random third clone using the Create function 
        clone1 = Instantiate(objArray[0], new Vector3(0, 0, 0), rotation);
        clone2 = Instantiate(objArray[0], new Vector3(0, 0, 45), rotation);
        clone3 = Create();

        //Starting the score value at 0
        score.SetText(points.ToString());
    }


    // Update is called once per frame
    void Update()
    {
        //if obsticles makes it to the end destroy that value and imediately crate a new one using the create function
        if (clone1.transform.position == new Vector3(0, 0, -50))
        {
            Destroy(clone1);
            clone1 = Create();

        }
        else if (clone2.transform.position == new Vector3(0, 0, -50))
        {
            Destroy(clone2);
            clone2 = Create();

        }
        else if (clone3.transform.position == new Vector3(0, 0, -50))
        {
            Destroy(clone3);
            clone3 = Create();
        }

        if (MovementScript.speed != 0)
        {
            points += Time.deltaTime;                   // Points are increased as time continues
            i_points = (int)points;                     // Points are then cinverted from float to interger
            score.SetText(i_points.ToString());         // Finally, Interger score is uptadated into the game
        }
        else 
        {   
            // When the game has concluded check if the current score beats your High Score
            if (i_points > h_score) 
            {
                h_score = i_points;
                //Update High Score
                highScore.SetText(h_score.ToString());
            }
            else 
            {
                //Display High Score 
                highScore.SetText(h_score.ToString());
            }
        }
        
    }

    GameObject Create() 
    {
        
        int randomNumber = Random.Range(0, 3);
        return Instantiate(objArray[randomNumber], start, rotation);
    }

    void ObjAssign() 
    {
        objArray[0] = GameObject.Find("Obsticles");
        objArray[1] = GameObject.Find("Obsticles1");
        objArray[2] = GameObject.Find("Obsticles2");
        objArray[3] = GameObject.Find("Obsticles3");
        /*
        objArray[4] = GameObject.Find("Obsticle(4)");
        objArray[5] = GameObject.Find("Obsticle(5)");
        */
        //  objArray[6] = GameObject.Find("Obsticle(6)");
        //  objArray[7] = GameObject.Find("Obsticle(7)");
    }
}
//Need origininal Obsticals to stay alive so that the other clones can spawn

/*  
 *  IDEA: As an onject deloads we can instansiate to ensure that
 *  We always are making an object as one deloads thus solving the speed 
 *  problem. No matter how fast we will go. Values will be created just as
 *  fast as they are erased
 */
