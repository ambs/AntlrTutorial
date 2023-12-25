namespace Logo2Svg.AST;

public class Method : INode
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
    

    public void Execute(Turtle turtle, List<ExprParam> parameters)
    {
        
    }
    
    public void Execute(Turtle turtle) => throw new NotImplementedException();
}