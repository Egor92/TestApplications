﻿namespace SendArchives
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
