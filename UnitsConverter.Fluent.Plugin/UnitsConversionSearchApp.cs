using Blast.API.Search;
using Blast.API.Search.SearchOperations;
using Blast.Core;
using Blast.Core.Interfaces;
using Blast.Core.Objects;
using Blast.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TextCopy;
using UnitsNet;

namespace UnitsConverter.Fluent.Plugin
{
    public class UnitsConversionSearchApp : ISearchApplication
    {
        private const string SearchAppName = "UnitsConverter";
        private const string IconGlyph = "\uE8EF";
        private const string UnitsConverterSearchTag = "unitsconverter";
        private readonly List<SearchTag> _searchTags;
        private SearchApplicationInfo _applicationInfo;
        private readonly List<ISearchOperation> _supportedOperations;
        private readonly List<ISearchOperation> _convertedOperations;

        private List<QuantityType> _supportedQuantities = new List<QuantityType> {
            QuantityType.Acceleration,
            QuantityType.Angle,
            QuantityType.Area,
            QuantityType.Duration,
            QuantityType.Energy,
            QuantityType.Information,
            QuantityType.Length,
            QuantityType.Mass,
            QuantityType.Power,
            QuantityType.Pressure,
            QuantityType.Speed,
            QuantityType.Temperature,
            QuantityType.Volume };

        public UnitsConversionSearchApp()
        {
            _searchTags = new List<SearchTag> { new SearchTag() { Name = "unitsconverter", Description = "Converts Units", IconGlyph = IconGlyph } };

            var copySearchOperation = new CopySearchOperation();
            var unitsConversionSearchOperation = new UnitsConversionSearchOperation();

            _supportedOperations = new List<ISearchOperation>
            {
                new UnitsConversionSearchOperation()
            };

            _convertedOperations = new List<ISearchOperation> { new CopySearchOperation() };

            _applicationInfo = new SearchApplicationInfo(SearchAppName, "This app converts units", 
                new SearchOperationBase[] { copySearchOperation, unitsConversionSearchOperation })
            {
                IsProcessSearchEnabled = false,
                IsProcessSearchOffline = false,
                SearchTagOnly = false,
                ApplicationIconGlyph = IconGlyph,
                SearchAllTime = ApplicationSearchTime.Fast,
                DefaultSearchTags = _searchTags
            };
        }
        public SearchApplicationInfo GetApplicationInfo() => _applicationInfo;

        public ValueTask LoadSearchApplicationAsync()
        {
            // This is used if you need to load anything asynchronously on Fluent Search startup
            return ValueTask.CompletedTask;
        }

        public ValueTask<IHandleResult> HandleSearchResult(ISearchResult searchResult)
        {
            switch (searchResult)
            {

                case UnitsConversionSearchResult:
                    string resultToCopy = searchResult.ResultName;
                    Clipboard.SetText(resultToCopy);
                    return new ValueTask<IHandleResult>(new HandleResult(true, false));
                case QuantitySearchResult:
                    SearchTag searchTag = searchResult.Tags.FirstOrDefault();
                    return new ValueTask<IHandleResult>(new HandleResult(true, true)
                    {
                        SearchRequest = new SearchRequest(string.Empty, searchTag?.Name, SearchType.SearchAll),
                        SearchTag = searchTag
                    });
                default:
                    return new(); // default do nothing
            }
        }

        public ValueTask<ISearchResult> GetSearchResultForId(string serializedSearchObjectId)
        {
            // This is used to calculate a search result after Fluent Search has been restarted
            // This is only used by the custom search tag feature
            return new();
        }

        public async IAsyncEnumerable<ISearchResult> SearchAsync(SearchRequest searchRequest, [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested || searchRequest.SearchType == SearchType.SearchProcess)
                yield break;

            string searchedTag = searchRequest.SearchedTag;
            string searchedText = searchRequest.SearchedText;

            if (string.IsNullOrWhiteSpace(searchedText))
            {
                yield return new QuantitySearchResult(searchedText, _supportedQuantities, IconGlyph, _supportedOperations);
            }

            string query = searchedText;
            ConvertModel model = InputInterpreter.Parse(query);
            if (model == null)
            {
                yield break;
            }

            if (model.ToUnit != null || searchedTag.Equals(UnitsConverterSearchTag))
            {
                var quantities = UnitHandler.Convert(model);
                foreach (var q in quantities)
                {
                    double score = (searchedTag != UnitsConverterSearchTag) ? 90.0 : 1.0;
                    yield return new UnitsConversionSearchResult(searchedText, q, IconGlyph, _convertedOperations, _searchTags, score);
                }
            }
        }
    }
}
