using ExampleDatabase;
using GenericServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace CommonWebParts.Dtos
{
    public class AddReviewDto : ILinkToEntity<Book>
    {
        [HiddenInput]
        public int BookId { get; set; }

        [ReadOnly(true)]
        public string Title { get; set; }

        [MaxLength(Review.NameLength)]
        public string VoterName { get; set; }

        public int NumStars { get; set; }
        public string Comment { get; set; }
    }
}
