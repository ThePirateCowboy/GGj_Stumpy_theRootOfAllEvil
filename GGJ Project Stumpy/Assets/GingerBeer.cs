using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GingerBeer : MonoBehaviour
{
    public GameObject PickUpEffect;
    private Collider2D coll;
    // Start is called before the first frame update
    private void OnEnable()
    {
        coll = GetComponent<Collider2D>();
    }
    void Start()
    {
        StartCoroutine(CountdownToDestroy());
        StartCoroutine(InitiatePickUp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator InitiatePickUp()
    {
        yield return new WaitForSeconds(0.5f);
        coll.enabled = true;
        yield break;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController.instance.UpdateRootbeerAmountAndUI();
            //Instantiate(PickUpEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    IEnumerator CountdownToDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        yield break;

    }
}
