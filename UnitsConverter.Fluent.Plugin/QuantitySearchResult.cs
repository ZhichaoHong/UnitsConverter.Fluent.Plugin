using Blast.Core.Interfaces;
using Blast.Core.Objects;
using Blast.Core.Results;

namespace UnitsConverter.Fluent.Plugin
{
    internal class QuantitySearchResult : SearchResultBase
    {
        private static readonly List<string> Examples = new List<string>
        {
            "5m in in",
            "5m in \"",
            "5 ac in m^2",
            "5 minute in d",
            "7j in wd",
            "5KB in b",
            "5w in hp(I)",
            "5000 pa in bar",
            "5c in f",
            "5 m/s in ft/s",
            "5 m³ in ft³"
        };

        private static List<T> GetRandomElements<T>(List<T> allElements, int randomCount = 4)
        {
            if (allElements.Count < randomCount)
            {
                return allElements;
            }

            List<int> indexes = new List<int>();

            // use HashSet if performance is very critical and you need a lot of indexes
            //HashSet<int> indexes = new HashSet<int>(); 

            List<T> elements = new List<T>();

            Random random = new Random();
            while (indexes.Count < randomCount)
            {
                int index = random.Next(allElements.Count);
                if (!indexes.Contains(index))
                {
                    indexes.Add(index);
                    elements.Add(allElements[index]);
                }
            }

            return elements;
        }

        public QuantitySearchResult(string searchedText, List<UnitsNet.QuantityType> quantities, string iconGlyph, IList<ISearchOperation> supportedOperations)
            : base(string.Empty, searchedText, "Converts Units", 1, supportedOperations,
                 new List<SearchTag>())
        {
            UseIconGlyph = true;
            IconGlyph = iconGlyph;
            if (string.IsNullOrWhiteSpace(searchedText))
            {
                List<string> examples = GetRandomElements<string>(Examples, 2);
                InformationElements = new List<InformationElement> { new("Example 1", examples[0]),
                    new("Example 2", examples[1]),
                    new("Quantities: ", string.Join(", ", quantities)) };
            }
        }

        protected override void OnSelectedSearchResultChanged()
        {
        }
    }
}