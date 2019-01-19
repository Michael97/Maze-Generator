/// <summary>
/// This enum defines all the possible tile types 
/// for our random walker map generator.
/// 
/// We specify that impassable = 0, so that this will be the default value,
/// the default value of an enum is always whatever value = 0, as underneath
/// the hood, enums are just ints in C# and 0 is the default value of an int.
/// For consistency, we also specify the value of passable - although we could
/// have let that automatically determine it's value.
/// If you do not specify which element is 0, it will be the first element.
/// </summary>
enum TileTypes {
    Impassable = 0,
    Passable = 1,
    RenderImpassable = 2,
    CarpetPassable = 3
}