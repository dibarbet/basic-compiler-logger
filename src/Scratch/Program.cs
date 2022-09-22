using Basic.CompilerLog.Util;
using Microsoft.CodeAnalysis;

using System.Reflection.PortableExecutable;

#pragma warning disable 8321

var filePath = @"C:\Users\dabarbet\source\repos\ConsoleApp8\msbuild.binlog";
// var filePath = @"C:\Users\jaredpar\code\wt\ros2\artifacts\log\Debug\Build.binlog";
// var filePath = @"C:\Users\jaredpar\code\roslyn\artifacts\log\Debug\Build.binlog";
//var filePath = @"C:\Users\jaredpar\code\roslyn\src\Compilers\CSharp\csc\msbuild.binlog";

//TestDiagnostics(filePath);
// RoundTrip(filePath);
await Do(filePath);

async Task SolutionScratchAsync(string binlogFilePath)
{
    using var stream = CompilerLogUtil.GetOrCreateCompilerLogStream(binlogFilePath);
    using var reader = SolutionReader.Create(stream, leaveOpen: false);
    var solution = reader.ReadSolution();
    var workspace = new AdhocWorkspace();
    workspace.AddSolution(solution);

    foreach (var project in workspace.CurrentSolution.Projects)
    {
        var compilation = await project.GetCompilationAsync();
        foreach (var syntaxTree in compilation!.SyntaxTrees)
        {
            Console.WriteLine(syntaxTree.ToString());
        }
    }
}

async Task Do(string binLogPath)
{
    var compilerLogFileName = $"{Path.GetFileNameWithoutExtension(binLogPath)}.compilerlog";
    var compilerLogFilePath = Path.Combine(Path.GetDirectoryName(binLogPath)!, compilerLogFileName);
    var diagnosticList = CompilerLogUtil.ConvertBinaryLog(
        binLogPath!,
        compilerLogFilePath);
    foreach (var diagnostic in diagnosticList)
    {
        Console.WriteLine(diagnostic);
    }

    using var stream = CompilerLogUtil.GetOrCreateCompilerLogStream(compilerLogFilePath);
    using var reader = SolutionReader.Create(stream);
    var solution = reader.ReadSolution();
    Console.WriteLine("projects " + solution.Projects.Count);
}



