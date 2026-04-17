public struct Id
{
    public int x;
    public int y;
    private bool isInitialized;
    public bool isNull => !isInitialized;
    public static readonly Id zero = new Id(0,0);

    public Id (int x, int y)
    {
        this.x = x;
        this.y = y;
        isInitialized = true;
    }
}