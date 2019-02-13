using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemExpert : MonoBehaviour {
    public Sprite[] rockPaperScissors;
    public Image imgSystemExpert;
    public Image imgRandomAI;
    public Text txtWinner;
    public Text txtWinsAI;
    public Text txtWinsRandom;
    public Text txtWinsNoOne;

    public Text txtChangeMatch;

    bool isExpertTurn = false;

    StatesNode[] possibleStatesArray;

    //Estos son los movimientos del random
    int movAAnterior = 0;
    int movAnterior = 0;
    int movActual = 0;
    //mov sistem expert
    int movPrediccion = 0;

    int sizeOfChances = 9;


    int contadorGanadasAI = 0;
    int contadorGanadasRandom = 0;
    int contadorNadieGano = 0;

    void Start()
    {
        possibleStatesArray = new StatesNode[sizeOfChances];
        FillPossibleStates();

        StartCoroutine(ManageTurns());
    }
	
    void FillPossibleStates()
    {        
        //possibleStatesArray[0].a1 = 0; possibleStatesArray[0].a2 = 0; possibleStatesArray[0].outcome = 0;
        int i = 0;
        for (int a1 = 1; a1 < 4; a1++)
        {
            for(int a2 = 1; a2 < 4; a2++)
            {
               // for(int c = 1; c < 4; c++)
                //{
                    //print("Insert at i " + i + " a1: " + a1 + " a2: ");
                    possibleStatesArray[i] = new StatesNode(a1, a2);
                    i++;
                //}
            }
        }
    }
    
    void RandomMove()
    {
        movAAnterior = movAnterior;
        movAnterior = movActual;

        movActual = Random.Range(1, 3);
        imgRandomAI.sprite = rockPaperScissors[movActual-1];
        print("RANDOM, went for " + movActual);

    }

    void ExpertMove()
    {       
        //Si no hay suficiente informormacion para actuar como sistema experto, comportarse como random
        if(movAAnterior == 0 || movAnterior == 0)
        {
            movPrediccion = Random.Range(1, 3);
            imgSystemExpert.sprite = rockPaperScissors[movPrediccion-1];
            print("Not enough info, Expert Goes random with " + movPrediccion);
            DoesExpertWin();
        }
        else
        {
            //SISTEMA EXPERTO
            int stateIndex = 0;

            //check for move
            for (int i = 0; i < sizeOfChances; i++)
            {
                if (possibleStatesArray[i].CheckCondition(movAAnterior, movAnterior))
                {
                    stateIndex = i;
                    break;
                }
            }

            //Get the outcome with the highest rate of succes
            int tempHighestIndex = 0;
            int tempHighestSuccess = possibleStatesArray[stateIndex].outcomes[tempHighestIndex].numberOfSuccess;
             for(int o = 0; o < 3; o++)
            {
                if(possibleStatesArray[stateIndex].outcomes[o].numberOfSuccess >= tempHighestSuccess)
                {
                    tempHighestSuccess = possibleStatesArray[stateIndex].outcomes[o].numberOfSuccess;
                    print("highest success found " + tempHighestSuccess);
                    tempHighestIndex = o;
                }
            }
            print("super highest found " + tempHighestSuccess);
            //Apply the highest rate of success
            movPrediccion = possibleStatesArray[stateIndex].outcomes[tempHighestIndex].outcome;
            //print("Movpreddiccion " + movPrediccion);
            imgSystemExpert.sprite = rockPaperScissors[movPrediccion-1];

            
            //Check if its a win win situation
            possibleStatesArray[stateIndex].outcomes[tempHighestIndex].ConditionSuccessful(DoesExpertWin());
            print("AI not random, went for " + movPrediccion + " because at index " + stateIndex  + " a1: " + possibleStatesArray[stateIndex].a1 + " a2: " + possibleStatesArray[stateIndex].a2 + " outcome: " + movPrediccion + " success: " + possibleStatesArray[stateIndex].outcomes[tempHighestIndex].numberOfSuccess);
        }

        //print("Does Expert win?" + DoesExpertWin());
    }

    bool DoesExpertWin()
    {
        bool result = false;
        switch(movPrediccion)
        {
            //rock
            case 1:
                {
                    switch(movActual)
                    {
                        case 1:
                            print("rock vs rock. no one wins");
                            txtWinner.text = "No one wins";
                            contadorNadieGano++;
                            result = false;
                            break;
                        case 2:
                            print("rock vs paper. Wins random");
                            txtWinner.text = "Wins random.";
                            contadorGanadasRandom++;
                            result = false;
                            break;
                        case 3:
                            print("rock vs scissors. Wins AI");
                            txtWinner.text = "Wins AI.";
                            contadorGanadasAI++;
                            result = true;
                            break;
                    }
                }
                break;
            //paper
            case 2:
                switch (movActual)
                {
                    case 1:
                        print("paper vs rock. wins AI");
                        txtWinner.text = "Wins AI.";
                        contadorGanadasAI++;
                        result = true;
                        break;
                    case 2:
                        print("paper vs paper. no one wins");
                        txtWinner.text = "No one wins";
                        contadorNadieGano++;
                        result = false;
                        break;
                    case 3:
                        print("paper vs scissors. Wins random");
                        txtWinner.text = "Wins random.";
                        contadorGanadasRandom++;
                        result = false;
                        break;
                }
                break;
            //scissors
            case 3:
                switch (movActual)
                {
                    case 1:
                        print("scissors vs rock. wins random");
                        txtWinner.text = "Wins random.";
                        contadorGanadasRandom++;
                        result = false;
                        break;
                    case 2:
                        print("scissors vs paper. wins AI");
                        txtWinner.text = "Wins AI.";
                        contadorGanadasAI++;
                        result = true;
                        break;
                    case 3:
                        print("scissors vs scissors. no one wins");
                        txtWinner.text = "No one wins";
                        contadorNadieGano++;
                        result = false;
                        break;
                }
                break;
        }

        UpdateStats();

        return result;
    }

    void UpdateStats()
    {
        txtWinsAI.text = contadorGanadasAI.ToString();
        txtWinsRandom.text = contadorGanadasRandom.ToString();
        txtWinsNoOne.text = contadorNadieGano.ToString();
    }

    IEnumerator ManageTurns()
    {
       // if (!isExpertTurn)
            RandomMove();
        //else
            ExpertMove();

        yield return new WaitForSeconds(2);
        txtChangeMatch.text = "Again!";
        yield return new WaitForSeconds(1);
        txtChangeMatch.text = "";

        // isExpertTurn = !isExpertTurn;

        //call turn again
        StartCoroutine(ManageTurns());
    }

    public void StartNewMatch()
    {
        //if (!isExpertTurn)
            RandomMove();
        ExpertMove();
        //else
        //    ExpertMove();

        
    }
}
