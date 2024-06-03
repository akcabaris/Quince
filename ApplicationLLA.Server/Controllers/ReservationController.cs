﻿using ApplicationLLA.Server.Dtos.Reservation;
using ApplicationLLA.Server.Extensions;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Mappers;
using ApplicationLLA.Server.Models;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservRepo;
        private readonly IPostRepository _postRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IWorkerRepository _workerRepo;
        private readonly UserManager<AppUser> _userManager;

        public ReservationController(IReservationRepository reservRepo, IPostRepository postRepo, UserManager<AppUser> userManager, ICustomerRepository customerRepo, IWorkerRepository workerRepo)
        {
            _reservRepo = reservRepo;
            _postRepo = postRepo;
            _userManager = userManager;
            _customerRepo = customerRepo;
            _workerRepo = workerRepo;
        }

        [HttpPost]
        [Route("{postId:int}")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromRoute] int postId, CreateReservationDto reservDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userMail = User.GetUserMail();
            var appUSer = await _userManager.FindByEmailAsync(userMail);
            if (appUSer == null)
            {
                return BadRequest("User does not exists");
            }

            var customer = await _customerRepo.GetByUserIdAsync(appUSer.Id.ToString());

            if (customer == null)
            {
                return BadRequest("Worker does not exists");
            }

            var post = await _postRepo.GetPostByIdAsync(postId);

            if (post == null) { return BadRequest("Post couldn't found"); }

            if (await _reservRepo.CheckReservationExistsAsync(customer, post)) return Ok("Reservation Already Exists");


            var reservModel = reservDto.ToReservationFromCreate(postId, customer.AppUserId);

            if (reservModel == null) return BadRequest("something went wrong");

            await _reservRepo.CreateAsync(reservModel);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userMail = User.GetUserMail();
            var appUSer = await _userManager.FindByEmailAsync(userMail);
            if (appUSer == null)
            {
                return BadRequest("User does not exists");
            }


            var reservModel = await _reservRepo.DeleteAsync(id);

            if (reservModel == null) return NotFound();

            if (appUSer.CustomerId != null && (reservModel.CustomerId != appUSer.CustomerId.ToString()))
            {
                return BadRequest("You can't delete other user's reservations");
            }

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsersReservations()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null)
            {
                return BadRequest("User does not exists");
            }

            var customer = await _customerRepo.GetByUserIdAsync(appUser.Id.ToString());

            if (customer == null)
            {
                return BadRequest("Customer does not exists");
            }

            List<UserReservationDto> reservationDtoList = new List<UserReservationDto>();

            var reservations = await _reservRepo.GetUsersReservationsAsync(customer);
            if (reservations == null) return BadRequest();

            foreach (var reservation in reservations)
            {
                var post = await _postRepo.GetPostByIdAsync(reservation.PostId);

                if (post != null && reservation != null)
                {
                    var worker = await _workerRepo.GetByUserIdAsync(post.WorkerId);

                    if (worker != null && worker.FullName != null)
                    {
                        reservationDtoList.Add(reservation.ToUserReservationDto(post.ToPostDto(worker.FullName, worker.PictureLink)));
                    }
                }
            }

            return Ok(reservationDtoList);
        }

        [HttpPut]
        [Authorize(Roles = "Customer, Worker")]
        [Route("{idReserv:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveOrDenyReservation([FromRoute] int idReserv, ReservationFinalizeDto reservDto)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null)
            {
                return BadRequest("User does not exists");
            }
            if (reservDto == null) return BadRequest();


            if (!(reservDto.Status == "Approved" || reservDto.Status == "Denied" || reservDto.Status == "Done")) return BadRequest("Status can be just 'Approved' or 'Denied' or 'Done'");

            var existsReservation = await _reservRepo.GetByIdAsync(idReserv);
            if (existsReservation == null) return NotFound();

            if (reservDto.Status == "Done" && appUser.Id.ToString() == existsReservation.CustomerId)
            {
                if (existsReservation.ReservationDate > DateTime.Today.AddDays(1))
                {
                    return BadRequest("The reservation can be marked as done at least after the reservation day.");
                }
                var reservModel = await _reservRepo.ApproveOrDenyReservationAsync(existsReservation.ReservationId, reservDto.Status.ToString());
                if (reservModel != null) return Ok();
            }
            else if (reservDto.Status == "Approved" || reservDto.Status == "Denied")
            {
                var post = await _postRepo.GetPostByIdAsync(existsReservation.PostId);
                var worker = await _workerRepo.GetByUserIdAsync(appUser.Id.ToString());

                if (post == null || worker == null) return NotFound();

                if (post.WorkerId == worker.AppUserId)
                {
                    var reservModel = await _reservRepo.ApproveOrDenyReservationAsync(existsReservation.ReservationId, reservDto.Status.ToString());
                    if (reservModel != null) return Ok();
                }

            }
            return BadRequest();
        }
    }
}