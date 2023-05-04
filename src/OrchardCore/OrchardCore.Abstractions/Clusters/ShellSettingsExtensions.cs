using OrchardCore.Environment.Shell;

namespace OrchardCore.Clusters;

/// <summary>
/// Extension methods for managing tenant clusters.
/// </summary>
public static class ShellSettingsExtensions
{
    /// <summary>
    /// Gets the tenant cluster related to the provided <see cref="ShellSettings"/>.
    /// </summary>
    public static string GetClusterId(this ShellSettings settings, ClustersOptions options)
    {
        // Check if the tenant id and then the slot is not yet initialized.
        if (settings.ClusterSlot == -1)
        {
            return null;
        }

        foreach (var cluster in options.Clusters)
        {
            // Check if the cluster slot of the current tenant is in the range.
            if (cluster.SlotMin > settings.ClusterSlot || cluster.SlotMax < settings.ClusterSlot)
            {
                continue;
            }

            return cluster.ClusterId;
        }

        return null;
    }
}