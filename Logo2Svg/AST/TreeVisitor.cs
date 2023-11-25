using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Logo2Svg.Language;

namespace Logo2Svg.AST
{
    public class TreeVisitor : LogoBaseVisitor<INode>
    {
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

        public override INode VisitCommand([NotNull] LogoParser.CommandContext context)
        {
            IToken command = null;
            List<Parameter> parameters = new();

            if (context.simpleCommand() is { } splCmd)
            {
                return Visit<Command>(splCmd);
            }

            if (context.Home() is { } homeCtx)
            {
                command = homeCtx.Symbol;
            }

            if (context.SetXY() is { } setXyCtx)
            {
                command = setXyCtx.Symbol;
                parameters.Add(Visit<Parameter>(context.simplePoint()));
            }

            if (context.SetPos() is { } setPosCtx)
            {
                command = setPosCtx.Symbol;
                parameters.Add(Visit<Parameter>(context.squarePoint()));
            }

            if (context.Arc() is { } argCtx)
            {
                command = argCtx.Symbol;
                parameters.AddRange(context.expr().Select(Visit<Parameter>));
            }

            return command is not null ? new Command(command.Type, command.Text, parameters.ToArray()) : null;
        }

        public override INode VisitProgram([NotNull] LogoParser.ProgramContext context)
        {
            Program program = new();
            program.AddRange(context.command().Select(cmd => Visit<Command>(cmd)).ToList());
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

        public T Visit<T>(IParseTree tree) => (T)Visit(tree);
    }
}
