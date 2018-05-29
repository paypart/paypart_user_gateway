using FluentValidation;
using Microsoft.Extensions.Options;
using paypart_user_gateway.Models;

namespace paypart_user_gateway.Services
{
    public class UserValidator : AbstractValidator<User>
    {
        IOptions<Settings> _settings;

        public UserValidator(IOptions<Settings> settings)
        {
            _settings = settings;
            RuleFor(req => req._id).NotEmpty().WithMessage("User Id is required");
            RuleFor(req => req.username).NotEmpty().WithMessage("Username is required");
            RuleFor(req => req.email).NotEmpty().WithMessage("Email is required");
            RuleFor(req => req.password).NotEmpty().WithMessage("Password is required");
            RuleFor(req => req.role_id).NotEmpty().WithMessage("Role Id is required");
            RuleFor(req => req.datecreated).NotEmpty().WithMessage("Date Created is required");
            RuleFor(req => req.createdby).NotEmpty().WithMessage("Createor is required");
            RuleFor(req => req.status).NotEmpty().WithMessage("Status is required");
        }
    }
    public class LoginValidator : AbstractValidator<Login>
    {
        IOptions<Settings> _settings;

        public LoginValidator(IOptions<Settings> settings)
        {
            _settings = settings;
            RuleFor(req => req.email).NotEmpty().WithMessage("Email is required");
            RuleFor(req => req.password).NotEmpty().WithMessage("Password is required");
        }
    }
}
