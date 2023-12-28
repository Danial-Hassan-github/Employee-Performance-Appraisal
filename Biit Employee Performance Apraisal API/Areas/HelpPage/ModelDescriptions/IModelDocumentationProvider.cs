using System;
using System.Reflection;

namespace Biit_Employee_Performance_Apraisal_API.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}