using ExampleDatabase;
using GenericServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonWebParts.Dtos
{
    public class RemoveReviewDto : ILinkToEntity<Book>
    {
        public int BookId { get; set; }
        public int ReviewId { get; set; }
    }
}
