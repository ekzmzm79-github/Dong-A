using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click_StartButton()
    {
        LoadingSceneManager.LoadScene("Village");
        
    }
    public void Click_ContinueButton()
    {
        LoadingSceneManager.LoadScene("Village");
        
    }
    public void Click_SettingButton()
    {

    }
    public void Click_ExitButton()
    {

    }
}
