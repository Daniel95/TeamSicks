using System.Collections.Generic;

public static class Tags {

    public static List<string> CharacterTags { get { return new List<string>() { Player, Enemy }; } }

    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Killer = "Killer";
    public const string Destroyer = "Destroyer";
    public const string Bouncy = "Bouncy";
    public const string StartPoint = "Start";
    public const string CheckPoint = "Checkpoint";
    public const string Finish = "Finish";
    public const string MainCamera = "MainCamera";
    public const string JumpTrigger = "JumpTrigger";
    public const string ConvexCorner = "ConvexCorner";

}
