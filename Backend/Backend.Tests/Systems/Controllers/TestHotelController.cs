using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FakeItEasy;
using Microsoft.Extensions.Options;

namespace Backend.Tests.Systems.Controllers;

public class TestHotelController
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly HotelsController _sut;

    public TestHotelController()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _context = new ApplicationDbContext(options);
        _mapper = A.Fake<IMapper>();
        var _sut = new HotelsController(_context, _mapper);
    }
}