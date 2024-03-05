using Microsoft.AspNetCore.Mvc;

namespace FinalWebApi.Tests.Unit.Controllers
{
    // This class is aimed to simplify ActionResult typecasting in Test Method.
    // Note: for now is not used. But actually works if you want to use it.
    // Of course this can be improved to be easier to use.
    public class TestHelper
    {
        public static TValue GetActionResultValue<TAction, TValue>(ActionResult<TValue> result) where TAction : ObjectResult
        {
            return (TValue)((TAction)result.Result).Value;
        }
    }
}