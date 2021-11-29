using System.Collections.Generic;
using Blast.Core.Interfaces;
using Blast.Core.Objects;
using Blast.Core.Results;
using UnitsNet;

namespace UnitsConverter.Fluent.Plugin;
public class UnitsConversionSearchResult : SearchResultBase
{
    public UnitsConversionSearchResult(string searchedText, IQuantity result, string iconGlyph, IList<ISearchOperation> supportedOperations,
        ICollection<SearchTag> tags, double score=1) : base(result.ToString(), searchedText, result.Unit.ToString(), score, supportedOperations, tags)
    {
        UseIconGlyph = true;
        IconGlyph = iconGlyph;

        if (!string.IsNullOrWhiteSpace(result.ToString()))
        {
            InformationElements = new List<InformationElement> { new("Converted in", $"{result.QuantityInfo.QuantityType}") };
        }

        MlFeatures = new Dictionary<string, string> { ["UnitType"] = result.Unit.ToString() };
    }
    public string Quantity { get; }
    public string ConvertedQuantity { get; private set; }

    protected override void OnSelectedSearchResultChanged()
    {
        
    }

}
