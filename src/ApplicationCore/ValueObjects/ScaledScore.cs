namespace ApplicationCore.ValueObjects;

public class ScaledScore
{
    public CultureScore Collaborate { get; set; }
    public CultureScore Create{ get; set; }
    public CultureScore Compete{ get; set; }
    public CultureScore Control{ get; set; }
    
    public ScaledScore(CultureScore collaborate, CultureScore create, CultureScore compete, CultureScore control)
    {
        Collaborate = collaborate;
        Create = create;
        Compete = compete;
        Control = control;
    }
}