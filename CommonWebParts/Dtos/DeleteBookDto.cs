using ExampleDatabase;
using GenericServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonWebParts.Dtos
{
    public class DeleteBookDto : ILinkToEntity<Book>
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorsOrdered { get; set; }
    }
}
