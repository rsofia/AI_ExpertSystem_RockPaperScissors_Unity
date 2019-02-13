using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesNode
{
    public struct OutCome
    {
        public int outcome;
        public int numberOfSuccess;

        public void ConditionSuccessful(bool _success)
        {
            if (_success)
                numberOfSuccess++;
            else
                numberOfSuccess--;
        }
    }

    //This class is a Vector3 Kind of Thing to store the SystemExpert's possibilities
    public int a1;
    public int a2;
    public OutCome[] outcomes = new OutCome[3];
    //public int outcome;
    //public int numberOfSuccess;

    public StatesNode(int _a1, int _a2)
    {
        a1 = _a1;
        a2 = _a2;

        //rellenar possible outcomes
        for(int i = 0; i < 3; i++)
        {
            outcomes[i].outcome = i+1;
            outcomes[i].numberOfSuccess = 0;
        }

    }

    //public StatesNode(int _a1, int _a2, int _c)
    //{
    //    a1 = _a1;
    //    a2 = _a2;
    //    //outcome = _c;
    //}


    public StatesNode()
    {
        a1 = 0;
        a2 = 0;
        //outcome = 0;
        //numberOfSuccess = 0;
    }

    public void InsertInfo(int _a1, int _a2, int _c)
    {
        a1 = _a1;
        a2 = _a2;
        //outcome = _c;
    }

    public bool CheckCondition(int _a1, int _a2)
    {
        bool result = false;
        if((a1 == _a1) && (a2 == _a2))
        {
            result = true;
        }

        return result;
    }

    



}
