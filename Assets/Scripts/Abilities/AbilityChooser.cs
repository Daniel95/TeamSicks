using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DisplayAbilities))]
public class AbilityChooser : MonoBehaviour {

    private const int TOTAL_NEW_ABILITIES = 3;
    DisplayAbilities displayAbilities;

    BaseAbility[] baseAbilities = new BaseAbility[TOTAL_NEW_ABILITIES];

    void Awake()
    {
        displayAbilities = GetComponent<DisplayAbilities>();
    }

    void NewAbilities()
    {
        for (int i = 0; i < baseAbilities.Length; i++)
        {
            if (baseAbilities[i] != null)
            {
                baseAbilities[i].OnDestroy();
            }
            baseAbilities[i] = (GetRandomAbility());
            baseAbilities[i].UIIndexGetSet = i;
        }

        displayAbilities.UpdateAbilities(baseAbilities);
    }

    public BaseAbility GetRandomAbility()
    {

        int _random = Random.Range(0, 2);
        BaseAbility _baseAbility;

        if (_random == 0)
        {
            _baseAbility = new RedirectAbility();
        }
        else
        {
            _baseAbility = new StunAbility();
        }

        _baseAbility.OnGenerate();

        return _baseAbility;
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
