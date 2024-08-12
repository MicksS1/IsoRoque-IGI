using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider HPslider;
    public TMP_Text hpNum;
    private int hpNumValue;

    // Start is called before the first frame update
    void Start()
    {
        HPslider = GetComponent<Slider>();
        //hpNum = GameObject.FindGameObjectWithTag("HPnumber").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        hpNumValue = (int)HPslider.value;
        hpNum.text = hpNumValue.ToString();
        //Debug.Log(hpNumValue);
    }

    public void setMaxHP(int hp)
    {
        HPslider.maxValue = hp;
        HPslider.value = hp;
    }

    public void setHP(int hp)
    {
        HPslider.value = hp;
    }
}
