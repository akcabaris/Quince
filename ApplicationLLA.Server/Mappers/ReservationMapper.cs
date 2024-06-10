using ApplicationLLA.Server.Dtos.Post;
using ApplicationLLA.Server.Dtos.Reservation;
using ApplicationLLA.Server.Dtos.Review;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Mappers
{
    public static class ReservationMapper
    {

        public static Reservation ToReservationFromCreate (this CreateReservationDto reservDto, int postId, string customerId)
        {
            return new Reservation
            {
                CustomerId = customerId,
                PostId = postId,
                ReservationDate = reservDto.ReservationDate,
                ReservationNote = reservDto.ReservationNote,
                
                Status = "Waiting",
            };

        }
        public static ReservationDto ToReservationDto (this Reservation reservModel, string postTitle, string customerName, string? pictureLink)
        {
            return new ReservationDto
            {
                ReservationId = reservModel.ReservationId,
                Status = reservModel.Status,
                ReservationDate = reservModel.ReservationDate,
                PostId = reservModel.PostId,
                ReservationNote = reservModel.ReservationNote,
                CustomerPictureLink = "http://localhost:5279/resources/" + pictureLink,
                PostTitle = postTitle,
                CustomerId = reservModel.CustomerId,
                CustomerName = customerName
            };
        }

        public static UserReservationDto ToUserReservationDto(this Reservation reservModel, PostDto postDto, ReviewDto? reviewDto)
        {
            return new UserReservationDto
            {
                ReservationId = reservModel.ReservationId,
                Status = reservModel.Status,
                ReservationDate = reservModel.ReservationDate,
                ReservationNote = reservModel.ReservationNote,
                CustomerId = reservModel.CustomerId,
                PostDto = postDto,
                ReviewDto = reviewDto,
            };
        }



    }
}
