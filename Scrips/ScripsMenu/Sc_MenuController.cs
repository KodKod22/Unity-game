using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sc_MenuLogic;

public class Sc_MenuController : MonoBehaviour
{
    #region Varables
    public Sc_MenuLogic curMenuLogic;
    string key_name = "Back";
    #endregion

    #region Controller
    public void Btn_ChangeScreen(string _ScreenName)
    {
        
        if (curMenuLogic != null)
        {
            if (_ScreenName == key_name)
            {
                curMenuLogic.pop_stack();
            }
            else
            {
                try
                {
                    Screens _toScreen = (Screens)Enum.Parse(typeof(Screens), _ScreenName);
                    curMenuLogic.ChangeScreen(_toScreen);
                    curMenuLogic.push_stack(_toScreen);
                }
                catch (Exception e)
                {
                    Debug.LogError("Fail to convert: " + e.ToString());
                }
            }
        }
    }
    public void Btn_Play()
    {
        Sc_Multiplayer.Instance.Btn_PlayLogic();
    }
    #endregion
}
