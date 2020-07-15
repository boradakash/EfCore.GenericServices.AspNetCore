using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExampleDatabase
{
    public class Review
    {
        public const int NameLength = 100;

        private Review() { }

        internal Review(int numStars, string comment, string voterName, int bookId = 0)
        {
            NumStars = numStars;
            Comment = comment;
            VoterName = voterName;
            BookId = bookId;
        }

        public int ReviewId { get; private set; }

        [MaxLength(NameLength)]
        public string VoterName { get; private set; }

        public int NumStars { get; private set; }
        public string Comment { get; private set; }

        //-----------------------------------------
        //Relationships

        public int BookId { get; private set; }
    }
}
