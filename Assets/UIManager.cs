using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text timeText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = TimeManager.instance.getCurrentDay() + ":  " + TimeManager.instance.getCurrentHour().ToString("00") + ":" + TimeManager.instance.getCurrentMinute().ToString("00");
    }
}
