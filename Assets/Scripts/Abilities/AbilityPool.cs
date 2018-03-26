using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AbilityPool : MonoBehaviour
{
    #region Singleton
    public static AbilityPool Instance { get { return GetInstance(); } }

    private static AbilityPool instance;

    private static AbilityPool GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AbilityPool>();
        }
        return instance;
    }
    #endregion

    [SerializeField]
    List<Ability> allAbilities;

    List<Ability> availableAbilites;

    List<Ability> usedAbilities = new List<Ability>();

    void Awake()
    {
        availableAbilites = allAbilities;
    }

    public Ability GetRandomAbility()
    {
        Ability _ability = availableAbilites[Random.Range(0, availableAbilites.Count)];

        availableAbilites.Remove(_ability);
        usedAbilities.Add(_ability);
        
        if (availableAbilites.Count <= 0)
        {
            foreach (Ability _usedAbility in usedAbilities)
            {
                availableAbilites.Add(_usedAbility);
            }
            usedAbilities.Clear();
        }

        return _ability;
    }
}