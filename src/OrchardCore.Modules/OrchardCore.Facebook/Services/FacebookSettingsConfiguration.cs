using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Shell.Models;
using OrchardCore.Environment.Shell;
using OrchardCore.Facebook.Settings;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace OrchardCore.Facebook.Services;

public class FacebookSettingsConfiguration : IConfigureOptions<FacebookSettings>
{
    private readonly FacebookService _facebookService;
    private readonly ShellSettings _shellSettings;
    private readonly ILogger _logger;

    public FacebookSettingsConfiguration(
        FacebookService facebookService,
        ShellSettings shellSettings,
        ILogger<FacebookSettingsConfiguration> logger)
    {
        _facebookService = facebookService;
        _shellSettings = shellSettings;
        _logger = logger;
    }

    public void Configure(FacebookSettings options)
    {
        var settings = GetFacebookSettingsAsync()
            .GetAwaiter()
            .GetResult();

        if (settings != null)
        {
            options.AppId = settings.AppId;
            options.AppSecret = settings.AppSecret;
            options.Version = settings.Version;
            options.FBInit = settings.FBInit;
            options.FBInitParams = settings.FBInitParams;
            options.SdkJs = settings.SdkJs;
        }
    }

    private async Task<FacebookSettings> GetFacebookSettingsAsync()
    {
        var settings = await _facebookService.GetSettingsAsync();

        if (_facebookService.ValidateSettings(settings).Any(result => result != ValidationResult.Success))
        {
            if (_shellSettings.State == TenantState.Running)
            {
                _logger.LogWarning("Facebook is not correctly configured.");
            }

            return null;
        }

        return settings;
    }
}