using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAbility : BaseAbility
{
    public override void OnGenerate()
    {
        UIText = "";
        UIImage = Resources.Load<Sprite>("Stun");

        base.OnGenerate();
    }

    public override void OnClick()
    {
        base.OnClick();
    }
}