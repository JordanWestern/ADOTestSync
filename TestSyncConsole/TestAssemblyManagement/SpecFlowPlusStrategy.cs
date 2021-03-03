namespace TestSyncConsole.TestAssemblyManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SpecflowPlusStrategy : ITestStrategy
    {
        private const string FeatureAttribute = "FeatureAttribute";
        private const string ScenarioAttribute = "ScenarioAttribute";

        public IEnumerable<UITest> GetTests(Assembly testAssembly)
        {
            var tests = new List<UITest>();

            foreach (var type in this.GetTypes(testAssembly))
            {
                foreach (var methodInfo in this.GetMethodInfos(type))
                {
                    tests.Add(new UITest
                    {
                        ScenarioTitle = this.GetScenarioTitle(methodInfo),
                        Tags = this.GetScenarioTags(methodInfo),
                        Module = methodInfo.Module.Name
                    });
                }
            }

            return tests;
        }

        private IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.CustomAttributes.Any(customAttribute => customAttribute.AttributeType.Name.Equals(FeatureAttribute)));
        }

        private IEnumerable<MethodInfo> GetMethodInfos(Type type)
        {
            return type.GetMethods().Where(methodInfo => methodInfo.CustomAttributes.Any(customAttribute => customAttribute.AttributeType.Name.Equals(ScenarioAttribute)));
        }

        private string GetScenarioTitle(MethodInfo methodInfo)
        {
            return methodInfo.CustomAttributes.ElementAt(0).ConstructorArguments.Where(arg => arg.ArgumentType.Equals("String")).First().Value.ToString();
        }

        private string[] GetScenarioTags(MethodInfo methodInfo)
        {
            // TODO: cast to string[] fails
            return (string[])methodInfo.CustomAttributes.ElementAt(0).ConstructorArguments.Where(arg => arg.ArgumentType.Equals("String[]")).First().Value;
        }
    }
}
