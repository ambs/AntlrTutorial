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
            int id = 0;

            if (context.Forward() is { } forwardContext)
            {
                command = forwardContext.GetText();
                id = forwardContext.Symbol.Type;
            }
            if (context.Right() is { } rightContext)
            {
                command = rightContext.GetText();
                id = rightContext.Symbol.Type;
            }

            return command != null ? new Command(id, command, value) : null;
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
