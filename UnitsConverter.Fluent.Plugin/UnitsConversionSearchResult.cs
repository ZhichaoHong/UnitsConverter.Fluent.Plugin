using System.Collections.Generic;
using Blast.Core.Interfaces;
using Blast.Core.Objects;
using Blast.Core.Results;
using UnitsNet;

namespace UnitsConverter.Fluent.Plugin;
public class UnitsConversionSearchResult : SearchResultBase
{
    private static readonly int _precision_max = 4;
    public UnitsConversionSearchResult(string searchedText, IQuantity result, string iconGlyph, IList<ISearchOperation> supportedOperations,
        ICollection<SearchTag> tags, double score=1) : base(Math.Round(result.Value, _precision_max).ToString(), searchedText, result.Unit.ToString(), score, supportedOperations, tags)
    {
        UseIconGlyph = true;
        IconGlyph = iconGlyph;

        if (!string.IsNullOrWhiteSpace(result.ToString()))
        {
            InformationElements = new List<InformationElement> { new("Converted in ", $"{result.QuantityInfo.QuantityType}") };
        }

        MlFeatures = new Dictionary<string, string> { ["UnitType"] = result.Unit.ToString() };

        var roundedResult = Math.Round(result.Value, _precision_max);

        ValueOnly = roundedResult.ToString();

        // Get the abbreviation from IQuantity
        string qstr = result.ToString();
        int index = qstr.IndexOf(" ");

        ValueWithUnitAbbrev = $"{roundedResult} {qstr.Substring(index)}";

        ValueWithUnit = $"{roundedResult} {result.Unit.ToString()}";
    }
    public string Quantity { get; }
    public string ConvertedQuantity { get; private set; }

    internal string ValueOnly { get; private set; }
    internal string ValueWithUnitAbbrev { get; private set; }
    internal string ValueWithUnit { get; private set; }

    protected override void OnSelectedSearchResultChanged()
    {
        
    }

}
