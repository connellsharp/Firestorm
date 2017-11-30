using System;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Pagination;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// Calculates the next and previous page links based on application config, request querystring and collection details.
    /// </summary>
    internal class PageLinkCalculator
    {
        private readonly PageConfiguration _configuration;
        private readonly PageInstruction _instruction;
        private readonly PageDetails _details;

        public PageLinkCalculator(PageConfiguration configuration, PageInstruction instruction, PageDetails details)
        {
            _configuration = configuration;
            _instruction = instruction;
            _details = details;
        }

        public PageLinks Calculate()
        {
            if (_details == null || !_details.HasNextPage)
                return null;

            // TODO query keys

            switch (_configuration.SuggestedNavigationType)
            {
                case PageNavigationType.SortAndFilter:
                    throw new NotImplementedException("Not implemented next page URL for sort and filter strategy.");
                    return new PageLinks
                    {
                        NextPath = "?where=" + _instruction.PageNumber
                    };

                case PageNavigationType.PageNumber:
                    return new PageLinks
                    {
                        NextPath = "?page=" + (_instruction.PageNumber + 1)
                    };

                case PageNavigationType.Offset:
                    return new PageLinks
                    {
                        NextPath = "?offset=" + (_instruction.Offset + _instruction.Size)
                    };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}