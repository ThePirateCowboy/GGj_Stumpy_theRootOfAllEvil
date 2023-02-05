using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public float timeTillRespawn = 1.5f;
    private bool hasBeenTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag ==  "Player" && !hasBeenTriggered)
        {
            StartCoroutine(WaitThenRespawn());
            hasBeenTriggered = false;
        }
    }
    IEnumerator WaitThenRespawn()
    {
        yield return new WaitForSeconds(timeTillRespawn);
        hasBeenTriggered = true;
        RespawnController.instance.Respawn();
    }
}
