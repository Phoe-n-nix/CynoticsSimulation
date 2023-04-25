using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class navbtnclk : MonoBehaviour
{
    public enum Btnlist {logout, loginsubmit, signsubmit, studentselect, inredirect, upredirect }
    public Btnlist btl;
    public Button logout_btn, logsub_btn, sisub_btn, stdntlog_btn, inredirect_btn, upredirect_btn;
    Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        Debug.Log("Currently Active Scene is "+scene.name);
        
       
    }

    // Update is called once per frame
    void Update()
    {
        switch(scene.name)
        {
            case "Main Functionality Pages":
                if (Hmbrgr_Panel.activeInHeirarchy)
                {
                    Debug.Log("Hamburger panel is out. ");
                }
        };
    }
   
    void splashload()
    {
        SceneManager.LoadScene("Splash Screen");
    }
    void studentloginload()
    {
        SceneManager.LoadScene("Student Login");
    }

}