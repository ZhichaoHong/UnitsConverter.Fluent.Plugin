using System.Collections.Generic;
using Blast.Core.Interfaces;
using Blast.Core.Objects;
using Blast.Core.Results;
using UnitsNet;

namespace UnitsConverter.Fluent.Plugin;
public class UnitsConversionSearchResult : SearchResultBase
{
    public UnitsConversionSearchResult(string searchedText, IQuantity result, string iconGlyph, IList<ISearchOperation> supportedOperations,
        ICollection<SearchTag> tags) : base(result.ToString(), searchedText, result.Unit.ToString(), 1, supportedOperations, tags)
    {
        UseIconGlyph = true;
        IconGlyph = iconGlyph;

        if (!string.IsNullOrWhiteSpace(result.ToString()))
        {
            InformationElements = new List<InformationElement> { new("Converted in", $"{result.QuantityInfo.QuantityType}") };
        }
    }
    public string Quantity { get; }
    public string ConvertedQuantity { get; private set; }

    protected override void OnSelectedSearchResultChanged()
    {
        
    }

}
