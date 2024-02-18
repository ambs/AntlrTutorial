using Logo2Svg.Turtle;

namespace Logo2Svg.AST.Nodes;

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

    public void Execute(TurtleState turtleState, List<INode> parameters)
    {
        var args = parameters.Cast<Parameter>().Select(x => x.Value(turtleState)).ToList();
        turtleState.EnterScope();

        foreach (var (name, value) in _parameters.Zip(args, (a, b) => (a, b)))
        {
            turtleState.DefineVariable(name, value);
        }

        try
        {
            Code.Execute(turtleState);
        }
        catch (LogoStopException) { /* recover */ }
        finally
        {
            turtleState.ExitScope();
        }
    }

    public override float Value(TurtleState turtleState) => throw new NotImplementedException();
}