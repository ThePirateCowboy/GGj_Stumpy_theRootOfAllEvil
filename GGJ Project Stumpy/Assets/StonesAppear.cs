using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesAppear : MonoBehaviour
{

    public GameObject StoneA, StoneB, StoneC, StoneD;

    public int threshhold = 5;
    public int current;
    public int stage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(current>= threshhold)
        {
            if (stage == 0)
            {
                stage++;
                current = 0;
                StoneA.SetActive(true);
            }
            else if (stage == 1)
            {
                stage++;
                current = 0;
                StoneB.SetActive(true);
            }else 
            if (stage == 2)
            {
                stage++;
                current = 0;
                StoneC.SetActive(true);
            }else 
            if (stage == 3)
            {
                stage++;
                current = 0;
                StoneD.SetActive(true);
            }
        }
        if(RespawnController.instance.StoneHenge)
        {
            SetStoneHenge();
        }
    }
    
    public void SetStoneHenge()
    {
        StoneA.SetActive(true);
        StoneC.SetActive(true);
        StoneD.SetActive(true);
        StoneB.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            current += other.GetComponent<EnemyHealth>().enemyValue;
            Destroy(other.gameObject);
        }
    }
}
