namespace TestSyncConsole.TestAssemblies
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using TestSyncConsole.Constants;
    using TestSyncConsole.Extensions;

    public class SpecflowPlusStrategy : ITestStrategy
    {
        // TODO: Pull out steps somehow....
        public IEnumerable<UITest> GetTests(Assembly testAssembly)
        {
            var tests = new List<UITest>();

            foreach (var type in this.GetTypesWithFeatureAttribute(testAssembly))
            {
                foreach (var methodInfo in this.GetMethodInfosWithScenarioAttribute(type))
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
                        Guid = fqn.GenerateGuid()
                    });
                }
            }

            return tests;
        }

        private IEnumerable<Type> GetTypesWithFeatureAttribute(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.CustomAttributes.Any(customAttribute => customAttribute.AttributeType.Name.Equals(SpecflowPlusConstants.FeatureAttribute)));
        }

        private IEnumerable<MethodInfo> GetMethodInfosWithScenarioAttribute(Type type)
        {
            return type.GetMethods()
                .Where(methodInfo => methodInfo.CustomAttributes.Any(customAttribute => customAttribute.AttributeType.Name.Equals(SpecflowPlusConstants.ScenarioAttribute)));
        }

        private string GetScenarioName(MethodInfo methodInfo)
        {
            return methodInfo.CustomAttributes
                .Single(x => x.AttributeType.Name.Equals(SpecflowPlusConstants.ScenarioAttribute)).ConstructorArguments
                .Where(arg => arg.ArgumentType.Name.Equals("String"))
                .Single().Value.ToString();
        }

        private string[] GetScenarioTags(MethodInfo methodInfo)
        {
            var tags = new List<string>();

            tags.AddRange(((ReadOnlyCollection<CustomAttributeTypedArgument>)methodInfo.CustomAttributes
                .Single(x => x.AttributeType.Name.Equals(SpecflowPlusConstants.ScenarioAttribute)).ConstructorArguments
                .Where(arg => arg.ArgumentType.Name.Equals("String[]"))
                .Single().Value)
                .Select(item => item.Value.ToString()));

            tags.AddRange(((ReadOnlyCollection<CustomAttributeTypedArgument>)methodInfo.DeclaringType.CustomAttributes
                .Single(x => x.AttributeType.Name.Equals(SpecflowPlusConstants.FeatureAttribute)).ConstructorArguments
                .Where(arg => arg.ArgumentType.Name.Equals("String[]"))
                .Single().Value)
                .Select(item => item.Value.ToString()));

            return tags.ToArray();
        }

        private string GetFeatureName(MethodInfo methodInfo)
        {
            return methodInfo.DeclaringType.CustomAttributes
                .Single(x => x.AttributeType.Name.Equals(SpecflowPlusConstants.FeatureAttribute)).ConstructorArguments
                .Where(arg => arg.ArgumentType.Name.Equals("String"))
                .Single().Value.ToString();
        }

        private string GetFullyQualifiedName(string projectName, string featureName, string scenarioName)
        {
            return string.Format(
                SpecflowPlusConstants.FullyQualifiedNameFormat,
                projectName,
                featureName,
                projectName,
                HttpUtility.UrlEncode(featureName),
                HttpUtility.UrlEncode(scenarioName));
        }
    }
}