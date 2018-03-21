using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPool : MonoBehaviour
{
    [SerializeField]
    List<Ability> totalAbilities;

    List<Ability> availableAbilities;

    List<Ability> usedAbilities = new List<Ability>();

    void Awake()
    {
        availableAbilities = totalAbilities;
    }

    public Ability GetRandomAbility()
    {
        Ability _ability = availableAbilities[Random.Range(0, availableAbilities.Count - 1)];

        availableAbilities.Remove(_ability);

        if (availableAbilities.Count <= 0)
        {
            availableAbilities = usedAbilities;
            usedAbilities.Clear();
        }

        return _ability;
    }
}