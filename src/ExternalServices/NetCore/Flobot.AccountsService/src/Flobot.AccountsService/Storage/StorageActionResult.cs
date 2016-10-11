using System.Collections.Generic;
using System.Linq;

namespace Flobot.AccountsService.Storage
{
    public class StorageActionResult
    {
        public ResultType Result { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public StorageActionResult(ResultType result)
            : this(result, Enumerable.Empty<string>())
        {
        }

        public StorageActionResult(ResultType result, IEnumerable<string> errors)
        {
            Result = result;
            Errors = errors;
        }
    }
}
