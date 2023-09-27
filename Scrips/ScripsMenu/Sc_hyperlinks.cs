using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_hyperlinks : MonoBehaviour
{
    public void OpenURL(string link)
    {
        Application.OpenURL(link);
    }
}
