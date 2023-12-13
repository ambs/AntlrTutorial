using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Logo2Svg.Language;

namespace Logo2Svg.AST;

/// <summary>
/// Visitors for the ANTLR parser tree. 
/// </summary>
public class TreeVisitor : LogoParserBaseVisitor<INode>
{
    /// <summary>
    /// Utility method to call the visitor function and cast.
    /// </summary>
    /// <param name="context">The grammar context to visit.</param>
    /// <typeparam name="T">The type to cast the visit result.</typeparam>
    /// <returns></returns>
    public T Visit<T>(IParseTree context) => (T) Visit(context);
    
    /// <summary>
    /// Visitor for the <c>Variable</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>VarName representation of the variable.</returns>
    public override INode VisitVariable(LogoParser.VariableContext context)
        => new VarName(context.Variable()?.GetText() ?? context.VariableRef()?.GetText());

    /// <summary>
    /// Visitor for the <c>Scalar</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Parameter representing the scalar.</returns>
    public override INode VisitScalar([NotNull] LogoParser.ScalarContext context)
        => context.value() is { } ctx ? Visit<Parameter>(ctx) : Visit<Parameter>(context.expr());
    
    /// <summary>
    /// Visitor for the <c>Value</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Parameter (ExprParam or ValueParam) representing the value.</returns>
    public override INode VisitValue([NotNull] LogoParser.ValueContext context)
    {
        if (context.True() is not null) return new ExprParam(LogoLexer.True);
        if (context.False() is not null) return new ExprParam(LogoLexer.False);

        var valueStr = (context.IntegerValue() ?? context.RealValue()).Symbol.Text;
        return new ValueParam(float.Parse(valueStr));
    }

    /// <summary>
    /// Visitor for the <c>SquarePoint</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>PointParam representing the point coordinates.</returns>
    public override INode VisitSquarePoint([NotNull] LogoParser.SquarePointContext context)
        => new PointParam(Visit<Parameter>(context.expr(0)), Visit<Parameter>(context.expr(1)));

    /// <summary>
    /// Visitor for the <c>SimplePoint</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>PointParam representing the point coordinates.</returns>
    public override INode VisitSimplePoint([NotNull] LogoParser.SimplePointContext context)
        => new PointParam(Visit<Parameter>(context.expr(0)), Visit<Parameter>(context.expr(1)));

    /// <summary>
    /// Visitor for the <c>SimpleCommand</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command representing the recognized command.</returns>
    public override INode VisitSimpleCommand([NotNull] LogoParser.SimpleCommandContext context)
    {
        var param = Visit<Parameter>(context.expr());
        var command = context.cmd.Type;
        var name = context.cmd.Text;
        return new Command(command, name, param);
    }

    /// <summary>
    /// Visitor for the <c>BinaryOp</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>ExprParam representing the operation.</returns>
    public override INode VisitBinaryOp([NotNull] LogoParser.BinaryOpContext context)
    {
        var parcels = context.expr().Select(Visit<Parameter>);
        var op = context.op.Type switch
        {
            LogoLexer.CircunflexSign => LogoLexer.Power,
            LogoLexer.PlusSign => LogoLexer.Sum,
            LogoLexer.MinusSign => LogoLexer.Difference,
            LogoLexer.AsteriskSign => LogoLexer.Product,
            LogoLexer.SlashSigh => LogoLexer.Quotient,
            LogoLexer.PercentSign => LogoLexer.Remainder,
            LogoLexer.LessSign => LogoLexer.Less,
            LogoLexer.GreaterSign => LogoLexer.Greater,
            LogoLexer.LessEqualSign => LogoLexer.LessEqual,
            LogoLexer.GreaterEqualSign => LogoLexer.GreaterEqual,
            _ => context.op.Type
        };
        return new ExprParam(op, parcels.ToArray());
    }

    /// <summary>
    /// Visitor for the <c>UnaryMinus</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>ExprParam representing the operation.</returns>
    public override INode VisitUnaryMinus([NotNull] LogoParser.UnaryMinusContext context)
        => new ExprParam(LogoLexer.Minus, Visit<Parameter>(context.expr()));
    
    /// <summary>
    /// Visitor for the <c>AtomicCmd</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command representing the non-parameter command.</returns>
    public override INode VisitAtomicCmd([NotNull] LogoParser.AtomicCmdContext context)
        => new Command(context.cmd.Type, context.cmd.Text);

    /// <summary>
    /// Visitor for the <c>AtomicCmd</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command representing the non-parameter command.</returns>
    public override INode VisitSetPosition(LogoParser.SetPositionContext context)
    {
        var (command, param) = context.SetXY() is { } setXyCtx
            ? (setXyCtx.Symbol, Visit<Parameter>(context.simplePoint()))
            : (context.SetPos().Symbol, Visit<Parameter>(context.squarePoint()));

        return new Command(command.Type, command.Text, param);
    }

    /// <summary>
    /// Visitor for the <c>Arc</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command representing the arc command.</returns>
    public override INode VisitArc(LogoParser.ArcContext context)
        => new Command(LogoLexer.Arc, "Arc", context.expr().Select(Visit).ToArray());
    
    /// <summary>
    /// Visitor for the <c>RepeatStmt</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command representing the repeat command.</returns>
    public override INode VisitRepeatStmt(LogoParser.RepeatStmtContext context)
        => new Command(LogoLexer.Repeat, "Repeat",
            Visit(context.expr()),
            Visit(context.cmdBlock()));

    /// <summary>
    /// Visitor for the <c>ForeverStmt</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command representing the forever command.</returns>
    public override INode VisitForeverStmt(LogoParser.ForeverStmtContext context)
        => new Command(LogoLexer.Forever, "Forever", Visit(context.cmdBlock()));

    /// <summary>
    /// Visitor for the <c>CmdBlock</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>CommandBlock with the included commands.</returns>
    public override INode VisitCmdBlock(LogoParser.CmdBlockContext context)
        => new CommandBlock(context.command().Select(Visit<Command>));

    /// <summary>
    /// Visitor for the <c>Program</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Program with the included commands.</returns>
    public override INode VisitProgram([NotNull] LogoParser.ProgramContext context)
        => new Program(context.command().Select(Visit<Command>));

    /// <summary>
    /// Visitor for the <c>Boolean</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Expression parameter for boolean expressions.</returns>
    public override INode VisitBoolean([NotNull] LogoParser.BooleanContext context)
        => new ExprParam(context.cmd.Type, context.expr().Select(Visit<Parameter>).ToArray());

    /// <summary>
    /// Visitor for the <c>Summation</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Expression parameter for summation expression.</returns>
    public override INode VisitSummation([NotNull] LogoParser.SummationContext context)
        => new ExprParam(context.Sum().Symbol.Type, context.expr().Select(Visit<Parameter>).ToArray());
    
    /// <summary>
    /// Visitor for the <c>Product</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Expression parameter for product expression.</returns>
    public override INode VisitProduct([NotNull] LogoParser.ProductContext context)
        => new ExprParam(context.Product().Symbol.Type, context.expr().Select(Visit<Parameter>).ToArray());

    /// <summary>
    /// Visitor for the <c>Quotient</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Expression parameter for quotient expression.</returns>
    public override INode VisitQuotient([NotNull] LogoParser.QuotientContext context)
        => new ExprParam(context.Quotient().Symbol.Type, Visit<Parameter>(context.expr()));

    /// <summary>
    /// Visitor for the <c>PrefixBinaryOp</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Expression parameter for the Polish Notation expressions.</returns>
    public override INode VisitPrefixBinaryOp([NotNull] LogoParser.PrefixBinaryOpContext context)
        => new ExprParam(context.cmd.Type, context.expr().Select(Visit<Parameter>).ToArray());

    /// <summary>
    /// Visitor for the <c>ArithFuncs</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Expression parameter for the arithmetic function expressions.</returns>
    public override INode VisitArithFuncs(LogoParser.ArithFuncsContext context)
        => new ExprParam(context.fun.Type, context.expr().Select(Visit<Parameter>).ToArray());

    /// <summary>
    /// Visitor for the <c>SetVariable</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command for the <c>make</c> and <c>name</c> commands.</returns>
    public override INode VisitSetVariable(LogoParser.SetVariableContext context)
    {
        var varName = context.Variable().GetText();
        var value = Visit<Parameter>(context.expr());
        return new Command(LogoLexer.Make, "make", new VarName(varName), value);
    }
    
    /// <summary>
    /// Visitor for the <c>Show</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command for the <c>show</c> command.</returns>
    public override INode VisitShow(LogoParser.ShowContext context)
        => new Command(LogoLexer.Show, "show", Visit<Parameter>(context.expr()));

    /// <summary>
    /// Visitor for the <c>IfStmt</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command for the <c>if</c> statement.</returns>
    public override INode VisitIfStmt(LogoParser.IfStmtContext context)
        => new Command(LogoLexer.If, "if", Visit(context.expr()), Visit(context.cmdBlock()));

    /// <summary>
    /// Visitor for the <c>IfElseStmt</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>Command for the <c>if</c> statement with else clause.</returns>
    public override INode VisitIfElseStmt(LogoParser.IfElseStmtContext context)
        => new Command(LogoLexer.IfElse, "ifElse",
            Visit(context.expr()), Visit(context.cmdBlock(0)), Visit(context.cmdBlock(1)));
    
    /// <summary>
    /// Visitor for the <c>ColourList</c> production.
    /// </summary>
    /// <param name="context">Production context.</param>
    /// <returns>A new ColourNode object.</returns>
    public override INode VisitColourList(LogoParser.ColourListContext context)
    {
        var rgb = context.expr().Select(Visit<ExprParam>).ToList();
        return new ColourNode(rgb[0], rgb[1], rgb[2]);
    }
}