using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FindTheNumberWebAPI.Models;
using System;

namespace FindTheNumberWebAPI.Controllers
{
    public enum OkResult
    {
        Smaller,
        Bigger
    }



    [ApiController]
    [Route("api/[controller]")]
    public class TheNumberController : ControllerBase
    {
        static int theNumber;
        static int tryCount;

        [HttpGet("{number}")]
        public ActionResult<TryResult> Try(int number)
        {
            try
            {
                if (theNumber == 0)
                {
                    RollTheNumber();
                    tryCount = 0;
                }

                if (tryCount == 20)
                {
                    ResetNumbers();
                    return this.StatusCode(StatusCodes.Status205ResetContent, "You reached 20 tries without find The Number => You should Modify your algorithm and try again !");
                }

                return CompareNumbers(number);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal server failure");
            }
        }

        ObjectResult CompareNumbers(int number)
        {
            if (theNumber < number)
            {
                return Ok(new TryResult { Result = OkResult.Smaller.ToString(), TryNumber = ++tryCount });
            }
            else if (theNumber > number)
            {
                return Ok(new TryResult { Result = OkResult.Bigger.ToString(), TryNumber = ++tryCount });
            }
            else
            {
                theNumber = 0;
                return this.Accepted(new TryResult { Result = "Your Algorithm succeeds ! Congratulations", TryNumber = ++tryCount });
            }
        }

        void RollTheNumber()
        {
            theNumber = new Random().Next(1, 50000);
        }

        void ResetNumbers()
        {
            theNumber = tryCount = 0;
        }
    }
}
