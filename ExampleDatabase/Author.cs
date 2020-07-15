using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExampleDatabase
{
    public class Author
    {
        public const int NameLength = 100;
        public const int EmailLength = 100;

        public Author() { }

        public int AuthorId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(NameLength)]
        public string Name { get; set; }

        [MaxLength(EmailLength)]
        public string Email { get; set; }

        //------------------------------
        //Relationships

        public ICollection<BookAuthor> BooksLink { get; set; }
    }

}
