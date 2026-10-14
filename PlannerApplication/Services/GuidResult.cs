namespace PlannerApplication.Services
{
    public class GuidResult
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        public Guid Id;

        public GuidResult(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public GuidResult(Guid id, bool isSuccess, string errormessage)
        {
            this.Id = id;
            this.IsSuccess = isSuccess;
            this.ErrorMessage = errormessage;
        }

        /// <summary>
        /// Returns a result instance with true 
        /// </summary>
        /// <returns>Result</returns>
        public static GuidResult Success(Guid id)
        {
            return new GuidResult(id, true, string.Empty);
        }

        /// <summary>
        /// Returns a result instance with false along with the errormessage.
        /// </summary>
        /// <param name="errorMessage">errormessage</param>
        /// <returns>Result</returns>
        public static GuidResult Failure(string errorMessage)
        {
            return new GuidResult(false, errorMessage);
        }
    }
}
