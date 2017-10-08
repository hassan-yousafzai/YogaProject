using System.Web.Http;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers.Api
{
    public class SessionsController : ApiController
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionsController(ISessionRepository sessionRepository = null)
        {
            _sessionRepository = sessionRepository ?? new SessionsRepository();
        }

        // DELETE /api/customers/1
        [HttpDelete]
        public void DeleteSession(int id)
        {
            _sessionRepository.DeleteSession(id);
        }
    }
}
