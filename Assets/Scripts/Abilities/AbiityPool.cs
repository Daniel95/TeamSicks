using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AbiityPool : MonoBehaviour
{
    #region Singleton
    public static AbiityPool Instance { get { return GetInstance(); } }

    private static AbiityPool instance;

    private static AbiityPool GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AbiityPool>();
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