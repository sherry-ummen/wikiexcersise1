using System;

namespace Wiki {

    public class Result {

        public bool IsSuccess { get; internal set; }

        public string Error { get; internal set; }

        public bool IsFailure
        {
            get { return !IsSuccess; }
            internal set { IsSuccess = !value; }
        }

        protected Result(bool isSuccess, string error) {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException();
            if (!isSuccess && error == string.Empty)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(string message) {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message) {
            return new Result<T>(default(T), false, message);
        }

        public static Result Ok() {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value) {
            return new Result<T>(value, true, string.Empty);
        }

    }

    public class Result<T> : Result {
        private T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _value;
            }
            internal set { _value = value; }
        }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error) {
            _value = value;
        }
    }
}
