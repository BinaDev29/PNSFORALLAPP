//using ECX.HR.Application.DTOs.Common;
//using ECX.HR.Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ECX.HR.Application.DTOs.Addresss
//{
//    public class AddressDto : BaseDtos
//    {
//        public int PId { get; set; }
//        public Guid Id { get; set; }
//        public Guid EmpId { get; set; }

//        // These properties are non-nullable but don't have an initial value.
//        // The 'required' keyword ensures they are initialized when a new object is created.
//        public required string Region { get; set; }
//        public required string Town { get; set; }
//        public required string SubCity { get; set; }
//        public required string Kebele { get; set; }
//        public required string HouseNo { get; set; }
//        public required string PhoneNumber { get; set; }
//        public int PostCode { get; set; }
//        public required string Email { get; set; }
//        public int Status { get; set; }
//    }
//}