namespace PCParts.Domain.Exceptions
{
    public class EntityAlreadyExistsException : DomainException
    {
        public EntityAlreadyExistsException(string phone)
            : base(DomainErrorCode.NotFound, $"The user with this phone {phone} already exists!")
        {
        }
    }
}
