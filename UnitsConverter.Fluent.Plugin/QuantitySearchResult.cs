using Blast.Core.Interfaces;
using Blast.Core.Objects;
using Blast.Core.Results;

namespace UnitsConverter.Fluent.Plugin
{
    internal class QuantitySearchResult : SearchResultBase
    {
        public QuantitySearchResult(string searchedText, List<UnitsNet.QuantityType> quantities, string iconGlyph, IList<ISearchOperation> supportedOperations)
            : base(string.Empty, searchedText, "Converts Units", 1, supportedOperations,
                 new List<SearchTag>())
        {
            UseIconGlyph = true;
            IconGlyph = iconGlyph;
            if (string.IsNullOrWhiteSpace(searchedText))
            {
                InformationElements = new List<InformationElement> { new("Example 1", "5 m in in"),
                    new("Example 2", "5 lbs to kg"),
                    new("Quantities: ", string.Join(", ", quantities)) };
            }
        }

        protected override void OnSelectedSearchResultChanged()
        {
        }
    }
}