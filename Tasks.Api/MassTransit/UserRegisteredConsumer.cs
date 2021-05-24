using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Tasks.Api.Data;
using Tasks.Api.Entities;
using Users.Data;

namespace Tasks.Api.MassTransit
{
    public class UserRegisteredConsumer : IConsumer<UserRegistered>
    {
        private readonly ApplicationContext _applicationContext;

        private readonly IMapper _mapper;

        public UserRegisteredConsumer(IMapper mapper, ApplicationContext applicationContext)
        {
            _mapper = mapper;
            _applicationContext = applicationContext;
        }

        public async Task Consume(ConsumeContext<UserRegistered> context)
        {
            await _applicationContext.Users.AddAsync(_mapper.Map<User>(context.Message));
            await _applicationContext.SaveChangesAsync();
        }
    }
}