namespace BlueLagoon.Shared.DevTools.Installers;

public static class InstallersFetcher
{
    public static IEnumerable<TInstaller> GetInstallers<TAssemblyClass, TInstaller>()
        where TAssemblyClass : class
        => typeof(TAssemblyClass).Assembly.GetTypes()
                    .Where(x => !x.IsAbstract && !x.IsInterface && typeof(TInstaller).IsAssignableFrom(x))
                    .Select(Activator.CreateInstance)
                    .Cast<TInstaller>()
                    .ToList();
}