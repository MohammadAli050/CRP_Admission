using DAL.ViewModels;
using DATAMANAGER;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace Admission.Controllers
{
    [RoutePrefix("api/AdmissionToUCAMImage")]
    public class AdmissionToUCAMImageController : ApiController
    {
        string url = System.Web.HttpContext.Current.Request.RawUrl;
        string clientIpAddress = HttpContext.Current.Request.UserHostAddress;
        string isLiveServerString = WebConfigurationManager.AppSettings["IsLiveServer"];
        string vId = WebConfigurationManager.AppSettings["AdmissionToUCAMImageTransferVId"];

        public class CandidateImageRequestModel
        {
            public string VID { get; set; }
            public string AdmissionTestRoll { get; set; }
            public int AcaCalId { get; set; }
        }


        /// <summary>
        /// AdmissionDocumentTypeId = 2 = Photo
        /// AdmissionDocumentTypeId = 3 = Signature
        /// </summary>
        public class ImageInfo
        {
            public long CandidateId { get; set; }
            public int AdmissionDocumentTypeId { get; set; }
            public string ImageExtension { get; set; }
            public string ImageName { get; set; }
            public byte[] ImageData { get; set; }
        }


        [HttpPost]
        [Route("GetPhotoSignature")]
        public IHttpActionResult GetPhotoSignature(CandidateImageRequestModel model)
        {
            ResponseAPI responseAPI = new ResponseAPI();
            
            try
            {
                if ((!string.IsNullOrEmpty(model.AdmissionTestRoll) && Convert.ToInt64(model.AdmissionTestRoll) > 0) && 
                    (model.VID.ToLower() == vId.ToLower()))
                {
                    long CandidateId = -1;
                    DAL.AdmissionTestRoll admTestRoll = null;
                    using (var db = new CandidateDataManager())
                    {
                        admTestRoll = db.AdmissionDB.AdmissionTestRolls.Where(x => x.AcaCalID == model.AcaCalId
                                                                                && x.TestRoll == model.AdmissionTestRoll).FirstOrDefault();
                    }

                    if (admTestRoll != null)
                    {
                        CandidateId = (long)admTestRoll.CandidateID;
                    }
                    

                    List<DAL.Document> dicumentList = null;
                    using (var db = new CandidateDataManager())
                    {
                        dicumentList = db.GetAllDocumentByCandidateID_AD(CandidateId);
                    }

                    if (dicumentList != null && dicumentList.Count > 0)
                    {
                        List<ImageInfo> imgInfoList = new List<ImageInfo>();

                        foreach (var tData in dicumentList)
                        {
                            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(tData.DocumentDetail.URL)))
                            {
                                string fileName = tData.DocumentDetail.Name;
                                string fileExtension = Path.GetExtension(fileName);
                                string filePath = System.Web.HttpContext.Current.Server.MapPath(tData.DocumentDetail.URL);

                                ////Type 1, convert image into binary
                                byte[] imagebyte = File.ReadAllBytes(filePath);

                                ImageInfo imgInfoModel = new ImageInfo();
                                imgInfoModel.CandidateId = CandidateId;
                                imgInfoModel.AdmissionDocumentTypeId = tData.DocumentTypeID;
                                imgInfoModel.ImageExtension = fileExtension;
                                imgInfoModel.ImageName = fileName;
                                imgInfoModel.ImageData = imagebyte;

                                imgInfoList.Add(imgInfoModel);
                            }

                        }


                        if (imgInfoList != null && imgInfoList.Count > 0)
                        {
                            responseAPI.ResponseCode = 200;
                            responseAPI.ResponseStatus = "Success";
                            responseAPI.ResponseMessage = "";
                            responseAPI.ResponseData = imgInfoList;
                        }
                        else
                        {
                            responseAPI.ResponseCode = 400;
                            responseAPI.ResponseStatus = "Failed";
                            responseAPI.ResponseMessage = "No Image and Signature found for Candidate!";
                            responseAPI.ResponseData = "";
                        }
                       

                    }
                    else
                    {
                        responseAPI.ResponseCode = 400;
                        responseAPI.ResponseStatus = "Failed";
                        responseAPI.ResponseMessage = "No Candidate Document Found!";
                        responseAPI.ResponseData = "";
                    }
                }
                else
                {
                    responseAPI.ResponseCode = 400;
                    responseAPI.ResponseStatus = "Failed";
                    responseAPI.ResponseMessage = "Invalid request!";
                    responseAPI.ResponseData = "";
                }
            }
            catch (Exception ex)
            {
                responseAPI.ResponseCode = 400;
                responseAPI.ResponseStatus = "Failed";
                responseAPI.ResponseMessage = "Exception; Error: " + ex.Message.ToString();
                responseAPI.ResponseData = "";
            }



            return Ok(responseAPI);
        }




        }
}
