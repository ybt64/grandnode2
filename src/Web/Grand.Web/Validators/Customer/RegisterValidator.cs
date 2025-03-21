﻿using FluentValidation;
using Grand.Domain.Customers;
using Grand.Infrastructure.Validators;
using Grand.Business.Common.Interfaces.Directory;
using Grand.Business.Common.Interfaces.Localization;
using Grand.Web.Models.Customer;
using Grand.SharedKernel.Extensions;

namespace Grand.Web.Validators.Customer
{
    public class RegisterValidator : BaseGrandValidator<RegisterModel>
    {
        public RegisterValidator(
            IEnumerable<IValidatorConsumer<RegisterModel>> validators,
            ITranslationService translationService,
            ICountryService countryService,
            CustomerSettings customerSettings)
            : base(validators)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(translationService.GetResource("Common.WrongEmail"));


            if (customerSettings.UsernamesEnabled)
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.Username.Required"));
            }

            if (customerSettings.FirstLastNameRequired)
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.FirstName.Required"));
                RuleFor(x => x.LastName).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.LastName.Required"));
            }

            RuleFor(x => x.Password).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.Password.Required"));

            if (!string.IsNullOrEmpty(customerSettings.PasswordRegularExpression))
                RuleFor(x => x.Password).Matches(customerSettings.PasswordRegularExpression).WithMessage(string.Format(translationService.GetResource("Account.Fields.Password.Validation")));

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.ConfirmPassword.Required"));
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(translationService.GetResource("Account.Fields.Password.EnteredPasswordsDoNotMatch"));

            //form fields
            if (customerSettings.CountryEnabled && customerSettings.CountryRequired)
            {
                RuleFor(x => x.CountryId)
                    .NotNull()
                    .WithMessage(translationService.GetResource("Address.Fields.Country.Required"));
                RuleFor(x => x.CountryId)
                    .NotEqual("")
                    .WithMessage(translationService.GetResource("Address.Fields.Country.Required"));
            }
            if (customerSettings.CountryEnabled &&
                customerSettings.StateProvinceEnabled &&
                customerSettings.StateProvinceRequired)
            {
                RuleFor(x => x.StateProvinceId).MustAsync(async (x, y, context) =>
                {
                    //does selected country has states?
                    var countryId = !string.IsNullOrEmpty(x.CountryId) ? x.CountryId : "";
                    var country = await countryService.GetCountryById(countryId);
                    if (country != null && country.StateProvinces.Any())
                    {
                        //if yes, then ensure that state is selected
                        if (string.IsNullOrEmpty(y))
                        {
                            return false;
                        }
                        if (country.StateProvinces.FirstOrDefault(x => x.Id == y) != null)
                            return true;
                    }
                    return false;

                }).WithMessage(translationService.GetResource("Account.Fields.StateProvince.Required"));
            }
            if (customerSettings.DateOfBirthEnabled && customerSettings.DateOfBirthRequired)
            {
                RuleFor(x => x.DateOfBirthDay).Must((x, context) =>
                {
                    var dateOfBirth = x.ParseDateOfBirth();
                    if (!dateOfBirth.HasValue)
                        return false;

                    return true;
                }).WithMessage(translationService.GetResource("Account.Fields.DateOfBirth.Required"));

                //minimum age
                RuleFor(x => x.DateOfBirthDay).Must((x, context) =>
                {
                    var dateOfBirth = x.ParseDateOfBirth();
                    if (dateOfBirth.HasValue && customerSettings.DateOfBirthMinimumAge.HasValue &&
                        CommonHelper.GetDifferenceInYears(dateOfBirth.Value, DateTime.Today) <
                        customerSettings.DateOfBirthMinimumAge.Value)
                        return false;

                    return true;
                }).WithMessage(string.Format(translationService.GetResource("Account.Fields.DateOfBirth.MinimumAge"), customerSettings.DateOfBirthMinimumAge));
            }
            if (customerSettings.CompanyRequired && customerSettings.CompanyEnabled)
            {
                RuleFor(x => x.Company).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.Company.Required"));
            }
            if (customerSettings.StreetAddressRequired && customerSettings.StreetAddressEnabled)
            {
                RuleFor(x => x.StreetAddress).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.StreetAddress.Required"));
            }
            if (customerSettings.StreetAddress2Required && customerSettings.StreetAddress2Enabled)
            {
                RuleFor(x => x.StreetAddress2).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.StreetAddress2.Required"));
            }
            if (customerSettings.ZipPostalCodeRequired && customerSettings.ZipPostalCodeEnabled)
            {
                RuleFor(x => x.ZipPostalCode).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.ZipPostalCode.Required"));
            }
            if (customerSettings.CityRequired && customerSettings.CityEnabled)
            {
                RuleFor(x => x.City).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.City.Required"));
            }
            if (customerSettings.PhoneRequired && customerSettings.PhoneEnabled)
            {
                RuleFor(x => x.Phone).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.Phone.Required"));
            }
            if (customerSettings.FaxRequired && customerSettings.FaxEnabled)
            {
                RuleFor(x => x.Fax).NotEmpty().WithMessage(translationService.GetResource("Account.Fields.Fax.Required"));
            }
        }
    }
}