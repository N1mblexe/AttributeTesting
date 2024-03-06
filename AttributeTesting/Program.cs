using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
internal sealed class FunctionInfoAttribute : Attribute
{
    public string Name { get; }
    public string Action { get; }
    public Type[] ParameterTypes { get; }

    public FunctionInfoAttribute(string name, string action, params Type[] parameterTypes)
    {
        Name = name;
        Action = action;
        ParameterTypes = parameterTypes;
    }
}

internal class FunctionInfo
{
    public string Name { get; }
    public string Action { get; }
    public Type[] ParameterTypes { get; }

    public FunctionInfo(string name, string action, Type[] parameterTypes)
    {
        Name = name;
        Action = action;
        ParameterTypes = parameterTypes;
    }
}

internal class FunctionCollector
{
    private List<FunctionInfo> functionList = new List<FunctionInfo>();

    public void CollectFunctions(Type type)
    {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            FunctionInfoAttribute attribute = (FunctionInfoAttribute)method.GetCustomAttribute(typeof(FunctionInfoAttribute), false);

            if (attribute != null)
            {
                FunctionInfo functionInfo = new FunctionInfo(attribute.Name, attribute.Action, attribute.ParameterTypes);
                functionList.Add(functionInfo);
            }
        }
    }

    public FunctionInfo FindFunctionByName(string functionName)
    {
        return functionList.FirstOrDefault(f => f.Name == functionName);
    }

    public void PrintFunctions()
    {
        foreach (FunctionInfo functionInfo in functionList)
        {
            Console.WriteLine(functionInfo.Name);
            Console.WriteLine(functionInfo.Action);
            Console.WriteLine(functionInfo.ParameterTypes);
            Console.WriteLine("------------------------------------------");
        }
    }
}

internal class Program
{
    [FunctionInfo("Function1", "DoSomething", typeof(int), typeof(string))]
    private static void MyFunction1(int param1, string param2)
    {
        // Function implementation
    }

    [FunctionInfo("Function2", "DoSomethingElse", typeof(double))]
    private static void MyFunction2(double param1)
    {
        // Function implementation
    }

    private static void Main()
    {
        Program program = new Program();
        FunctionCollector collector = new FunctionCollector();
        collector.CollectFunctions(typeof(Program));

        collector.PrintFunctions();
    }
}