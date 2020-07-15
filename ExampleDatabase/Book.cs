﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;

namespace ExampleDatabase
{

    public class Book
    {
        public const int PromotionalTextLength = 200;
        //-----------------------------------------------
        //relationships

        //Use uninitialised backing fields - this means we can detect if the collection was loaded
        private HashSet<Review> _reviews;

        //-----------------------------------------------
        //ctors

        private Book() { }

        public Book(string title, string description, DateTime publishedOn,
            string publisher, decimal price, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));

            Title = title;
            Description = description;
            PublishedOn = publishedOn;
            Publisher = publisher;
            ActualPrice = price;
            OrgPrice = price;
            ImageUrl = imageUrl;
            _reviews = new HashSet<Review>();       //We add an empty list on create. I allows reviews to be added when building test data
        }

        public static IStatusGeneric<Book> CreateBook(string title, string description,
            string publisher, decimal price, string imageUrl)
        {
            var status = new StatusGenericHandler<Book>();
            if (string.IsNullOrWhiteSpace(title))
                status.AddError("The book title cannot be empty.");

            var book = new Book
            {
                Title = title,
                Description = description,
                PublishedOn = DateTime.Now,
                Publisher = publisher,
                ActualPrice = price,
                OrgPrice = price,
                ImageUrl = imageUrl,
                _reviews = new HashSet<Review>()       //We add an empty list on create. I allows reviews to be added when building test data
            };
            return status.SetResult(book);
        }

        public int BookId { get; private set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime PublishedOn { get; set; }
        public string Publisher { get; private set; }
        public decimal OrgPrice { get; private set; }
        public decimal ActualPrice { get; private set; }

        [MaxLength(PromotionalTextLength)]
        public string PromotionalText { get; private set; }

        public string ImageUrl { get; private set; }

        public IEnumerable<Review> Reviews => _reviews?.ToList();

        public void UpdatePublishedOn(DateTime publishedOn)
        {
            PublishedOn = publishedOn;
        }

        public void AddReview(int numStars, string comment, string voterName, DbContext context = null)
        {
            if (_reviews != null)
            {
                _reviews.Add(new Review(numStars, comment, voterName));
            }
            else if (context == null)
            {
                throw new ArgumentNullException(nameof(context),
                    "You must provide a context if the Reviews collection isn't valid.");
            }
            else if (context.Entry(this).IsKeySet)
            {
                context.Add(new Review(numStars, comment, voterName, BookId));
            }
            else
            {
                throw new InvalidOperationException("Could not add a new review.");
            }
        }

        //This works with the GenericServices' IncludeThen Attribute to pre-load the Reviews collection
        public void AddReviewWithInclude(int numStars, string comment, string voterName)
        {
            if (_reviews == null)
                throw new InvalidOperationException("The Reviews collection must be loaded before calling this method");
            _reviews.Add(new Review(numStars, comment, voterName));
        }

        public void RemoveReview(Review review, DbContext context = null)
        {
            if (_reviews != null)
            {
                //This is there to handle the add/remove of reviews when first created (or someone uses an .Include(p => p.Reviews)
                _reviews.Remove(review);
            }
            else if (context == null)
            {
                throw new ArgumentNullException(nameof(context),
                    "You must provide a context if the Reviews collection isn't valid.");
            }
            else if (review.BookId != BookId || review.ReviewId <= 0)
            {
                // This ensures that the review is a) linked to the book you defined, and b) the review has a valid primary key
                throw new InvalidOperationException("The review either hasn't got a valid primary key or was not linked to the Book.");
            }
            else
            {
                //NOTE: EF Core can delete a entity even if it isn't loaded - it just has to have a valid primary key.
                context.Remove(review);
            }
        }

        //This works with the GenericServices' IncludeThen Attribute to pre-load the Reviews collection
        public void RemoveReviewWithInclude(int reviewId)
        {
            if (_reviews == null)
                throw new InvalidOperationException("The Reviews collection must be loaded before calling this method");
            var localReview = _reviews.SingleOrDefault(x => x.ReviewId == reviewId);
            if (localReview == null)
                throw new InvalidOperationException("The review with that key was not found in the book's Reviews.");
            _reviews.Remove(localReview);
        }

        public IStatusGeneric AddPromotion(decimal actualPrice, string promotionalText)
        {
            var status = new StatusGenericHandler();
            if (string.IsNullOrWhiteSpace(promotionalText))
            {
                status.AddError("You must provide some text to go with the promotion.", nameof(PromotionalText));
                return status;
            }

            ActualPrice = actualPrice;
            PromotionalText = promotionalText;

            status.Message = $"The book's new price is ${actualPrice:F}.";

            return status;
        }

        public void RemovePromotion()
        {
            ActualPrice = OrgPrice;
            PromotionalText = null;
        }

     
    }

}