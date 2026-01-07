using Common.String;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResManager = System.Resources.ResourceManager;

namespace Common.ResourceManager;

public static class ResourceManagerExtensions
{
    private const int _numberOfCharsRepresentingLangSuffix = 2;
    private const int _indexIndicatorCorrection = 1;
    private const string _resourceNotFoundMessage = "źródło nieodnalezione";

    public static string GetCollectionOfResourceStringsFromOneResourceAndConcatThemAll(this ResManager resourceManager, (string, string[], int ConcatenationOrder)[] resourceNamesAndOptionalParams, string lang, IEnumerable<string> langSuffixes)
    {
        StringBuilder stringBuilder = new();
        lang = lang?.FirstCharToUpper();

      
        int resultForLastElementInCollection = 0;
        (string, string[], int)[] resourceNames = resourceNamesAndOptionalParams.Where(x => x.Item1 != null).OrderBy(x => x.ConcatenationOrder).ToArray();

        int maxIndex = resourceNames.Length - _indexIndicatorCorrection;

        for (int i = 0; i <= maxIndex; i++)
        {
            var (nameofResourceString, parameters, concatenationOrder) = resourceNames[i];
            resourceManager.GetResourceString(stringBuilder, nameofResourceString, lang, langSuffixes, parameters);
            if (maxIndex - i > resultForLastElementInCollection)
                stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }

    public static string GetResourceString(this ResManager resourceManager, string nameofResourceString, string lang, IEnumerable<string> langSuffixes, string[] parameters = null, StringBuilder externalStringBuilder = null)
    {
        StringBuilder stringBuilder = null;

        if (externalStringBuilder is not null)
            stringBuilder = externalStringBuilder.Append(nameofResourceString);
        else
        stringBuilder = new(nameofResourceString);
        
        int startIndexOfLangSuffix = nameofResourceString.Length - _numberOfCharsRepresentingLangSuffix;

        char[] lastTwoCharsFromName = new char[2];
        stringBuilder.CopyTo(startIndexOfLangSuffix, lastTwoCharsFromName, 0, _numberOfCharsRepresentingLangSuffix);
        string lastTwoCharsFromNameAsString = new(lastTwoCharsFromName);

        if (lastTwoCharsFromName.Length > 0 && char.IsUpper(lastTwoCharsFromName[0]) && langSuffixes.Any(x => x == lastTwoCharsFromNameAsString.ToLower()))
        {
            if (lang != lastTwoCharsFromNameAsString)
                stringBuilder.Replace(lastTwoCharsFromNameAsString, lang.FirstCharToUpper(), startIndexOfLangSuffix, _numberOfCharsRepresentingLangSuffix);
        }
        else
            stringBuilder.Append(lang.FirstCharToUpper());
        
        if (stringBuilder.Length > 0)
            if (parameters != null && parameters.Length > 0)
            {
                int currentResourceNameLength = stringBuilder.Length;
                stringBuilder.AppendFormat(resourceManager.GetString(stringBuilder.ToString()), parameters).Remove(0, currentResourceNameLength);

                return stringBuilder.ToString();

            }
            else 
                return resourceManager.GetString(stringBuilder.ToString());
        else
            return resourceManager.GetString(stringBuilder.ToString()) ?? _resourceNotFoundMessage;
    }

    private static void GetResourceString(this ResManager resourceManager, StringBuilder stringBuilder, string nameofResourceString, string lang, IEnumerable<string> langSuffixes, string[] parameters = null)
    {
        string langSuffixFromResourceName = nameofResourceString.Substring(nameofResourceString.Length - _numberOfCharsRepresentingLangSuffix, _numberOfCharsRepresentingLangSuffix);

        StringBuilder resourceNameBuilder = new(nameofResourceString);

        if (!string.IsNullOrWhiteSpace(langSuffixFromResourceName) && char.IsUpper(langSuffixFromResourceName[0]) && langSuffixes.Any(x => x == langSuffixFromResourceName.ToLower()))
        {
            if (lang != langSuffixFromResourceName)
                resourceNameBuilder.Replace(langSuffixFromResourceName, lang, resourceNameBuilder.Length - _numberOfCharsRepresentingLangSuffix, _numberOfCharsRepresentingLangSuffix);
        }
        else
            resourceNameBuilder.Append(lang);

        if (resourceNameBuilder.Length > 0)
            if (parameters != null && parameters.Length > 0)
                stringBuilder.AppendFormat(resourceManager.GetString(resourceNameBuilder.ToString()), parameters).ToString();
            else
                stringBuilder.Append(resourceManager.GetString(resourceNameBuilder.ToString()));
    }
}

