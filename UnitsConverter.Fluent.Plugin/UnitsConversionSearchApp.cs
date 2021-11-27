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
            _searchTags = _supportedQuantities.Select(x => new SearchTag { Name = x.ToString(), Description = $"Converts {x.ToString()} Units", IconGlyph = IconGlyph }).ToList();

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
                SearchTagOnly = true,
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
            Type type = searchResult.GetType();

            if (type == typeof(QuantitySearchResult))
            {
                // This will cause Fluent Search to search again using the selected quantity type
                SearchTag searchTag = searchResult.Tags.FirstOrDefault();
                return new ValueTask<IHandleResult>(new HandleResult(true, true)
                {
                    SearchRequest = new SearchRequest(string.Empty, searchTag?.Name, SearchType.SearchAll),
                    SearchTag = searchTag
                });
            }

            // Type is UnitsConversionSearchResult
            string resultToCopy = searchResult.ResultName;
            Clipboard.SetText(resultToCopy);
            return new ValueTask<IHandleResult>(new HandleResult(true, false));
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

            QuantityType quantityType = QuantityType.Undefined;
            if (!searchedTag.Equals(UnitsConverterSearchTag) &&
                !_supportedQuantities.Any(x => Enum.TryParse<QuantityType>(searchedTag, true, out quantityType)))
                yield break;

            if (quantityType == QuantityType.Undefined)
            {
                bool searchAll = string.IsNullOrWhiteSpace(searchedText);
                foreach(string quantityName in _supportedQuantities.Select(x => x.ToString()))
                {
                    if (searchAll || quantityName.SearchBlind(searchedTag))
                        yield return new QuantitySearchResult(searchedText, quantityName, IconGlyph, _supportedOperations);
                }
                yield break;
            }

            var results = new List<IQuantity>();
            try
            {
                string[] parts = searchedText.Split(new string[] { "in", "to" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    results = ConvertUtil.Convert(parts[0].Trim(), quantityType, parts[1].Trim());
                }
                else
                {
                    results = ConvertUtil.Convert(parts[0].Trim(), quantityType);
                }
                if (results.Count == 0)
                {
                    yield break;
                }
            }
            catch (Exception ex)
            {
                yield break;
            }
            foreach (var r in results)
            {
                yield return new UnitsConversionSearchResult(searchedText, r, IconGlyph, _convertedOperations, _searchTags);
            }

        }
    }
}
