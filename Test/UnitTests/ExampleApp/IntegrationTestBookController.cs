// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using AspNetCore;
using AspNetCore.UnitTesting;
using CommonWebParts;
using CommonWebParts.Dtos;
using ExampleDatabase;
using ExampleWebApi.Controllers;
using GenericServices.AspNetCore;
using GenericServices.Configuration;
using GenericServices.PublicButHidden;
using GenericServices.Setup;
using Microsoft.AspNetCore.JsonPatch;
using Test.Helpers;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests.ExampleApp
{
    public class IntegrationTestBookController
    {
        private readonly IGenericServicesConfig _genericServiceConfig = new GenericServicesConfig
        {
            DtoAccessValidateOnSave = true, //This causes validation to happen on create/update via DTOs
            DirectAccessValidateOnSave = true, //This causes validation to happen on direct create/update and delete
            NoErrorOnReadSingleNull = true //When working with WebAPI you should set this flag. Response then sends 204 on null result
        };

        [Fact]
        public async Task TestDeleteReviewOk()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<ExampleDbContext>();
            using (var context = new ExampleDbContext(options))
            {
                context.Database.EnsureCreated();
                context.SeedDatabase();

                var controller = new BookController();
                var utData = context.SetupSingleDtoAndEntities<AddReviewDto>(_genericServiceConfig);
                var service = new CrudServicesAsync(context, utData.ConfigAndMapper);

                //ATTEMPT
                var dto = new AddReviewDto()
                {
                    BookId = 1,
                    Title = "Efcore in action",
                    VoterName = "test",
                    NumStars = 5,
                    Comment = "test comment",
                };
                var response = await controller.AddReview(dto, service);

                //VERIFY
                response.GetStatusCode().ShouldEqual(200);


                //Delete review

                utData = context.SetupSingleDtoAndEntities<RemoveReviewDto>(_genericServiceConfig);
                service = new CrudServicesAsync(context, utData.ConfigAndMapper);

                //ATTEMPT
                var removeReviewDto = new RemoveReviewDto()
                {
                    BookId = 1,
                    ReviewId = 1
                };
                response = await controller.DeleteReview(removeReviewDto, service);

                //VERIFY
                response.GetStatusCode().ShouldEqual(200);


            }
        }

    }
}