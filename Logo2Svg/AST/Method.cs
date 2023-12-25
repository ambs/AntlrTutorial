namespace Logo2Svg.AST;

public class Method
{
    private string _name;
    private int _arity;
    private List<string> _parameters;
    private Program _code;

    public Method(string name, List<string> parameters, List<Command> program)
    {
        _name = name;
        _parameters = parameters;
        _arity = _parameters.Count;
        _code = new Program(program);
    }
    
    public string Name => $"{_name.ToLowerInvariant()}/{_arity}";

    public void Execute(Turtle turtle, List<ExprParam> parameters)
    {
        
    }
}