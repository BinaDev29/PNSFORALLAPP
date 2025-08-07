//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Text.RegularExpressions;
//using ECX.HR.Application.DTOs.Addresss;
//using System.ComponentModel.DataAnnotations;
//using System.Numerics;
//using Newtonsoft.Json.Linq;

//namespace ECX.HR.Application.DTOs.Address.Validator
//{
//    public class AddressDtoValidator : AbstractValidator<AddressDto>
//    {
//        public AddressDtoValidator()
//        {
//            RuleFor(p => p.Region)
//                .NotEmpty().WithMessage("{PropertyName} is requiered.")
//                .NotNull();
//            RuleFor(p => p.Town)
//                .NotEmpty().WithMessage("{PropertyName} is requiered.")
//                .NotNull();
//            RuleFor(p => p.PhoneNumber)
//                .Must(IsPhoneValid).WithMessage("Please specify a valid PhoneNumber");
//            RuleFor(p => p.Email)
//                .Must(IsEmailValid).WithMessage("please specify a valid Email");
//        }

//        public static bool IsPhoneValid(string PhoneNumber)
//        {
//            string regex = @"^([0])?[1-9][0-9]{8}$";
//            if (PhoneNumber != null)
//                return Regex.IsMatch(PhoneNumber, regex);
//            else return false;
//        }
//        private static readonly Regex EmailRegex = new Regex(
//        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
//        RegexOptions.Compiled | RegexOptions.IgnoreCase
//         );

//        public static bool IsEmailValid(string Email)
//        {
//            var email = Email as string;
//            if (email != null)
//                return EmailRegex.IsMatch(email);
//            else return false;
//        }
//    }
//}

