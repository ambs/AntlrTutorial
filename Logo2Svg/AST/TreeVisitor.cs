using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Logo2Svg.Language;

namespace Logo2Svg.AST
{
    public class TreeVisitor : LogoBaseVisitor<INode>
    {
        public override INode VisitValue([NotNull] LogoParser.ValueContext context)
        {
            var valueStr = (context.IntegerValue() ?? context.RealValue()).Symbol.Text;
            return new ValueParam(float.Parse(valueStr));
        }

        public override INode VisitSquarePoint(LogoParser.SquarePointContext context)
        {
            var xVal = Visit<ValueParam>(context.value(0));
            var yVal = Visit<ValueParam>(context.value(1));
            return new PointParam(xVal, yVal);
        }

        public override INode VisitSimplePoint([NotNull] LogoParser.SimplePointContext context)
        {
            var xVal = Visit<ValueParam>(context.value(0));
            var yVal = Visit<ValueParam>(context.value(1));
            return new PointParam(xVal, yVal);
        }

        public override INode VisitSimpleCommand([NotNull] LogoParser.SimpleCommandContext context)
        {
            var @param = Visit<ValueParam>(context.value());
            var command = context.cmd.Type;
            var name = context.cmd.Text;
            return new Command(command, name, @param);
        }

        
        public override INode VisitCommand(LogoParser.CommandContext context)
        {
            IToken command = null;
            List<IParameter> parameters = new();

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
                parameters.Add(Visit<IParameter>(context.simplePoint()));
            }

            if (context.SetPos() is { } setPosCtx)
            {
                command = setPosCtx.Symbol;
                parameters.Add(Visit<IParameter>(context.squarePoint()));
            }

            if (context.Arc() is { } argCtx)
            {
                command = argCtx.Symbol;
                parameters.AddRange(context.value().Select(Visit<IParameter>));
            }

            return command is not null ? new Command(command.Type, command.Text, parameters.ToArray()) : null;
        }


        public override INode VisitProgram([NotNull] LogoParser.ProgramContext context)
        {
            Program program = new();
            program.AddRange(context.command().Select(cmd => Visit<Command>(cmd)).ToList());
            return program;
        }
        
        public T Visit<T>(IParseTree tree) => (T)Visit(tree);
    }
}
