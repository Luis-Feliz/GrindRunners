using UnityEngine;


public class MovementScript : MonoBehaviour
{
    public GameObject obsticles;                    // Game Object being manipulated
    public static float speed;                      // Speed in which the Game object will be moving

    // Loctaion where Value will be moved to
    Vector3 location = new Vector3(0, 0, -50);
    
    
    public void Start()
    {
        speed = 10;
    }

    public void Update()
    {
        //Reassigning the current game object position to the Vector "Location" we desire 
        obsticles.transform.position = Vector3.MoveTowards(obsticles.transform.position, location, speed * Time.deltaTime);
    }
}
