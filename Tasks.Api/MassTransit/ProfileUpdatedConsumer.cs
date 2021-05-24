using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Tasks.Api.Data;
using Users.Data;

namespace Tasks.Api.MassTransit
{
    public class ProfileUpdatedConsumer : IConsumer<ProfileUpdated>
    {
        private readonly ApplicationContext _applicationContext;
        
        private readonly IMapper _mapper;

        public ProfileUpdatedConsumer(IMapper mapper, ApplicationContext applicationContext)
        {
            _mapper = mapper;
            _applicationContext = applicationContext;
        }

        public async Task Consume(ConsumeContext<ProfileUpdated> context)
        {
            var updatedProfile = context.Message;
            var user = _applicationContext.Users.FirstOrDefault(x => x.UserId == updatedProfile.UserId);
            var updatedUser = _mapper.Map(updatedProfile, user);

            _applicationContext.Users.Update(updatedUser ?? throw new Exception("Can`t map updated profile"));
            await _applicationContext.SaveChangesAsync();
        }
    }
}