using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Admission.Controllers
{
    public class TestCustomeApiController : ApiController
    {
        //// Link : localhost/api/TestCustomeApi?id=1&firstName=Ariq&lastName=Rahman
        //[HttpGet]
        //public IEnumerable<string> Get(int id = 0, string firstName = null, string lastName = null)
        //{
        //    return new string[] { "ID : "+ id, "First Name : "+ firstName, "Last Name : "+ lastName };
        //}


        public class TelitalkEducationSubjectModel
        {
            public string subCode { get; set; }
            public string subName { get; set; }
            public string grade { get; set; }
            public decimal gpoint { get; set; }
            public decimal mark { get; set; }
        }
        public class TelitalkEducationResultModel
        {
            public int responseCode { get; set; }
            public string responseDesc { get; set; }
            public string board { get; set; }
            public string rollNo { get; set; }
            public int passYear { get; set; }
            public string name { get; set; }
            public string father { get; set; }
            public string mother { get; set; }
            public string regNo { get; set; }
            public string gender { get; set; }
            public string result { get; set; }
            public decimal gpa { get; set; }
            public decimal gpaExc4th { get; set; }
            public string sscBoard { get; set; }
            public int sscPassYear { get; set; }
            public string sscRoll { get; set; }
            public string sscRegNo { get; set; }
            public string studGroup { get; set; }
            public string eiin { get; set; }
            public decimal totalObtMark { get; set; }
            public decimal totalExc4TH { get; set; }
            public string iName { get; set; }
            public string cCode { get; set; }
            public string cName { get; set; }
            public string thana { get; set; }
            public string sub4thCode { get; set; }
            public List<TelitalkEducationSubjectModel> subject { get; set; }
        }



        // GET api/<controller>/5
        [HttpGet]
        [Route("api/getdata")]
        public IHttpActionResult Get()
        {
            

            string test = "{\"responseCode\":\"1\",\"responseDesc\":\"Success\",\"board\":\"DHAKA\",\"rollNo\":\"123456\",\"passYear\":\"2018\",\"name\":\"BADIUL ALAM BHUIYAN\",\"father\":\"AKTEROZZAMAN BHUIYAN\",\"mother\":\"RAHANA AKTER\",\"regNo\":\"1310539095\",\"gender\":\"MALE\",\"result\":\"P\",\"gpa\":\"4.83\",\"gpaExc4th\":\"4.33\",\"sscBoard\":\"DHAKA\",\"sscPassYear\":\"2016\",\"sscRoll\":\"127688\",\"sscRegNo\":\"1310539095\",\"studGroup\":\"SCIENCE\",\"eiin\":\"133965\",\"totalObtMark\":\"0922\",\"totalExc4TH\":\"0838\",\"iName\":\"DR. MAHBUBUR RAHMAN MOLLAH COLLEGE\",\"cCode\":\"120\",\"cName\":\"DHAKA - 23, SHAYMPUR MODEL SCHOOL & COLLEGE\",\"thana\":\"DEMRA\",\"sub4thCode\":\"178\",\"subject\":[{\"subCode\":\"101\",\"subName\":\"BANGLA\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"157\"},{\"subCode\":\"107\",\"subName\":\"ENGLISH\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"142\"},{\"subCode\":\"275\",\"subName\":\"INFORMATION & COMMUNICATION TECHNOLOGY\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"082\"},{\"subCode\":\"174\",\"subName\":\"PHYSICS\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"151\"},{\"subCode\":\"176\",\"subName\":\"CHEMISTRY\",\"grade\":\"A\",\"gpoint\":\"4.00\",\"mark\":\"142\"},{\"subCode\":\"265\",\"subName\":\"HIGHER MATHEMATICS\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"164\"},{\"subCode\":\"178\",\"subName\":\"BIOLOGY\",\"grade\":\"A+\",\"gpoint\":\"5.00\",\"mark\":\"164\"}]}";
            TelitalkEducationResultModel term = JsonConvert.DeserializeObject<TelitalkEducationResultModel>(test);

            return Ok(term);
        }

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}