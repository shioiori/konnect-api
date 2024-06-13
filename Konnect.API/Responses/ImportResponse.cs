using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Responses
{
    public class ImportResponse : Response
    {
    }
  
    public class ImportUserToDatabaseResponse : Response
    {
    }

    public class ImportTimetableResponse : ImportResponse
    {
        public List<EventDTO> Events { get; set; }
    }
}
