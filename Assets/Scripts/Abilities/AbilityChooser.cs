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
        List<BaseAbility> baseAbilities = new List<BaseAbility>();
        for (int i = 0; i < totalNewAbilities; i++)
        {
            baseAbilities.Add(AbilityPool.Instance.GetRandomAbility());
        }

        displayAbilities.UpdateAbilities(baseAbilities.ToArray());
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
