using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float moveSpeed;
    public SpriteRenderer theSR;
    public GameObject waypoints;
    public bool movingRight;
    public Rigidbody2D theRB;
    public int current;
    public float WPradius;
    public float force;
    public float t;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            if (transform.position.x >=0)
            {
                movingRight = false;
                theSR.flipX = false;
            }
            else
            {
                theSR.flipX = true;
            }
       
        

        if(!PauseMenu.instance.isPaused)
        {
            if(waypoints !=null)
            {

                if (Vector3.Distance(waypoints.transform.position, transform.position) < WPradius)
                {
                        Destroy(gameObject);
                }
                Vector3 a = transform.position;
                Vector3 b = waypoints.transform.position;
                transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), moveSpeed);
            }
        }
    }

}

