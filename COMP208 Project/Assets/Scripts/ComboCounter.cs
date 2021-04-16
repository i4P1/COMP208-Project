using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    private int comboCount;
    //Animator animator;

    public void updateComboCount(int amount = 1) {
        comboCount += amount;

        //animator.updateComboCounter(comboCount, heavy)
    }

    public void updateComboCount(bool reset, bool heavy = false) {
        comboCount = 0;
        //animator.updateComboCounter(comboCount, heavy)
    }

    public int getComboCount() {
        return comboCount;
    }
}
