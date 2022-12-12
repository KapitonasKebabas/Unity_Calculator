using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;

public class CalculatorButtons : MonoBehaviour
{
    public struct FractionStruct 
    {
        public long x;
        public long y;
    };
    //Default start position for printig options in canvas
    private Vector2 DefaultstartPosition = new Vector2(200.0f,350.0f);
    //GameObjects_Buttons
    public GameObject fractionPref;
    public GameObject plusPref;
    public GameObject minusPref;
    public GameObject divPref;
    public GameObject multiPref;
    public GameObject eqPref;
    public GameObject answerPref;
    public GameObject operationObject;
    public GameObject fracButton;
    public GameObject eqButton;
    public GameObject answerDivisionByZero;
    public GameObject answerZero;
    public int maxFractions = 6;

    //List for pressed options
    public List<GameObject> uiElements;
    void Start()
    {
        operationsEnable(false);
        eqEnable(false);
    }
    void Update()
    {
        if(uiElements.Count >= (maxFractions*2)-1)
        {
            operationsEnable(false);
            fractionEnable(false);
        }
        Vector2 startPosition = DefaultstartPosition;

        //Print in canvas selected options
        foreach (GameObject i in uiElements) 
        {
            i.transform.position = startPosition;
            startPosition.x += 50f;
            i.transform.position = startPosition;
        }

        //Reset position
        startPosition = DefaultstartPosition;
    }
    
    public void fraction_add_UI()
    {
        //Add to list fraction
        GameObject newFrac = GameObject.Instantiate(fractionPref);
        newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);
        uiElements.Add(newFrac);
    }
    public void plus_add_UI()
    {
        //Add to list plus symbol
        GameObject newFrac = GameObject.Instantiate(plusPref);
        newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);
        uiElements.Add(newFrac);
    }
    public void minus_add_UI()
    {
        //Add to list minus symbol
        GameObject newFrac = GameObject.Instantiate(minusPref);
        newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);
        uiElements.Add(newFrac);
    }
    public void div_add_UI()
    {
         //Add to list div symbol
        GameObject newFrac = GameObject.Instantiate(divPref);
        newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);
        uiElements.Add(newFrac);
    }
    public void multi_add_UI()
    {
         //Add to list multi symbol
        GameObject newFrac = GameObject.Instantiate(multiPref);
        newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);
        uiElements.Add(newFrac);
    }
    public void eq_add_UI()
    {
        GameObject newFrac = GameObject.Instantiate(eqPref);
        newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);

        //Calculation call
        this.gameObject.GetComponent<Calculations>().FS = this.gameObject.GetComponent<Calculations>().calculate(uiElements);
        uiElements.Add(newFrac);

        //Answer add
        if(this.gameObject.GetComponent<Calculations>().FS.y == 0)
        {
            newFrac = GameObject.Instantiate(answerDivisionByZero);
            newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);
        }
        else if(this.gameObject.GetComponent<Calculations>().FS.x == 0)
        {
            newFrac = GameObject.Instantiate(answerZero);
            newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);
        }
        else{
            newFrac = GameObject.Instantiate(answerPref);
            newFrac.transform.SetParent(GameObject.Find("CalculatorPanel").transform);

            //check if fraction is minus
            bool minusAdd = false;
            if(this.gameObject.GetComponent<Calculations>().FS.x < 0)
            {
                this.gameObject.GetComponent<Calculations>().FS.x *= -1;
                minus_add_UI();
                minusAdd = true;
            }
            //checking y for minus
            if(this.gameObject.GetComponent<Calculations>().FS.y < 0)
            {
                this.gameObject.GetComponent<Calculations>().FS.y *= -1;
                if(!minusAdd){
                minus_add_UI();
                }
            }


            newFrac.transform.GetChild(0).GetComponent<TMP_Text>().text = (this.gameObject.GetComponent<Calculations>().FS.x).ToString();
            newFrac.transform.GetChild(1).GetComponent<TMP_Text>().text = (this.gameObject.GetComponent<Calculations>().FS.y).ToString();
        }

        uiElements.Add(newFrac);
    }

    public void operationsEnable(bool enable)
    {
        for(int i = 0; i<operationObject.transform.childCount; i++)
        {
            operationObject.transform.GetChild(i).GetComponent<Button>().enabled = enable;
        }
    }
    public void fractionEnable(bool enable) {
        fracButton.GetComponent<Button>().enabled = enable;
    }
    public void eqEnable(bool enable) {
        eqButton.GetComponent<Button>().enabled = enable;
    }
    public void enableAll(bool enable) {
        {
            operationsEnable(enable);
            fractionEnable(enable);
        }
    }
    public void clear()
    {
        foreach (GameObject i in uiElements) 
        {
            Destroy(i);
        }
        fractionEnable(true);
        operationsEnable(false);
        uiElements.Clear();
    }
     public void backspace()
    {
        if(uiElements.Count > 0){
        Destroy( uiElements[uiElements.Count-1]);
        uiElements.RemoveAt( uiElements.Count-1 );
        enableAll(true);
        }

    }

}
