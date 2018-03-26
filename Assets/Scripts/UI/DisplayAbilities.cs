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
            Image[] _holderSprite = abilityButtons[i].GetComponentsInChildren<Image>();
            _holderSprite[2].sprite = _abilities[i].image.sprite;
            _holderSprite[2].color = Color.white;
        }
    }
}