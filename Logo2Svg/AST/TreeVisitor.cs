using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Logo2Svg.Language;

namespace Logo2Svg.AST
{
    public class TreeVisitor : LogoBaseVisitor<INode>
    {
        public override INode VisitCommand([NotNull] LogoParser.CommandContext context)
        {
            string value = context.Value().GetText();
            string command = null;

            if (context.Forward() is { } forwardContext)
            {
                command = forwardContext.GetText();
            }
            if (context.Right() is { } rightContext)
            {
                command = rightContext.GetText();
            }

            return command != null ? new Command(command, value) : null;
        }

        public override INode VisitProgram([NotNull] LogoParser.ProgramContext context)
        {
            Program program = new();
            program.AddRange(context.command().Select(cmd => Visit(cmd) as Command).ToList());
            return program;
        }
    }
}
