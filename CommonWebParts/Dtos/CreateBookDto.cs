using ExampleDatabase;
using GenericServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CommonWebParts.Dtos
{
    public class CreateBookDto : ILinkToEntity<Book>
    {
        //This will be populated with the primary key of the created book
        public int BookId { get; set; }

        //I would normally have the Required attribute to catch this at the front end
        //But to show how the static create method catches that error I have commented it out
        //[Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public string Publisher { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public CreateBookDto()
        {
        }

        //---------------------------------------------------------
        //Now the data for the front end

        public struct KeyName
        {
            public KeyName(int authorId, string name)
            {
                AuthorId = authorId;
                Name = name;
            }

            public int AuthorId { get; }
            public string Name { get; }
        }

        public List<KeyName> AllPossibleAuthors { get; private set; }

        public void BeforeDisplay(ExampleDbContext context)
        {
            AllPossibleAuthors = context.Set<Author>().Select(x => new KeyName(x.AuthorId, x.Name))
                .OrderBy(x => x.Name).ToList();
        }

   
    }
}
