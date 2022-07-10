using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Determins what state the Charater is at all times.
public enum Grounded { FLOOR, LEFT, RIGHT, AIR, SLIDE }; 
public enum Copy { FLOOR, LEFT, RIGHT, AIR, SLIDE }; 

public class ControllerScript : MonoBehaviour
{
    
    public Grounded state;                          //Var to Determine enum state 
    public Copy copy;                               //Var to Copy enum state
    
    public Rigidbody rb;                            //Rigidbody of player
    public Animator myAnim;                         //Animator for animations
    public CapsuleCollider playerCollider;          //Var for player Collider to edit collider hight for sliding

    private GameObject gameOver;                    //Game object value to show gameover menu
    private GameObject pause;                       //Game object value to show pause menu

    Vector3 gravity;                                //Var to control gravity
    Vector3 jump;                                   //Var to give player Jump
    Quaternion rot;                                 //Var to give player rotation

    public float speed;                             //Var to determine the speed of the trasition from floor to wall
    public float jumpForce;                         //Var to Determine Jump Hight
    public float jumpTime = 0.05f;                  //Var to Determine time jumping
    public float time;                              //Var to determine the timer countdown
    
    bool noHolding;                                 //Bool to ensure that the being held doesn't hold the button
    bool isGrounded;                                //Var to check if player is on the ground
    bool set;

    private readonly float smooth = 7;              //Var to determing how smooth the rotation of character will 

    void Start()
    {
        gameOver = GameObject.Find("GameOver_Menu");
        gameOver.SetActive(false);

        pause = GameObject.Find("Pause_Menu");
        pause.SetActive(false);
        
        rb = GetComponent<Rigidbody>();             //RigidBody Assignment
        state = Grounded.FLOOR;                     //Starts the state immediatly in floor
        
        myAnim = GetComponent<Animator>();          //Assigning the Component Animator
        time = jumpTime;

        Time.timeScale = 1;
    }

    void OnCollisionEnter(Collision collision)
    {   
        //If statement to determin if the player is touching obsticles
        if (collision.gameObject.tag == "Obsticles")
        {                  
            /*  Game over Screen
                Menu pop up made here to show a game over menu 
                containing the game over words, your current score 
                (although the score can also be in the top right 
                corner and stay there)
                and a menu made for the choice to restart, go to 
                your main menu, or just continue  where you left off,
                (For a price).
                go to the speed in the movement Script and set it to 0 
                to effectivly pause the game
                maybe change the enum state to a Game over one to stop 
                players from moving within the menu
            */
            MovementScript.speed = 0f;
            gameOver.SetActive(true);

            //pop up menu
        }
        //if not check if the player is touching ground of not
        else 
        { 
            //rb.AddForce(jump * 0, ForceMode.Impulse);
            isGrounded = true;
            noHolding = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("escape") && MovementScript.speed != 0)
        {
            Pausing();
        }
        switch (state) 
        {
            case Grounded.FLOOR:
                FloorSetup();   //Setting up Floor Gravity and other Misc.

                //Change directions input
                if (Input.GetAxis("Horizontal") < 0 && (isGrounded)) //Going Left
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                    state = Grounded.AIR;
                    copy = Copy.LEFT;
                }
                else if (Input.GetAxis("Horizontal") > 0 && (isGrounded)) //Going Right
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                    state = Grounded.AIR;
                    copy = Copy.RIGHT;
                }
                //Jumping Input
                else if (Input.GetButtonDown("Jump") && (isGrounded))
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                }
                //Sliding Input
                else if (Input.GetAxis("Vertical") < 0 && (isGrounded))
                {
                    time = 0.75f;
                    state = Grounded.SLIDE;
                    copy = Copy.FLOOR;
                    myAnim.Play("Running Slide");
                }
                //Fast Fall Input
                else if (isGrounded == false)
                {
                    if (Input.GetAxis("Vertical") < 0 && noHolding == true)
                    {
                        FastFall();
                        noHolding = false;
                    }
                }
                break;
            case Grounded.RIGHT:
                RightSetup();       //Setting up Right Gravity and other Misc.
                
                //Change directions input
                if (Input.GetAxis("Horizontal") < 0 && (isGrounded))
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                    state = Grounded.AIR;
                    copy = Copy.LEFT;
                }
                else if (Input.GetAxis("Vertical") < 0 && (isGrounded))
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                    state = Grounded.AIR;
                    copy = Copy.FLOOR;
                }
                //Sliding Input
                else if (Input.GetAxis("Horizontal") > 0 && (isGrounded))
                {
                    time = 0.75f;
                    state = Grounded.SLIDE;
                    copy = Copy.RIGHT;
                    myAnim.Play("Running Slide");
                }
                //Jumping Input
                else if (Input.GetButtonDown("Jump") && (isGrounded))
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                }
                //Fast Fall Input
                else if (isGrounded == false)
                {
                    if (Input.GetAxis("Horizontal") > 0 && noHolding == true)
                    {
                        FastFall();
                        noHolding = false;
                    }
                }
                break;
            case Grounded.LEFT:
                LeftSetup();        //Setting up Left Gravity and other Misc.
                                    //Change directions input
                if (Input.GetAxis("Vertical") < 0 && (isGrounded))
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                    state = Grounded.AIR;
                    copy = Copy.FLOOR;
                }
                else if (Input.GetAxis("Horizontal") > 0 && (isGrounded))
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                    state = Grounded.AIR;
                    copy = Copy.RIGHT;
                }
                //Sliding Input
                else if (Input.GetAxis("Horizontal") < 0 && (isGrounded))
                {
                    time = 0.75f;
                    state = Grounded.SLIDE;
                    copy = Copy.LEFT;
                    myAnim.Play("Running Slide");
                }
                //Jumping Input
                else if (Input.GetButtonDown("Jump") && (isGrounded))
                {
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                }
                //Fast Fall Input
                else if (isGrounded == false)
                {
                    if (Input.GetAxis("Horizontal") < 0 && noHolding == true)
                    {
                        FastFall();
                        noHolding = false;
                    }
                }
                break;
            case Grounded.AIR:
                // Timer set up before switching 
                if (time > 0)
                {
                    time -= Time.deltaTime;
                }
                else
                {
                    state = (Grounded)copy;
                    time = jumpTime;
                }
                // Change to the previously mentioned direction
                break;
            case Grounded.SLIDE:
                // timer set up to reset the hight position
                if (time > 0)
                {
                    time -= Time.deltaTime;
                    playerCollider.height = 1;
                }
                else
                {
                    state = (Grounded)copy;
                    time = .5f;
                    playerCollider.height = 2;

                }
                // commnds to recet the character hight andd animation
                // if they press a button early
                break;
        }

        
    }

    /**************************************************************/
    /*
     * Name:        floorSetup
     * Parameters:  None
     * Purpose:     Setting the gravity of the floor state,
     *              along with Character orrientation, and 
     *              potentially Character animation.
     */
    /**************************************************************/

    void FloorSetup()
    {
        Physics.gravity = gravity;
        gravity.x = 0;
        gravity.y = -25;
        gravity.z = 0;
        jump = new Vector3(0f, 2.5f, 0.0f);
        
        rot = Quaternion.Euler(0, 0, 0);
        rb.transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * smooth);
    }

    /**************************************************************/
    /*
     * Name:        leftSetup
     * Parameters:  None
     * Purpose:     Setting the gravity of the left state,
     *              along with Character orrientation, and 
     *              potentially Character animation.
     */
    /**************************************************************/

    void LeftSetup()
    {
        Physics.gravity = gravity;
        jump = new Vector3(2.5f, 0.0f, 0.0f);   //Jumping Vector
        gravity.x = -25;
        gravity.y = 0;
        gravity.z = 0;

        rot = Quaternion.Euler(0, 0, -90);
        rb.transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * smooth);
    }

    /**************************************************************/
    /*
     * Name:        rightSetup
     * Parameters:  None
     * Purpose:     Setting the gravity of the right state,
     *              along with Character orrientation, and 
     *              potentially Character animation.
     */
    /**************************************************************/

    void RightSetup()
    {
        Physics.gravity = gravity;
        jump = new Vector3(-2.5f, 0.0f, 0.0f);   //Jumping Vector
        gravity.x = 25;
        gravity.y = 0;
        gravity.z = 0;

        rot = Quaternion.Euler(0, 0, 90);
        rb.transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * smooth);
    }

    void FastFall() 
    {
        rb.AddForce(-jump * jumpForce, ForceMode.Impulse);
    }

    void Pausing() 
    {
        if (set == true)
        {
            pause.SetActive(true);
            Time.timeScale = 0;
            set = false;
        }
        else if (set == false)
        {
            pause.SetActive(false);
            Time.timeScale = 1;
            set = true;
        }
    }
}
/*
 * THings I want:
 *  -   IDEA 
 *          Make the value a function that moves the 
 *          character to their intended location but 
 *          has to something to 
 *  -   Slide input 
 *          lower rigid body and raise it in an animation? 
 *  -   Proper Time to move around
 *          Timer to determine Jump 
 *          Before max high or after?
 *          
 *  -   What I should do when it comes to the animation
 *          Perhaps it would be more applicable to put it into a function
 *          however similar to the timer problem with the jumping animation
 *          there isn't a way to just switch into a function and keep 
 *          it there for  timer as far as i know
 *          
 *          What if I do somehing when it comes to the state like a 
 *          slide state that changes into a timer
 *          on that note why sdon't I use a timer on the Airial state 
 *          as a way to determine what I wanna switch to
 *  Extras:
 *  
 *  Vector3 floorPos = new Vector3(0, 0, 0);        //Var to determine Floor position
 *  Vector3 rightPos = new Vector3(5, 2.5f, 0f);    //Var to determine Right position
 *  Vector3 leftPos = new Vector3(-5, 2.5f, 0f);    //Var to determine Left position
 *   //rb.transform.position = Vector3.MoveTowards(floorPos, leftPos, speed * Time.deltaTime);
 *   //rb.transform.position = Vector3.Lerp(floorPos, leftPos, Time.deltaTime);
 *   //If statements to determine what state will be used
        if (state == Grounded.FLOOR)
        {
            FloorSetup();   //Setting up Floor Gravity and other Misc.

            //Change directions input
            if (Input.GetAxis("Horizontal") < 0 && (isGrounded)) //Going Left
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                state = Grounded.AIR;
                copy = Copy.LEFT;
            }
            else if (Input.GetAxis("Horizontal") > 0 && (isGrounded)) //Going Right
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                state = Grounded.AIR;
                copy = Copy.RIGHT;
            }
            //Jumping Input
            else if (Input.GetButtonDown("Jump") && (isGrounded))
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            //Sliding Input
            else if (Input.GetAxis("Vertical") < 0 && (isGrounded))
            {
                time = 0.75f;
                state = Grounded.SLIDE;
                copy = Copy.FLOOR;
                myAnim.Play("Running Slide");
            }
            //Fast Fall Input
            else if (isGrounded == false)
            {
                if (Input.GetAxis("Vertical") < 0 && noHolding == true)
                {
                    FastFall();
                    noHolding = false;
                }
            }
        }
        else if (state == Grounded.RIGHT)
        {
            RightSetup();       //Setting up Right Gravity and other Misc.
            //Change directions input
            if (Input.GetAxis("Horizontal") < 0 && (isGrounded))
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                state = Grounded.AIR;
                copy = Copy.LEFT;
            }
            else if (Input.GetAxis("Vertical") < 0 && (isGrounded))
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                state = Grounded.AIR;
                copy = Copy.FLOOR;
            }
            //Sliding Input
            else if (Input.GetAxis("Horizontal") > 0 && (isGrounded))
            {
                time = 0.75f;
                state = Grounded.SLIDE;
                copy = Copy.RIGHT;
                myAnim.Play("Running Slide");
            }
            //Jumping Input
            else if (Input.GetButtonDown("Jump") && (isGrounded))
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            //Fast Fall Input
            else if (isGrounded == false)
            {
                if (Input.GetAxis("Horizontal") > 0 && noHolding == true)
                {
                    FastFall();
                    noHolding = false;
                }
            }

        }
        else if (state == Grounded.LEFT)
        {
            LeftSetup();        //Setting up Left Gravity and other Misc.
            //Change directions input
            if (Input.GetAxis("Vertical") < 0 && (isGrounded))
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                state = Grounded.AIR;
                copy = Copy.FLOOR;
            }
            else if (Input.GetAxis("Horizontal") > 0 && (isGrounded))
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                state = Grounded.AIR;
                copy = Copy.RIGHT;
            }
            //Sliding Input
            else if (Input.GetAxis("Horizontal") < 0 && (isGrounded))
            {
                time = 0.75f;
                state = Grounded.SLIDE;
                copy = Copy.LEFT;
                myAnim.Play("Running Slide");
            }
            //Jumping Input
            else if (Input.GetButtonDown("Jump") && (isGrounded))
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            //Fast Fall Input
            else if (isGrounded == false)
            {
                if (Input.GetAxis("Horizontal") < 0 && noHolding == true)
                {
                    FastFall();
                    noHolding = false;
                }
            }
        }
        else if (state == Grounded.AIR) 
        {
            // Timer set up before switching 
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else 
            {
                state = (Grounded)copy;
                time = jumpTime;
            }
            // Change to the previously mentioned direction
        }
        else if (state == Grounded.SLIDE) 
        {
            // timer set up to reset the hight position
            if (time > 0)
            {
                time -= Time.deltaTime;
                playerCollider.height = 1;
            }
            else
            {
                state = (Grounded)copy;
                time = .5f;
                playerCollider.height = 2;

            }
            // commnds to recet the character hight andd animation
            // if they press a button early
        }
*/


