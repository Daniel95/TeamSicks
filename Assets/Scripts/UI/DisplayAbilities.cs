using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAbilities : MonoBehaviour
{
    [SerializeField]
    Button[] abilityButtons;

    public void UpdateAbilities(Ability[] _abilities)
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {   
            abilityButtons[i].GetComponent<Image>().sprite = _abilities[i].image.sprite;
        }
    }
}