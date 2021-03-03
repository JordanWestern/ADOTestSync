namespace TestSyncConsole.TestAssemblyManagement
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    public class SpecflowPlusStrategy : ITestStrategy
    {
        private const string FeatureAttribute = "FeatureAttribute";
        private const string ScenarioAttribute = "ScenarioAttribute";
        private const string FqnFormat = "{0}.{1}.#()::TestAssembly:{2}/Feature:{3}/Scenario:{4}";

        public IEnumerable<UITest> GetTests(Assembly testAssembly)
        {
            var tests = new List<UITest>();

            foreach (var type in this.GetTypes(testAssembly))
            {
                foreach (var methodInfo in this.GetMethodInfos(type))
                {
                    var featureName = this.GetFeatureName(methodInfo);
                    var scenarioName = this.GetScenarioName(methodInfo);
                    var fqn = this.GetFullyQualifiedName(methodInfo.Module.Name.Replace(".dll", string.Empty), featureName, scenarioName);

                    tests.Add(new UITest
                    {
                        ScenarioName = this.GetScenarioName(methodInfo),
                        Tags = this.GetScenarioTags(methodInfo),
                        Module = methodInfo.Module.Name,
                        FullyQualifiedName = fqn,

                        // TODO: Generate GUID or hash unique for each test
                        // Guid = new Guid()
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

        private string GetScenarioName(MethodInfo methodInfo)
        {
            return methodInfo.CustomAttributes.Single(x => x.AttributeType.Name.Equals(ScenarioAttribute)).ConstructorArguments
                .Where(arg => arg.ArgumentType.Name.Equals("String"))
                .Single().Value.ToString();
        }

        private string[] GetScenarioTags(MethodInfo methodInfo)
        {
            var tags = new List<string>();

            tags.AddRange(((ReadOnlyCollection<CustomAttributeTypedArgument>)methodInfo.CustomAttributes.Single(x => x.AttributeType.Name.Equals(ScenarioAttribute)).ConstructorArguments
                .Where(arg => arg.ArgumentType.Name.Equals("String[]"))
                .Single().Value)
                .Select(item => item.Value.ToString()));

            tags.AddRange(((ReadOnlyCollection<CustomAttributeTypedArgument>)methodInfo.DeclaringType.CustomAttributes.Single(x => x.AttributeType.Name.Equals(FeatureAttribute)).ConstructorArguments
                .Where(arg => arg.ArgumentType.Name.Equals("String[]"))
                .Single().Value)
                .Select(item => item.Value.ToString()));

            return tags.ToArray();
        }

        private string GetFeatureName(MethodInfo methodInfo)
        {
            return methodInfo.DeclaringType.CustomAttributes.Single(x => x.AttributeType.Name.Equals(FeatureAttribute)).ConstructorArguments
                .Where(arg => arg.ArgumentType.Name.Equals("String"))
                .Single().Value.ToString();
        }

        private string GetFullyQualifiedName(string projectName, string featureName, string scenarioName)
        {
            return string.Format(FqnFormat, projectName, featureName, projectName, HttpUtility.UrlEncode(featureName), HttpUtility.UrlEncode(scenarioName));
        }
    }
}