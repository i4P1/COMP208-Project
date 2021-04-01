using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    private int comboCount;
    //Animator animator;

    public void updateComboCount(int amount = 1, bool reset = false, bool heavy = false) {
        if(reset) comboCount = 0;
        else comboCount += amount;

        //animator.updateComboCounter(comboCount, heavy)
    }

    public int getComboCount() {
        return comboCount;
    }
}
