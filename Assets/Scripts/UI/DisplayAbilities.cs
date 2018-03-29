using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the visual aspects of the ability buttons
/// </summary>
public class DisplayAbilities : MonoBehaviour
{

    [SerializeField] private Button[] abilityButtons;

    /// <summary>
    /// Updates the visuals of the ability ui buttons
    /// </summary>
    /// <param name="_abilities"></param>
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