namespace Logo2Svg.AST;

public class Method : Parameter
{
    private readonly string _name;
    private readonly int _arity;
    private readonly List<string> _parameters;
    public Program Code { get; }
    
    public Method(string name, List<string> parameters, List<Command> program)
    {
        _name = name;
        _parameters = parameters;
        _arity = _parameters.Count;
        Code = new Program(program);
    }
    
    public string Name => $"{_name.ToLowerInvariant()}/{_arity}";
    

    public void Execute(Turtle turtle, List<INode> parameters)
    {
        var args = parameters.Cast<Parameter>().Select(x => x.Value(turtle)).ToList();
        turtle.EnterScope();

        foreach (var (name, value) in _parameters.Zip(args, (a, b) => (a, b)))
        {
            turtle.DefineVariable(name, value);
        }

        try
        {
            Code.Execute(turtle);
        }
        catch (LogoStopException)
        {
            // recover
        }
        finally
        {
            turtle.ExitScope();
        }
    }

    public override float Value(Turtle turtle) => throw new NotImplementedException();
}