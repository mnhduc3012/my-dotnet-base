using Microsoft.Extensions.Options;
using MyDotNetBase.Application.Abstractions.Emails;
using System.Reflection;

namespace MyDotNetBase.Infrastructure.Emails;

public class HtmlTemplateRenderer : ITemplateRenderer
{
    private readonly EmailTemplateConfiguration _emailTemplateConfiguration;

    public HtmlTemplateRenderer(IOptions<EmailTemplateConfiguration> emailTemplateConfiguration)
    {
        _emailTemplateConfiguration = emailTemplateConfiguration.Value;
    }

    public async Task<string> RenderAsync(string templateName, object model)
    {
        var filePath = Path.Combine(_emailTemplateConfiguration.TemplateFolder, templateName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Template file not found.", filePath);

        var content = await File.ReadAllTextAsync(filePath);

        var props = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            var value = prop.GetValue(model)?.ToString() ?? string.Empty;
            var placeholder = $"{{{{{prop.Name}}}}}";
            content = content.Replace(placeholder, value);
        }

        return content;
    }
}
