using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Calculations : MonoBehaviour
{
    public struct FractionStruct 
    {
        public long x;
        public long y;
        public FractionStruct(long xx, long yy)
        : this()
        {
            this.x = xx;
            this.y = yy;
        }
    };
    public FractionStruct FS; //This is used in CalculatorButtons.cs
    public List<FractionStruct> fractList = new List<FractionStruct>();
    public List<char> symbolList;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public FractionStruct calculate(List<GameObject> uiElements)
    {
        /*
        Idea:
        One symbol [i](symbol list) is used with [i] element in fraction list -> (+,-,/,*) -> [i+1] element in fraction list
        After calculations symbol is deleted and i element from fractionList is deleted, i+1 element updated with new value
        Steps:
        1. First symbols and fractions is seperated to difrent list. Fractions input is formated to struct 
        2. Then first for is for multi and division, because they have priority
        3. And second for is for minus and plus
        4. Format to lowest value possible and save
        5. After all calculations only one Vector left in vector list and thisvalue is returned to equal button function where it was called 
        */

        //Add fraction to struct_fraction list and symbol to char_symbol list
        foreach (GameObject i in uiElements) 
        {
            if(i.name == "Fraction_UI(Clone)")
            {
                long x = long.Parse(i.transform.GetChild(0).GetComponent<TMP_InputField>().text);
                long y = long.Parse(i.transform.GetChild(1).GetComponent<TMP_InputField>().text);

                //Formating number to lowest form
                long d = __gcd(x, y);

                fractList.Add(new FractionStruct((x/d),(y/d)));
            }
            else
            {
                symbolList.Add(i.GetComponent<TMP_Text>().text[0]);
            }
        }

        //Priority multi and div
        for(int i = 0; i < symbolList.Count; i++)
        {
            if(symbolList[i] == '*' || symbolList[i] == '/'){
            i =callCalculations(i);
            }
            
        }

        //Add and minus calculations
        for(int i = 0; i < symbolList.Count; i++)
        {
            i =callCalculations(i);
        }


        if(fractList.Count == 1)
        {
            FractionStruct answer = fractList[0];

            //Clear lists
            symbolList.Clear();
            fractList.Clear();

            return (answer); //Answer return
        }
        else
        {
            //Clear lists
            symbolList.Clear();
            fractList.Clear();
            
            FractionStruct ifWronganswer;
            ifWronganswer.x =0;
            ifWronganswer.y =0;
            return ifWronganswer; //Default value if something goes wrong with code
        }
    }

    //Operations // x is top element in fraction, y is bottom element in fraction

    //calculate numbers
    public FractionStruct calculate_Number(FractionStruct first, FractionStruct second, char symbol) 
    {

        //Default value
        FractionStruct answer;
        answer.x = 0;
        answer.y = 0;

        //Veriables for easier use
        long fx = first.x;
        long fy = first.y;
        long sx = second.x;
        long sy = second.y;

        if(symbol == '+'){
            if(fy == sy)
            {
                answer.x = fx + sx;
                answer.y = fy;
            }
            else
            {
                fx *= sy;
                sx *= fy;

                //Finding the the lowest form
                long dtemp = __gcd((fx + sx), fy*sy);
                long x = (fx + sx)/dtemp;
                long y = (fy*sy)/dtemp;

                answer.x = x;
                answer.y = y;
            }
        }
        else if(symbol == '-')
        {
            if(fy == sy)
            {
                answer.x = fx - sx;
                answer.y = fy;
            }
            else
            {
                fx *= sy;
                sx *= fy;

                //Finding the the lowest form
                long dtemp = __gcd((fx - sx), fy*sy);
                long x = (fx - sx)/dtemp;
                long y = (fy*sy)/dtemp;

                answer.x = x;
                answer.y = y;
            }
        }
        else if(symbol == '*')
        {
            answer.x = fx * sx;
            answer.y = fy * sy;
        }
        else if(symbol == '/')
        {
            answer.x = fx * sy;
            answer.y = fy * sx;
        }
        
        //Format numbers to lowest form and enter to fraction_struct new value
        long d =  __gcd(answer.x, answer.y);

        answer.x = answer.x/d;
        answer.y = answer.y/d;
        return answer;
    }
    private int callCalculations(int i)
    {
            //Calling calculation 
            FractionStruct answer = calculate_Number(fractList[i], fractList[i+1], symbolList[i]);
            
            //Deleting fraction_struct i and updating value in fraction_struct i+1, deleting symbol that was used
            fractList[i+1] = answer;
            fractList.Remove(fractList[i]);
            symbolList.Remove(symbolList[i]);
            
            i = i-1;//reset i value

            return i;
    }
    
   
    static long __gcd(long a, long b)
    {
        if (b == 0)
            if (a == 0)
                return 1;
            else
                return a;
        return __gcd(b, a % b);
        
    }
}
