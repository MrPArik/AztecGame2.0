using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

 public bool PlayerIsAiming;

    [SerializeField] private Texture2D cursorTexture;   
    Vector2 cursorHotspot; 

    void Start()
    {
        Cursor.visible=false;
            Cursor.lockState=CursorLockMode.Locked;
        
        
        //cursorHotspot=new Vector2(cursorTexture.width/2,cursorTexture.height/2);
        //Cursor.SetCursor(cursorTexture,cursorHotspot,CursorMode.Auto);
        
    }
    private void Update() {
        if(Input.GetKey(KeyCode.Mouse1)){
            Cursor.visible=true;
            Cursor.lockState=CursorLockMode.None;
            PlayerIsAiming=true;
        }else{
            Cursor.visible=false;
            Cursor.lockState=CursorLockMode.Locked;
            PlayerIsAiming=false;
        }
    }

   
}
