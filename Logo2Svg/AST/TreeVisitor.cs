using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Logo2Svg.Language;

namespace Logo2Svg.AST
{
    public class TreeVisitor : LogoParserBaseVisitor<INode>
    {
        public override INode VisitVariable(LogoParser.VariableContext context)
            => new VarName(context.Variable()?.GetText() ?? context.VariableRef()?.GetText());
        

        public override INode VisitScalar([NotNull] LogoParser.ScalarContext context)
        {
            return context.value() is { } ctx ? Visit<Parameter>(ctx) : Visit<Parameter>(context.expr());
        }

        public override INode VisitValue([NotNull] LogoParser.ValueContext context)
        {
            var valueStr = (context.IntegerValue() ?? context.RealValue()).Symbol.Text;
            return new ValueParam(float.Parse(valueStr));
        }

        public override INode VisitSquarePoint([NotNull] LogoParser.SquarePointContext context)
        {
            var xVal = Visit<Parameter>(context.expr(0));
            var yVal = Visit<Parameter>(context.expr(1));
            return new PointParam(xVal, yVal);
        }

        public override INode VisitSimplePoint([NotNull] LogoParser.SimplePointContext context)
        {
            var xVal = Visit<Parameter>(context.expr(0));
            var yVal = Visit<Parameter>(context.expr(1));
            return new PointParam(xVal, yVal);
        }

        public override INode VisitSimpleCommand([NotNull] LogoParser.SimpleCommandContext context)
        {
            var param = Visit<Parameter>(context.expr());
            var command = context.cmd.Type;
            var name = context.cmd.Text;
            return new Command(command, name, param);
        }

        public override INode VisitBinaryOp([NotNull] LogoParser.BinaryOpContext context)
        {
            var parcels = context.expr().Select(Visit<Parameter>);
            var op = context.op.Text switch
            {
                "^" => LogoLexer.Power,
                "+" => LogoLexer.Sum,
                "-" => LogoLexer.Difference,
                "*" => LogoLexer.Product,
                "/" => LogoLexer.Quotient,
                "%" => LogoLexer.Remainder,
                _ => throw new ArgumentOutOfRangeException()
            };
            return new ExprParam(op, parcels.ToArray());
        }

        public override INode VisitUnaryMinus([NotNull] LogoParser.UnaryMinusContext context)
        {
            var sub = Visit<Parameter>(context.expr());
            return new ExprParam(LogoLexer.Minus, sub);
        }
        
        public override INode VisitHome([NotNull] LogoParser.HomeContext context)
        {
            return new Command(LogoLexer.Home, "Home");
        }

        public override INode VisitSetPosition(LogoParser.SetPositionContext context)
        {
            var (command, param) = context.SetXY() is { } setXyCtx?
                (setXyCtx.Symbol, Visit<Parameter>(context.simplePoint())) :
                (context.SetPos().Symbol, Visit<Parameter>(context.squarePoint()));
            
            return new Command(command.Type, command.Text, param);
        }

        public override INode VisitArc(LogoParser.ArcContext context)
        {
            var parameters = context.expr().Select(Visit<Parameter>).ToArray();
            return new Command(LogoLexer.Arc, "Arc", parameters);
        }

        public override INode VisitProgram([NotNull] LogoParser.ProgramContext context)
        {
            Program program = new();
            program.AddRange(context.command().Select(Visit<Command>).ToList());
            return program;
        }

        public override INode VisitSummation([NotNull] LogoParser.SummationContext context)
        {
            var parcels = context.expr().Select(Visit<Parameter>);
            return new ExprParam(context.Sum().Symbol.Type, parcels.ToArray());
        }

        public override INode VisitProduct([NotNull] LogoParser.ProductContext context)
        {
            var parcels = context.expr().Select(Visit<Parameter>);
            return new ExprParam(context.Product().Symbol.Type, parcels.ToArray());
        }

        public override INode VisitQuotient([NotNull] LogoParser.QuotientContext context)
        {
            return new ExprParam(context.Quotient().Symbol.Type, Visit<Parameter>(context.expr()));
        }

        public override INode VisitPrefixBinaryOp([NotNull] LogoParser.PrefixBinaryOpContext context)
        {
            var parcels = context.expr().Select(Visit<Parameter>);
            return new ExprParam(context.cmd.Type, parcels.ToArray());
        }

        public override INode VisitArithFuncs(LogoParser.ArithFuncsContext context)
        {
            var parameters = context.expr().Select(Visit<Parameter>);
            return new ExprParam(context.fun.Type, parameters.ToArray());
        }

        public override INode VisitSetVariable(LogoParser.SetVariableContext context)
        {
            var varName = context.Variable().GetText();
            var value = Visit<Parameter>(context.expr());
            return new Command(LogoLexer.Make, "make", new VarName(varName), value);
        }

        public override INode VisitShow(LogoParser.ShowContext context)
        => new Command(LogoLexer.Show, "show", Visit<Parameter>(context.expr()));
        

        public T Visit<T>(IParseTree tree) => (T)Visit(tree);
    }
}
