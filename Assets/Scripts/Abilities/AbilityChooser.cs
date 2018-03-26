using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DisplayAbilities))]
public class AbilityChooser : MonoBehaviour {

    private int totalNewAbilities = 3;
    DisplayAbilities displayAbilities;

    void Awake()
    {
        displayAbilities = GetComponent<DisplayAbilities>();
    }

    void NewAbilities()
    {
        Ability[] _newAbilities = new Ability[totalNewAbilities];
        for (int i = 0; i < totalNewAbilities; i++)
        {
            //Debug.Log("Hmmm?");
            _newAbilities[i] = AbilityPool.Instance.GetRandomAbility();
        }

        displayAbilities.UpdateAbilities(_newAbilities);
    }

    void OnEnable()
    {
        EndTurnButton.PlayerTurnStartedEvent += NewAbilities;
    }

    void OnDisable()
    {
        EndTurnButton.PlayerTurnStartedEvent -= NewAbilities;
    }
}
