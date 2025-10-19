using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.GetAllUser
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<User>>
    {
        private readonly IUserRepository _repository;
        public GetAllUserQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var listUser = await _repository.GetAllAsync();
            return listUser.ToList();
        }
    }
}
