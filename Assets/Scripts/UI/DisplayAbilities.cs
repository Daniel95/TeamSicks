using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAbilities : MonoBehaviour
{
    [SerializeField]
    Button[] abilityButtons;

    public void UpdateAbilities(BaseAbility[] _abilities)
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            Image[] _holderSprite = abilityButtons[i].GetComponentsInChildren<Image>();
            Text[] _holderText = abilityButtons[i].GetComponentsInChildren<Text>();

            _holderText[0].text = _abilities[i].UITextGetter();
            _holderSprite[2].sprite = _abilities[i].UIImageGetter();
            _holderSprite[2].color = Color.white;
        }
    }
}