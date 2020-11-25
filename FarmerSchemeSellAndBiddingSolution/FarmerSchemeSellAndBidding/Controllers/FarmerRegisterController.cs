﻿using FarmerSchemeSellAndBidding.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FarmerSchemeSellAndBidding.Controllers
{
    public class FarmerRegisterController : ApiController
    {
        FarmerSchemeDBEntities4 db = new FarmerSchemeDBEntities4();
        
        //Post:api/FarmerRegister
        [HttpPost]
        public IHttpActionResult PostFRegistration()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                string rt = "FARMER";
                string ueid = httpRequest["Emailid"];

                dynamic emailid=(from r in db.RollTypes
                                where r.RollType1.ToUpper() == rt && r.UserEmailId.ToLower() == ueid.ToLower()
                                select r.UserEmailId).ToList();
                if (emailid.Count != 0)
                {
                    return Ok(ueid + " is already used by another farmer");
                }
                else
                {
                    string imageName1 = null;
                    //Upload Image
                    var postedFile1 = httpRequest.Files["Image1"];
                    //Create custom filename
                    imageName1 = new String(Path.GetFileNameWithoutExtension(postedFile1.FileName).Take(10).ToArray()).Replace(" ", "-");
                    imageName1 = imageName1 + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile1.FileName);
                    var filePath1 = HttpContext.Current.Server.MapPath("~/Image/" + imageName1);
                    

                    string imageName2 = null;
                    //var httpRequest = HttpContext.Current.Request;
                    //Upload Image
                    var postedFile2 = httpRequest.Files["Image2"];
                    //Create custom filename
                    imageName2 = new String(Path.GetFileNameWithoutExtension(postedFile2.FileName).Take(10).ToArray()).Replace(" ", "-");
                    imageName2 = imageName2 + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile2.FileName);
                    var filePath2 = HttpContext.Current.Server.MapPath("~/Image/" + imageName2);
                    

                    string imageName3 = null;
                    //var httpRequest = HttpContext.Current.Request;
                    //Upload Image
                    var postedFile3 = httpRequest.Files["Image3"];
                    //Create custom filename
                    imageName3 = new String(Path.GetFileNameWithoutExtension(postedFile3.FileName).Take(10).ToArray()).Replace(" ", "-");
                    imageName3 = imageName3 + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile3.FileName);
                    var filePath3 = HttpContext.Current.Server.MapPath("~/Image/" + imageName3);
                    

                    //Adding data into table userRegister
                    UserRegister userRegister = new UserRegister();
                    userRegister.UserEmailId = ueid;
                    userRegister.password = httpRequest["Password"];
                    userRegister.ContactNo_ = httpRequest["ContactNo"];
                    userRegister.fullname = httpRequest["Fullname"];
                    userRegister.address1 = httpRequest["Address1"];
                    userRegister.address2 = httpRequest["Address2"];
                    userRegister.city = httpRequest["City"];
                    userRegister.state = httpRequest["State"];
                    userRegister.pincode = (int?)Convert.ToInt64(httpRequest["Pincode"]);
                    userRegister.pancardDocument = filePath2;
                    userRegister.aadharCardDocument = filePath1;


                    //Adding data into table rollType
                    RollType rollType = new RollType();
                    //rollType.UserID= Convert.ToInt32(httpRequest["Emailid"]);
                    rollType.RollType1 = "FARMER";
                    rollType.certificate = filePath3;
                    rollType.ApprovedStatus = null;
                    rollType.UserEmailId = ueid;

                    //Adding data into table farmerLand
                    FarmerLand farmerLand = new FarmerLand();
                    farmerLand.landArea = httpRequest["LArea"];
                    farmerLand.landAddress = httpRequest["LAddress"];
                    farmerLand.landPinCode = (int?)Convert.ToInt64(httpRequest["LPincode"]);

                    //Adding data into table bankDetail
                    BankDetail bankDetail = new BankDetail();
                    bankDetail.IFSCCode = httpRequest["IFSCCode"]; ;
                    bankDetail.BankAccountNo = (int?)Convert.ToInt64(httpRequest["BankAccountno"]);


                    //saving Images
                    postedFile1.SaveAs(filePath1);
                    postedFile2.SaveAs(filePath2);
                    postedFile3.SaveAs(filePath3);


                    // saving data into table userRegister
                    db.UserRegisters.Add(userRegister);
                    db.SaveChanges();

                    // saving data into table rolltype
                    db.RollTypes.Add(rollType);
                    db.SaveChanges();

                    //getting userid as foreingn key for other table  
                    int uid = (from r in db.RollTypes
                               where r.RollType1.ToUpper() == rt && r.UserEmailId.ToLower() == ueid.ToLower()
                               select r.UserID).First();

                    //saving farmer land table data
                    farmerLand.UserID = uid;
                    db.FarmerLands.Add(farmerLand);
                    db.SaveChanges();

                    //saving data of bank details
                    bankDetail.UserID = uid;
                    db.BankDetails.Add(bankDetail);
                    db.SaveChanges();

                    return Ok("Registration Successfull. Use "+uid+" for Login Userid");
                }
            }
            catch(Exception e)
            {
                return Ok(e.Message);
            }
            
        }
    }
}