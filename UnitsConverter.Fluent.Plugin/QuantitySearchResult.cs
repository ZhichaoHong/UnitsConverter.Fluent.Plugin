using Blast.Core.Interfaces;
using Blast.Core.Results;

namespace UnitsConverter.Fluent.Plugin
{
    internal class QuantitySearchResult : SearchResultBase
    {
        public QuantitySearchResult(string searchedText, string quantityName, string iconGlyph, IList<ISearchOperation>supportedOperations)
            :base($"Converts Units in {quantityName}", searchedText, quantityName, 1, supportedOperations,
                 new List<SearchTag> { new() { Name = quantityName, IconGlyph = iconGlyph}})
        {
            UseIconGlyph = true;
            IconGlyph = iconGlyph;
        }

        protected override void OnSelectedSearchResultChanged()
        {
        }
    }
}