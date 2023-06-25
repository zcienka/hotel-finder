using AutoMapper;
using Backend.Controllers;
using Backend.Models;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests.Systems.Controllers
{
    internal class TestCommentsController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly HotelsController _sut;

        public TestCommentsController()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new ApplicationDbContext(options);
            _mapper = A.Fake<IMapper>();
            var _sut = new CommentsController(_context, _mapper);
        }
    }
}