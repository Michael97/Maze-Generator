
class Node
{
    //Set up by having all the walls being true
    //This is so we can easily knock them down
    //during the algorithm
    public bool North = true;
    public bool South = true;
    public bool East = true;
    public bool West = true;

    //Default value = false
    public bool HasBeenVisited = false;

    public int x;
    public int y;

    public Node(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public int WallCheck()
    {
        int i = 0;

        if (North)
            i++;

        if (South)
            i++;

        if (East)
            i++;

        if (West)
            i++;

        return i;
    }
};