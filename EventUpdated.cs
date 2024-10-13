using System;

public class EventUpdated
{
	public EventUpdated()
	{
		string Title { get; set; }
		string Message { get; set; }
		DateTime DateTime { get; set; }
		List<Guid> participantsId { get; set; }
	}
}
