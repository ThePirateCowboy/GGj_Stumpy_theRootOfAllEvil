using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisableAfterSeconds : MonoBehaviour
{
    public float timeTillinActive;
    public string leafingAround = "Try Again!";
    public TextMeshProUGUI Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        
        yield return new WaitForSeconds(timeTillinActive);
        
        Text.text = leafingAround;
        yield return new WaitForSeconds(timeTillinActive);

        Text.text = "YOU'RE DEAD!!!!";
        gameObject.SetActive(false);
        yield break;
    }
}
