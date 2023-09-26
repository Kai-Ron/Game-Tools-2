using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnlockCode : MonoBehaviour
{
    public string code = "******";
    public TMP_Text display;
    public GameObject winScreen;

    
    // Start is called before the first frame update
    void Start()
    {
        display.text = code;
        winScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TypeCode(int number)
    {
        if (code.Length == 6)
        {
            ResetCode();
            code += number;
            display.text = code;
        }
        else
        {
            code += number;
            display.text = code;
        }
    }

    public void EnterCode(string input)
    {
        if (code == input)
        {
            winScreen.SetActive(true);
        }
        else
        {
            code = "";
            display.text = code;
        }
    }

    public void ResetCode()
    {
        code = "";
        display.text = code;
    }
}
