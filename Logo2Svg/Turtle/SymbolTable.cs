namespace Logo2Svg;

public class SymbolTable
{
    private readonly Stack<Dictionary<string, float>> _symbolTable;

    public SymbolTable()
    {
        _symbolTable = new Stack<Dictionary<string, float>>();
        _symbolTable.Push(new Dictionary<string, float>());
    }

    public void EnterScope()
    {
        var top = _symbolTable.Peek();
        _symbolTable.Push(new Dictionary<string, float>(top));
    }

    public void ExitScope()
    {
        _symbolTable.Pop();
    }
    
    /// <summary>
    /// Defines a variable in the Symbol Table.
    /// </summary>
    /// <param name="varName">The variable name to define.</param>
    /// <param name="value">The variable's value.</param>
    public void DefineVariable(string varName, float value) => _symbolTable.Peek()[varName] = value;

    /// <summary>
    /// Queries the Symbol Table for a variable.
    /// </summary>
    /// <param name="varName">The variable name to be queries.</param>
    /// <param name="value">The value of the variable, if it is defined.</param>
    /// <returns>A boolean stating if the variable was found in the symbol table.</returns>
    public bool RetrieveVariable(string varName, out float value) => _symbolTable.Peek().TryGetValue(varName, out value);

    
}