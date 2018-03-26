using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /*
    [SerializeField]
    List<Ability> allAbilities;

    List<Ability> availableAbilites;

    List<Ability> usedAbilities = new List<Ability>();
    */

    void Awake()
    {
        //availableAbilites = allAbilities;
    }

    public BaseAbility GetRandomAbility()
    {

        int _random = Random.Range(0,2);
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
}