using UnityEngine;

/// <summary>
/// AbilityChooser checks what for abilities there are in the game (redirect or stun etc) and randomly sets the abilities to the max abilities.
/// Meaning that you can have a maximum of 'number' and that the abilities will be randomly set to the maximum number.
/// </summary>
[RequireComponent(typeof(DisplayAbilities))]
public class AbilityChooser : MonoBehaviour {

    private const int TOTAL_NEW_ABILITIES = 3;
    private DisplayAbilities displayAbilities;
	private  BaseAbility[] baseAbilities = new BaseAbility[TOTAL_NEW_ABILITIES];

	/// <summary>
	/// GetRandomAbility() sets all the abilities with a random value and calls an event _baseAbility.OnGenerate();
	/// </summary>
	/// <returns></returns>
	public BaseAbility GetRandomAbility()
	{

		int _random = Random.Range(0, 0);
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

	private void Awake()
    {
        displayAbilities = GetComponent<DisplayAbilities>();
    }

    private void NewAbilities()
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

    private void OnEnable()
    {
        AbilityPlacement.OnEnableButtons += NewAbilities;
    }

    private void OnDisable()
    {
        AbilityPlacement.OnEnableButtons -= NewAbilities;
    }
}