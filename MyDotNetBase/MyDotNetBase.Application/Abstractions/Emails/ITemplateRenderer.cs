namespace MyDotNetBase.Application.Abstractions.Emails;

public interface ITemplateRenderer
{
    Task<string> RenderAsync(string templateName, object model);
}
