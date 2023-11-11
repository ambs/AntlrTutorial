using Antlr4.Runtime;
using Logo2Svg.Language;
using Logo2Svg.AST;

namespace Logo2Svg;

static class App {
  static int Main(string[] args)
  {
    // Check we have the correct arguments
    if (args.Length != 2) {
      Console.Error.WriteLine("Usage: logo2svg <input.logo> <output.svg>");
      return 1;
    }

    var input = args[0];
    var output = args[1];
    Console.WriteLine($"From {input} to {output}");
    try
    {
      using FileStream fs = File.OpenRead(input);
      AntlrInputStream inputStream = new AntlrInputStream(fs);
      var lexer = new LogoLexer(inputStream);
      var parser = new LogoParser(new CommonTokenStream(lexer));
      
      var programContext = parser.program();
      var visitor = new TreeVisitor();
      Program program = visitor.Visit<Program>(programContext);

      Turtle turtle = new Turtle();
      program.Execute(turtle);
      turtle.Save(output);
    }
    catch (Exception exception)
    {
      Console.WriteLine($"Error: {exception}");                
    }

    return 0;
  }
}
