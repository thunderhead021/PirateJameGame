public class DisplayInfo
{
    public string name;
    public TargetType targetType;

    public DisplayInfo(string name, TargetType targetType) 
    {
        this.name = name;
        this.targetType = targetType;
    }

    public DisplayInfo()
    {
        this.name = "";
        this.targetType = TargetType.None;
    }
}
