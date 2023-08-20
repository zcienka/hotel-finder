using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Backend.Dtos;
using Backend.Requests;
using Backend.Data;
using Backend.Core.IConfiguration;

namespace Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReservationsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResult<ReservationDto>>> GetReservations([FromQuery] PagingQuery query)
    {
        if (!int.TryParse(query.Limit, out int limitInt)
            || !int.TryParse(query.Offset, out int offsetInt))
        {
            return NotFound();
        }

        var reservations = await _unitOfWork.Reservations.GetAll();
        var reservationsDtos =
            reservations.Select(reservation => _mapper.Map<ReservationDto>(reservation)).ToList();

        return await ApiResult<ReservationDto>.CreateAsync(
            reservationsDtos,
            offsetInt,
            limitInt,
            "/reservations"
        );
    }


    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ReservationDto>> GetReservation(string id)
    {
        var reservationExists = await _unitOfWork.Reservations.Exists(id);


        if (!reservationExists)
        {
            return NotFound();
        }

        var reservation = _unitOfWork.Reservations.GetById(id);
        var reservationDto = _mapper.Map<ReservationDto>(reservation);

        return reservationDto;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutReservation(string id, ReservationRequest reservationRequest)
    {
        if (!id.Equals(reservationRequest.Id))
        {
            return BadRequest();
        }

        var hotelExists = await _unitOfWork.Hotels.Exists(reservationRequest.HotelId);

        if (!hotelExists)
        {
            return NotFound("Hotel not found");
        }

        var userExists = await _unitOfWork.Users.Exists(reservationRequest.UserEmail);

        if (!userExists)
        {
            return NotFound("User not found");
        }


        var reservation = _mapper.Map<Reservation>(reservationRequest);

        try
        {
            _unitOfWork.Reservations.Update(reservation);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ReservationExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Reservation>> PostReservation(ReservationRequest reservationRequest)
    {
        var hotelExists = await _unitOfWork.Hotels.Exists(reservationRequest.HotelId);

        if (!hotelExists)
        {
            return NotFound("Hotel not found");
        }

        var isRoomInHotel =
            _unitOfWork.Reservations.IsRoomInHotel(reservationRequest.HotelId, reservationRequest.RoomId);

        if (!isRoomInHotel)
        {
            return NotFound("Room with that id not found");
        }

        if (reservationRequest.CheckOutDate <= reservationRequest.CheckInDate)
        {
            return BadRequest(
                "Check-out date must be later than the check-in date.");
        }

        var userExists = await _unitOfWork.Users.Exists(reservationRequest.UserEmail);

        if (!userExists)
        {
            return NotFound("User not found");
        }

        var reservations = await _unitOfWork.Reservations.GetAll();

        bool isReservationConflict = reservations.Any(r =>
            r.RoomId == reservationRequest.RoomId && r.HotelId == reservationRequest.HotelId &&
            !(reservationRequest.CheckOutDate <= r.CheckInDate || reservationRequest.CheckInDate >= r.CheckOutDate));

        if (isReservationConflict)
        {
            return BadRequest("Reservation conflicts with existing reservations");
        }

        var reservation = _mapper.Map<Reservation>(reservationRequest);
        await _unitOfWork.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservationRequest);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteReservation(string id)
    {
        var reservation = await _unitOfWork.Reservations.GetById(id);

        if (reservation == null)
        {
            return NotFound();
        }

        _unitOfWork.Reservations.Delete(reservation);

        return NoContent();
    }

    private async Task<bool> ReservationExists(string id)
    {
        return await _unitOfWork.Reservations.Exists(id);
    }
}