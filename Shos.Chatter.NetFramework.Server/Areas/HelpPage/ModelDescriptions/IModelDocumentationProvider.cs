using System;
using System.Reflection;

namespace Shos.Chatter.NetFramework.Server.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}