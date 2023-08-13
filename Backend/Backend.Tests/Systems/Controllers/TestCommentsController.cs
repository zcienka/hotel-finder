using AutoMapper;
using FakeItEasy;


namespace Backend.Tests.Systems.Controllers
{
    public class TestCommentsController
    {
        private readonly IMapper _mapper;

        public TestCommentsController()
        {
            _mapper = A.Fake<IMapper>();
        }
    }
}