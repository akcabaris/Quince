using ApplicationLLA.Server.Dtos.Post;
using ApplicationLLA.Server.Dtos.Reservation;
using ApplicationLLA.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Mappers
{
    public static class PostMapper
    {
        public static AllPostDto ToPostDto(this Post postModel, string workerName, string pictureLink)
        {
            return new AllPostDto
            {
                PostId = postModel.PostId,
                Category = postModel.Category,
                City = postModel.City,
                County = postModel.County,
                Description = postModel.Description,
                IsPostActive = postModel.IsPostActive,
                Price = postModel.Price,
                pictureLink = "http://localhost:5279/resources/" + pictureLink,
                PriceCurrency = postModel.PriceCurrency,
                PriceWorkUnit = postModel.PriceWorkUnit,
                Title = postModel.Title,
                WorkerId = postModel.WorkerId,
                WorkerName = workerName
            };

        }

        public static PostDto ToPostDtoOne(this Post postModel)
        {
            return new PostDto
            {
                PostId = postModel.PostId,
                Category = postModel.Category,
                City = postModel.City,
                County = postModel.County,
                Description = postModel.Description,
                IsPostActive = postModel.IsPostActive,
                Price = postModel.Price,
                PriceCurrency= postModel.PriceCurrency,
                PriceWorkUnit = postModel.PriceWorkUnit,
                Title = postModel.Title,
                WorkerId = postModel.WorkerId
            };

        }

        public static Post ToPost(this PostDto postDto, string workerId)
        {
            return new Post
            {
                PostId = postDto.PostId,
                Category = postDto.Category,
                City = postDto.City,
                County = postDto.County,
                Description = postDto.Description,
                IsPostActive = postDto.IsPostActive,
                Price = postDto.Price,
                PriceCurrency = postDto.PriceCurrency,
                PriceWorkUnit= postDto.PriceWorkUnit,
                Title = postDto.Title,
                WorkerId = workerId
            };

        }

        public static UsersPostsDto? ToUsersPostsDto(this Post post, int countOfWaitingReservations)
        {
            if (post == null) return null;
            return new UsersPostsDto
            {
                PostId = post.PostId,
                Category = post.Category,
                City = post.City,
                County = post.County,
                Description = post.Description,
                IsPostActive = post.IsPostActive,
                Price = post.Price,
                PriceCurrency= post.PriceCurrency,
                PriceWorkUnit = post.PriceWorkUnit,
                Title = post.Title,
                WorkerId = post.WorkerId,
                CountOfWaitingReservation = countOfWaitingReservations,
            };
        }

        public static Post ToPostFromCreate(this CreatePostDto postDto, string workerId)
        { 
            return new Post
            {
                Category = postDto.Category,
                City = postDto.City,
                County = postDto.County,
                Description = postDto.Description,
                IsPostActive = true,
                Price = postDto.Price,
                PriceCurrency = postDto.PriceCurrency,
                PriceWorkUnit= postDto.PriceWorkUnit,
                Title = postDto.Title,
                WorkerId = workerId
            };
            
        }

        public static Post ToPostFromUpdate(this UpdatePostDto postDto, string workerId, int postId)
        {
            return new Post
            {

                PostId = postId,
                Category = postDto.Category,
                City = postDto.City,
                County = postDto.County,
                Description = postDto.Description,
                IsPostActive = postDto.IsPostActive,
                Price = postDto.Price,
                PriceCurrency= postDto.PriceCurrency,
                PriceWorkUnit = postDto.PriceWorkUnit,
                Title = postDto.Title,
                WorkerId = workerId

            };

        }
        public static DeletedPostDto ToPostDtoFromDelete(this Post postModel)
        {
            return new DeletedPostDto
            {
                PostId = postModel.PostId,
                Title = postModel.Title
            };
        }

    }
}
