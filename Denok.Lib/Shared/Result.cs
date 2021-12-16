using System;

namespace Denok.Lib.Shared
{
    public sealed class Result<T, E>  
        where E : class
    {

        private T value;
        private E error;

        private Result() {}

        private Result(T value, E error)
        {
            this.value = value;
            this.error = error;
        }

        public static Result<T, E> From(T value, E error)
        {
            return new Result<T, E>(value, error);
        }

        public bool IsEmpty()
        {
            return value == null;
        }

        public bool IsError()
        {
            return error != null;
        }

        public T Get()
        {
            return value;
        }

        public E Error()
        {
            return error;
        }
    }
}