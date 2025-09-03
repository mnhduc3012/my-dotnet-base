using Microsoft.Extensions.Options;
using MyDotNetBase.Application.Abstractions.Emails;
using System.Collections.Concurrent;
using System.Reflection;

namespace MyDotNetBase.Infrastructure.Emails;

public class HtmlTemplateRenderer : ITemplateRenderer
{
    private readonly string _templateFolder;
    private readonly ConcurrentDictionary<string, string> _templateCache = new();

    public HtmlTemplateRenderer(IOptions<EmailTemplateConfiguration> emailTemplateConfiguration)
    {
        _templateFolder = Path.GetFullPath(
            emailTemplateConfiguration.Value.TemplateFolder,
            AppContext.BaseDirectory);
    }

    public async Task<string> RenderAsync(string templateName, object model, CancellationToken cancellationToken)
    {
        var fileName = $"{templateName}.html";

        if (!_templateCache.TryGetValue(fileName, out var templateContent))
        {
            var filePath = Path.Combine(_templateFolder, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Template file not found.", filePath);

            templateContent = await File.ReadAllTextAsync(filePath, cancellationToken);

            _templateCache[fileName] = templateContent;
        }

        var content = templateContent;

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
