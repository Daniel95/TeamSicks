using UnityEngine;

/// <summary>
/// StunAbility inheritance from the BaseAbility class
/// </summary>
public class StunAbility : BaseAbility
{
	/// <summary>
	/// OnGenerate loads the sprite for the stunAbility.
	/// After it activates base.OnGenerate();
	/// </summary>
    public override void OnGenerate()
    {
        UIText = "";
        UIImage = Resources.Load<Sprite>("Stun");

        base.OnGenerate();
    }
}