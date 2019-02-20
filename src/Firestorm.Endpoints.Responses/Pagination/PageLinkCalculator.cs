using System;
using Firestorm.Rest.Web;

namespace Firestorm.Endpoints.Responses.Pagination
{
    /// <summary>
    /// Calculates the next and previous page links based on application config, request querystring and collection details.
    /// </summary>
    public class PageLinkCalculator : IPageLinkCalculator
    {
        private readonly PaginationConfiguration _configuration;

        public PageLinkCalculator(PaginationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PageLinks Calculate(PageInstruction instruction, PageDetails details)
        {
            if (details == null || !details.HasNextPage)
                return null;

            switch (_configuration.SuggestedNavigationType)
            {
                case PageNavigationType.PageNumber:
                    return new PageLinks
                    {
                        Next = new PageInstruction
                        {
                            PageNumber = instruction.PageNumber + 1,
                            Size = instruction.Size
                        }
                    };

                case PageNavigationType.Offset:
                    return new PageLinks
                    {
                        Next = new PageInstruction
                        {
                            Offset = instruction.Offset + instruction.Size,
                            Size = instruction.Size
                        }
                    };

                case PageNavigationType.SortAndFilter:
                    throw new NotImplementedException("Not implemented next page URL for sort and filter strategy.");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}