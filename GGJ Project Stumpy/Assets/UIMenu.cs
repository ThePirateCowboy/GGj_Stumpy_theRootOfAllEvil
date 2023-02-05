using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public string levelToLoadOnStart = "Test Movement";
    public Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene()
    {
        anim.SetTrigger("FadeOut");
        LoadSceneAsync(levelToLoadOnStart);
        
    }

    public void Quitting()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneAsync(string levelToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(.8f);
        
        
    }
    
}
