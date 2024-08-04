using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_webapi_ef.Data;
using dotnet_webapi_ef.DTOs.MeetingDTO;
using dotnet_webapi_ef.Mappers;
using dotnet_webapi_ef.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_webapi_ef.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingController : ControllerBase
    {

        // Attribute context is private and readonly
        // _name => only use in this class => this.xxxx
        private readonly ApplicationDBContext _context;
        public MeetingController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var meetings = _context.Meetings.ToList();
            return Ok(meetings);
        }


        [HttpGet("{id}")]
        public IActionResult GetByID([FromRoute] int id)
        {
            var meeting = _context.Meetings.Find(id);
            if (meeting == null)
            {
                return NotFound();
            }
            return Ok(meeting);
        }

        [HttpPost]
        public IActionResult InsertMeeting([FromBody] MeetingDTO meetingDTO)
        {
            Meeting meeting = meetingDTO.ToMeeting();
            _context.Meetings.Add(meeting);
            int affect = _context.SaveChanges();
            if (affect > 0)
            {
                return CreatedAtAction(nameof(GetByID), new { id = meeting.Idx }, meeting.ToMeetingDTO());
            }
            return StatusCode(400);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMeeting([FromRoute] int id)
        {
            Meeting meeting = _context.Meetings.Find(id);
            if (meeting != null)
            {
                _context.Meetings.Remove(meeting);
                int affect = _context.SaveChanges();
                if (affect > 0)
                {
                    return Ok();
                }
            }
            return StatusCode(400);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateMeeting([FromRoute] int id, [FromBody] MeetingDTO meetingDTO)
        {
            Meeting meeting = _context.Meetings.Find(id);
            if (meeting != null)
            {
                //meeting = MeetingMapper.ToMeeting(meetingDTO);

                meeting.Detail = meetingDTO.Detail;
                meeting.Meetingdatetime = meetingDTO.Meetingdatetime;
                meeting.Latitude = meetingDTO.Latitude;
                meeting.Longitude = meetingDTO.Longitude;

                _context.Meetings.Update(meeting);
                int affect = _context.SaveChanges();
                if (affect > 0)
                {
                    return Accepted(meeting.ToMeetingDTO());
                }
            }
            return StatusCode(400);

        }

    }
}