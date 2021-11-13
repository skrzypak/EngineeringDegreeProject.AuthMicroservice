namespace AuthMicroservice.Core.Fluent.Entities.Confirmation
{
    public class PasswordConfirmation : Confirmation
    {
        public int UserDomainId { get; set; }
        public string HashUserCredential { get; set; }
        public virtual UserDomain UserDomain { get; set; }
    }
}
