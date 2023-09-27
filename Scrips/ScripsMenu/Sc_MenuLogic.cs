using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_MenuLogic : MonoBehaviour
{
    #region Varables
    public enum Screens
    {
        MainMenu, Loading, Options, Multiplayer, Student_info, Singel_play,Loading_Multiplayer
    };
    private Dictionary<string, GameObject> unityObjects;
    Stack <Screens> screen_name = new Stack<Screens>();
    private Screens curScreen;
    private Screens prevScreen;
    int count = 0;
    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        
        InitLogic();
    }
    #endregion

    #region Logic
    private void Init()
    {
        curScreen = Screens.MainMenu;
        prevScreen = Screens.MainMenu;
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] _unityObj = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach (GameObject g in _unityObj)
        {
            if (unityObjects.ContainsKey(g.name) == false)
                unityObjects.Add(g.name, g);
            else Debug.LogError("This key " + g.name + " Is Already inside the Dictionary!!!");
        }
    }

    private void InitLogic()
    {
        if (unityObjects.ContainsKey("Screen_Loading"))
        {
            unityObjects["Screen_Loading"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_Options"))
        {
            unityObjects["Screen_Options"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_Student_info"))
        {
            unityObjects["Screen_Student_info"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_Multiplayer"))
        {
            unityObjects["Screen_Multiplayer"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_Singel_play"))
        {
            unityObjects["Screen_Singel_play"].SetActive(false);
        }
        if (unityObjects.ContainsKey("Screen_Loading_Multiplayer"))
        {
            unityObjects["Screen_Loading_Multiplayer"].SetActive(false);
        }

    }
    public void ChangeScreen(Screens _ToScreen)
    {
        prevScreen = curScreen;
        curScreen = _ToScreen;

        if (prevScreen == Screens.MainMenu && curScreen == Screens.Loading)
        {
            curScreen = Screens.Singel_play;
            prevScreen = Screens.Loading;
            unityObjects["Screen_" + Screens.MainMenu].SetActive(false);
            unityObjects["Img_Background"].SetActive(false);
            
        }
       
        else if (unityObjects.ContainsKey("Screen_" + prevScreen))
        {
            unityObjects["Screen_" + prevScreen].SetActive(false);
        }
            
           
        if (unityObjects.ContainsKey("Screen_" + curScreen))
        {
            unityObjects["Screen_" + curScreen].SetActive(true);
        }
            
    }
    #endregion

    #region Stack
    public void pop_stack()
    {
        if (count > 0)
        {
            screen_name.Pop();
            count--; 
        }

        if (count > 0)
        {
            ChangeScreen(screen_name.Peek());
        }
        else
        {
            ChangeScreen(Screens.MainMenu); 
        }
    }

    public void push_stack(Screens newScreen)
    {
        screen_name.Push(newScreen);
        count++;
    }

    #endregion
}
