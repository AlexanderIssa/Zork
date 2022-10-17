namespace Zork
{
    //an enumeration is a collected list of names, can't have the same name in the same enum, PURPOSE IS ONLY TO CREATE A LIST OF RELATED NAMES
    //this enum is related because they are all commands for our Zork game
    //enum is a data type
    public enum Commands //enumeration (enum) is a pascal case
    {
        Quit = 0, //don't need = 0, does it automatically
        Look, //1
        North, //2..
        South,
        East,
        West,
        Uknown,
        Score,
        Reward
    }
}