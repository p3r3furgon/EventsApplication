namespace Users.Application.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(Guid id) : base($"User with id:({id}) was not found.")
        {
        }
    }
}
