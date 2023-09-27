using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sc_volume : MonoBehaviour
{
    #region Varables
    private AudioSource audioSrc;
    private float musicVolume = 1f;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        audioSrc.volume = musicVolume;
        
    }
    #endregion

    #region Volume_set
    public void setVolume(float valume1)
    {
         musicVolume = valume1;
    }

    public void Slider_volume()
    {
        GameObject.Find("Txt_Valume").GetComponent<TextMeshProUGUI>().text = GameObject.Find("Slider_Valume").GetComponent<Slider>().value.ToString();
    }
    #endregion
}
